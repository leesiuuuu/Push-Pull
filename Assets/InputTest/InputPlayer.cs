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
    public Animator Anim;       // 부모 - 이동 / 점프 / Idle / Die / Cleared
    public Animator GloveAnim;  // 자식 - PushGlove 등 장갑 애니메이션

    public List<AudioClip> PlayerSounds = new List<AudioClip>();

    private Rigidbody2D rb;
    Vector2 moveInput;
    private bool moveLeft = false;
    private bool moveRight = false;
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

    // 애니메이션 동기화용
    private string lastAnimName = "";

    // 플립 동기화용
    [SyncVar(hook = nameof(OnFlipChanged))]
    private bool syncFlip = false;

    private static readonly HashSet<string> GloveAnimStates = new HashSet<string>
    {
        "PushGlove",
    };

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (Anim == null && transform.parent != null)
            Anim = transform.parent.GetComponent<Animator>();

        if (GloveAnim == null)
            GloveAnim = GetComponentInChildren<Animator>();
    }

    public override void OnStartLocalPlayer()
    {
        PlayerInput = GetComponent<PlayerInput>();
        if (PlayerInput != null) PlayerInput.enabled = true;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!isLocalPlayer)
        {
            var pi = GetComponent<PlayerInput>();
            if (pi != null) pi.enabled = false;
            if (rb != null) rb.isKinematic = true;
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

        if (!GrabGlove.grabing) UpdateGrabRotation();

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

        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

        if (Mathf.Abs(moveInput.x) > flipThreshold && !GrabGlove.grabing)
        {
            if (moveInput.x > 0f && flip) Flip();
            else if (moveInput.x < 0f && !flip) Flip();
        }
    }

    // ───────────────────────────────────────────
    // 애니메이션 동기화
    // ───────────────────────────────────────────

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
        if (GloveAnimStates.Contains(animName))
        {
            if (GloveAnim == null)
            {
                Debug.LogError("[InputPlayer] GloveAnim이 null입니다! Inspector에서 장갑 오브젝트의 Animator를 연결해주세요.");
                return;
            }
            GloveAnim.Play(animName);
        }
        else
        {
            if (Anim == null)
            {
                Debug.LogError("[InputPlayer] Anim이 null입니다!");
                return;
            }
            Anim.Play(animName);
        }
    }

    [Command]
    private void CmdPlayAnimation(string animName)
    {
        RpcPlayAnimation(animName);
    }

    [ClientRpc]
    private void RpcPlayAnimation(string animName)
    {
        if (isLocalPlayer) return;
        PlayAnimLocal(animName);
    }

    // ───────────────────────────────────────────
    // 플립 동기화
    // ───────────────────────────────────────────

    private void OnFlipChanged(bool oldVal, bool newVal)
    {
        ApplyFlip(newVal);
    }

    // 실제 Scale 반전 적용
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
            GrabGlove.DOGrab();
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