using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class InputPlayer : NetworkBehaviour
{
    [SerializeField] private ExChargeUi UI;
    [SerializeField] private SoundManager soundManager;

    public PlayerInput PlayerInput;
    public Transform GrabObject;
    public PushGlove PushGlove;
    public Grab GrabGlove;

    [Header("Animators")]
    public Animator Anim;

    public List<AudioClip> PlayerSounds = new List<AudioClip>();

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool moveLeft = false;
    private bool moveRight = false;
    private bool moving = false;    
    public float moveSpeed = 4f;
    [SerializeField] private bool flip;
    [SerializeField] private float flipThreshold = 0.2f;
    public bool cantMove = false;
    public bool jumpAble = true;

    [Header("Push / Charge")]
    public float MaxPushCharge = 35f;
    public float ChargeTime = 1f;
    public float PushCharge = 0f;
    private bool isCharging = false;
    public bool Push = false;

    public bool PushHeld { get; private set; } = false;
    public bool GrabHeld { get; private set; } = false;

    [Header("Grab / Pull")]
    public float RotSpeed = 1f;
    public float maxAngle = 20f;
    private float RPressTime = 0f;
    private float aimSmooth = 8f;
    private bool isGrabHolding = false;
    private Vector2 grabControlInput = Vector2.zero;
    public float grabDeadzone = 0.15f;

    public string ControlScheme = "Keyboard, Mouse";

    private string lastAnimName = "";

    [SyncVar(hook = nameof(OnFlipChanged))]
    private bool syncFlip = false;

    [SyncVar(hook = nameof(OnAnimChanged))]
    private string syncAnimName = "";


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (Anim == null) Anim = GetComponent<Animator>();
    }

    public override void OnStartLocalPlayer()
    {
        if (PlayerInput != null) PlayerInput.enabled = true;
        if (rb != null) rb.isKinematic = false;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!isLocalPlayer)
        {
            if (PlayerInput != null) PlayerInput.enabled = false;
        }
    }

    void OnEnable()
    {
        if (PlayerInput != null) ControlScheme = PlayerInput.currentControlScheme;
    }


    void Update()
    {
        if (!isLocalPlayer) return;
        if (Time.timeScale == 0f) return;
        if (cantMove) return;

        if (moveLeft || moveRight)  moving = true;
        else moving = false;

        if (PushHeld) UI.OnPush();
        else UI.OffPush();

        if (GrabHeld) UI.OnGrab();
        else UI.OffGrab();

        if (PlayerInput != null) ControlScheme = PlayerInput.currentControlScheme;

        if (isCharging)
        {
            float chargeRate = (ChargeTime > 0f) ? (MaxPushCharge / ChargeTime) : MaxPushCharge;
            PushCharge += chargeRate * Time.deltaTime;
            if (PushCharge > MaxPushCharge) PushCharge = MaxPushCharge;
        }

        if (PushGlove != null) PushGlove.PushPower = PushCharge;

        if (GrabGlove != null && !GrabGlove.grabing) UpdateGrabRotation();

        if (jumpAble)
        {
            if (Mathf.Abs(moveInput.x) > 0.01f) PlayAnim("Move");
            else PlayAnim("Idle");
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        if (Time.timeScale == 0f) return;
        if (cantMove) return;

        if (moving)
        {
            Vector3 move = new Vector3(moveInput.x * moveSpeed * Time.deltaTime, 0f, 0f);
            transform.Translate(move);
        }
        
        if (Mathf.Abs(moveInput.x) > flipThreshold && GrabGlove != null && !GrabGlove.grabing)
        {
            if (moveInput.x > 0f && flip) Flip();
            else if (moveInput.x < 0f && !flip) Flip();
        }
    }

    // ───────────────────────────────────────────
    // 애니메이션 동기화
    // ───────────────────────────────────────────

    private void OnAnimChanged(string oldVal, string newVal)
    {
        if (isLocalPlayer) return;
        PlayAnimLocal(newVal);
    }

    public void PlayAnim(string animName)
    {
        PlayAnimLocal(animName);

        if (animName != lastAnimName)
        {
            lastAnimName = animName;
            CmdPlayAnimation(animName);
        }
    }

    private void PlayAnimLocal(string animName)
    {
        if (string.IsNullOrEmpty(animName)) return;
        Anim?.Play(animName);
    }

    [Command]
    private void CmdPlayAnimation(string animName)
    {
        syncAnimName = animName;
    }

    // ───────────────────────────────────────────
    // 플립 동기화
    // ───────────────────────────────────────────

    private void OnFlipChanged(bool oldVal, bool newVal)
    {
        if (isLocalPlayer) return;
        ApplyFlip(newVal);
    }

    private void ApplyFlip(bool isFlipped)
    {
        Vector3 scale = transform.localScale;
        scale.x = isFlipped ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    [Command]
    private void CmdSyncFlip(bool isFlipped)
    {
        syncFlip = isFlipped;
    }

    // ───────────────────────────────────────────
    // 밀치기 장갑 펀치 애니메이션 동기화
    // ───────────────────────────────────────────

    public void SyncPunchAnim()
    {
        CmdPunchAnim();
    }

    [Command]
    private void CmdPunchAnim()
    {
        RpcPunchAnim();
    }

    [ClientRpc]
    private void RpcPunchAnim()
    {
        if (isLocalPlayer) return;
        PushGlove?.DoPunchAnim();
    }

    // ───────────────────────────────────────────
    // 상대 플레이어 끌기
    // ───────────────────────────────────────────

    public void SyncMoveTarget(uint targetNetId, Vector3 targetPos)
    {
        if (isLocalPlayer) CmdMoveTarget(targetNetId, targetPos);
    }

    [Command]
    private void CmdMoveTarget(uint targetNetId, Vector3 targetPos)
    {
        RpcMoveTarget(targetNetId, targetPos);
    }

    [ClientRpc]
    private void RpcMoveTarget(uint targetNetId, Vector3 targetPos)
    {
        if (NetworkClient.spawned.TryGetValue(targetNetId, out NetworkIdentity identity))
            identity.transform.position = targetPos;
    }

    // ───────────────────────────────────────────
    // 밀치기
    // ───────────────────────────────────────────

    public void SyncApplyPush(uint targetNetId, Vector2 dir, float power)
    {
        if (isLocalPlayer) CmdApplyPush(targetNetId, dir, power);
    }

    [Command]
    private void CmdApplyPush(uint targetNetId, Vector2 dir, float power)
    {
        RpcApplyPush(targetNetId, dir, power);
    }

    [ClientRpc]
    private void RpcApplyPush(uint targetNetId, Vector2 dir, float power)
    {
        if (!NetworkClient.spawned.TryGetValue(targetNetId, out NetworkIdentity identity)) return;
        Rigidbody2D rigid = identity.GetComponent<Rigidbody2D>();
        if (rigid == null) return;

        Vector2 impulseVector = dir * power + Vector2.up * power / 2f;
        rigid.AddForce(impulseVector, ForceMode2D.Impulse);
    }

    // ───────────────────────────────────────────
    // 입력 처리
    // ───────────────────────────────────────────

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer) return;
        if (context.started || context.performed) moveLeft = true;
        else if (context.canceled) moveLeft = false;
        UpdateMoveInput();
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer) return;
        if (context.started || context.performed) moveRight = true;
        else if (context.canceled) moveRight = false;
        UpdateMoveInput();
    }

    private void UpdateMoveInput()
    {
        if (moveLeft && moveRight) moveInput = Vector2.zero;
        else if (moveLeft) moveInput = Vector2.left;
        else if (moveRight) moveInput = Vector2.right;
        else moveInput = Vector2.zero;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer) return;
        if (context.performed && jumpAble)
        {
            rb.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
            jumpAble = false;
            SoundManager.Instance?.SFXPlay("PlayerJump_1", PlayerSounds[(int)global::PlayerSounds.Jump]);
            PlayAnim("Jump");
        }
    }

    public void OnPush(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer) return;
        if (context.started)
        {
            PushHeld = true;
            isCharging = true;
            PushCharge = 0f;
        }
        else if (context.canceled)
        {
            PushHeld = false;
            if (isCharging)
            {
                isCharging = false;
                Push = true;
                SoundManager.Instance?.SFXPlay("PlayerPush_1", PlayerSounds[(int)global::PlayerSounds.Push]);
                PushGlove?.DoPunchAnim();
                SyncPunchAnim();
            }
        }
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer) return;
        if (context.started)
        {
            GrabHeld = true;
            isGrabHolding = true;
            RPressTime = Time.time;
            if (GrabObject != null) GrabObject.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (context.canceled)
        {
            GrabHeld = false;
            isGrabHolding = false;
            GrabGlove?.DOGrab();
            SoundManager.Instance?.SFXPlay("PlayerPull_1", PlayerSounds[(int)global::PlayerSounds.Pull]);
            grabControlInput = Vector2.zero;
        }
    }

    public void OnGrabControll(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer) return;
        grabControlInput = context.ReadValue<Vector2>();
    }

    // ───────────────────────────────────────────
    // Grab 회전
    // ───────────────────────────────────────────

    private void UpdateGrabRotation()
    {
        if (GrabObject == null) return;
        if (!isGrabHolding) return;

        bool isKeyboard = ControlScheme != null && ControlScheme.ToLower().Contains("keyboard");

        if (isKeyboard)
        {
            var mouse = Mouse.current;
            Camera cam = Camera.main;
            if (mouse != null && cam != null)
            {
                Vector2 screenPos = mouse.position.ReadValue();
                Vector3 world = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, cam.nearClipPlane));
                world.z = GrabObject.position.z;

                Vector3 dirWorld = world - GrabObject.position;
                Vector3 dirLocal = transform.InverseTransformDirection(dirWorld);

                if (dirLocal.sqrMagnitude > 0.0001f)
                {
                    float rawLocalAngle = Mathf.Atan2(dirLocal.y, dirLocal.x) * Mathf.Rad2Deg;
                    float desiredLocal = Mathf.Clamp(rawLocalAngle, -Mathf.Abs(maxAngle), Mathf.Abs(maxAngle));
                    float currentLocalZ = GrabObject.localEulerAngles.z;
                    float smoothLocalZ = Mathf.LerpAngle(currentLocalZ, desiredLocal, Time.deltaTime * aimSmooth);
                    GrabObject.localRotation = Quaternion.Euler(0f, 0f, smoothLocalZ);
                }
                else ApplyOscillationIfNeeded();
            }
            else ApplyOscillationIfNeeded();
        }
        else
        {
            Vector2 stick = grabControlInput;
            if (stick.magnitude >= grabDeadzone)
            {
                float rawAngle = Mathf.Atan2(stick.y, stick.x) * Mathf.Rad2Deg;
                float desiredLocal = Mathf.Clamp(rawAngle, -Mathf.Abs(maxAngle), Mathf.Abs(maxAngle));
                float currentLocalZ = GrabObject.localEulerAngles.z;
                float smoothLocalZ = Mathf.LerpAngle(currentLocalZ, desiredLocal, Time.deltaTime * aimSmooth);
                GrabObject.localRotation = Quaternion.Euler(0f, 0f, smoothLocalZ);
            }
            else ApplyOscillationIfNeeded();
        }
    }

    private void ApplyOscillationIfNeeded()
    {
        if (GrabObject == null) return;

        if (Time.time - RPressTime >= 0.5f)
        {
            float elapsed = Time.time - RPressTime - 0.5f;
            float angle = Mathf.Sin(elapsed * RotSpeed) * maxAngle;
            float currentLocalZ = GrabObject.localEulerAngles.z;
            float smoothLocalZ = Mathf.LerpAngle(currentLocalZ, angle, Time.deltaTime * aimSmooth);
            GrabObject.localRotation = Quaternion.Euler(0f, 0f, smoothLocalZ);
        }
        else
        {
            float currentLocalZ = GrabObject.localEulerAngles.z;
            float smoothLocalZ = Mathf.LerpAngle(currentLocalZ, 0f, Time.deltaTime * aimSmooth);
            GrabObject.localRotation = Quaternion.Euler(0f, 0f, smoothLocalZ);
        }
    }

    // ───────────────────────────────────────────
    // 유틸
    // ───────────────────────────────────────────

    public void Flip()
    {
        flip = !flip;
        ApplyFlip(flip);
        if (isLocalPlayer) CmdSyncFlip(flip);
    }

    public bool ConsumePush(out float outCharge)
    {
        if (Push)
        {
            outCharge = PushCharge;
            Push = false;
            PushCharge = 0f;
            return true;
        }
        outCharge = 0f;
        return false;
    }

    public void Die()
    {
        if (!isLocalPlayer) return;
        SoundManager.Instance.SFXPlay("PlayerDied_1", PlayerSounds[(int)global::PlayerSounds.Die]);
        cantMove = true;
        PlayAnim("Die");
    }

    public void Cleared()
    {
        if (!isLocalPlayer) return;
        cantMove = true;
        PlayAnim("Cleared");
    }
}