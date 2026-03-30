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
    public Animator Anim;
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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (PlayerInput == null) PlayerInput = GetComponent<PlayerInput>();
        if (Anim == null && transform.parent != null) Anim = transform.parent.GetComponent<Animator>();
    }

    void Start()
    {
        if (!isLocalPlayer)
        {
            if (PlayerInput != null)
                PlayerInput.enabled = false;

            if (rb != null) rb.isKinematic = true;
        }
    }

    void OnEnable()
    {
        if (PlayerInput != null)
            ControlScheme = PlayerInput.currentControlScheme;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (cantMove) return;

        if (!isLocalPlayer) return;

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

        if (!GrabGlove.grabing)
        {
            UpdateGrabRotation();
        }

        if (jumpAble)
        {
            if (Mathf.Abs(moveInput.x) > 0.01f) Anim?.Play("Move");
            else Anim?.Play("Idle");
        }
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;
        if (cantMove) return;

        if (!isLocalPlayer) return;

        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

        if (Mathf.Abs(moveInput.x) > flipThreshold && !GrabGlove.grabing)
        {
            if (moveInput.x > 0f && flip) Flip();
            else if (moveInput.x < 0f && !flip) Flip();
        }
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.started || context.performed) moveLeft = true;
        else if (context.canceled) moveLeft = false;
        UpdateMoveInput();
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
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
        if (context.performed && jumpAble)
        {
            rb.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
            jumpAble = false;
            SoundManager.Instance?.SFXPlay("PlayerJump_1", PlayerSounds[(int)global::PlayerSounds.Jump]);
            Anim?.Play("Jump");
        }
    }

    public void OnPush(InputAction.CallbackContext context)
    {
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
        grabControlInput = context.ReadValue<Vector2>();
    }

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

    private float ClampAngleToMax(float baseDeg, float targetDeg, float maxDeg)
    {
        float delta = Mathf.DeltaAngle(baseDeg, targetDeg);
        float clamped = Mathf.Clamp(delta, -Mathf.Abs(maxDeg), Mathf.Abs(maxDeg));
        return baseDeg + clamped;
    }

    public void Flip()
    {
        flip = !flip;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
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
        SoundManager.Instance.SFXPlay("PlayerDied_1", PlayerSounds[(int)global::PlayerSounds.Die]);
        Anim.Play("Die");
        cantMove = true;
    }

    public void Cleared()
    {
        cantMove = true;
        Anim.Play("Cleared");
    }
}