using System.Collections.Generic;
using UnityEngine;

public class NewPlayer1 : MonoBehaviour
{
    Rigidbody2D rigid;
    NewPushGlove glove;
    NewPlayer1Grab grab;
    GameObject Grab;

    Animator anim;

    public float moveSpeed = 4f;
    public int x;
    public bool jumpAble = true;
    public bool Push;
    public bool flip;
    public float pushCharge = 0f;
    public float maxPushCharge = 35f;
    public float chargeTime = 1f;
    private bool isCharging = false;

    public bool cantMove;

    private float RPressTime = 0f;
    [SerializeField] private float RotSpeed = 1f;
    public float maxAngle = 20f;

    [Header("Sound Clips")]
    public List<AudioClip> player1Sounds = new List<AudioClip>();

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = transform.parent.GetComponent<Animator>();
        glove = GetComponentInChildren<NewPushGlove>();
        grab = GetComponentInChildren<NewPlayer1Grab>();
        Grab = GameObject.Find("Grab1");
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            return;

        if (cantMove)
            return;
        if (!grab.grabing)
        {
            GrabRot();
        }

        glove.player1PushPower = pushCharge;

        //PlayerPrefs로 받아올 수 있도록 수정함
        // KeySetting.IsGamePad -> PlayerPrefs.GetInt("ControlMode") == 1
        if (!KeySetting.IsGamePad)
        {
            if (Input.GetKeyDown(KeySetting.player1Keys[KeyAction.PUSH]))
            {
                isCharging = true;
                pushCharge = 0f;
            }
            if (Input.GetKey(KeySetting.player1Keys[KeyAction.PUSH]) && isCharging)
            {
                pushCharge += (maxPushCharge / chargeTime) * Time.deltaTime;
                if (pushCharge > maxPushCharge)
                {
                    pushCharge = maxPushCharge;
                }
            }
            if (Input.GetKeyUp(KeySetting.player1Keys[KeyAction.PUSH]) && isCharging)
            {
                isCharging = false;
                Push = true;
                SoundManager.Instance.SFXPlay("PlayerPush_1", player1Sounds[(int)PlayerSounds.Push]);
            }

            if (Input.GetKeyDown(KeySetting.player1Keys[KeyAction.Jump]) && jumpAble)
            {
                rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
                jumpAble = false;
                SoundManager.Instance.SFXPlay("PlayerJump_1", player1Sounds[(int)PlayerSounds.Jump]);
                anim.Play("Jump");
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump1") && jumpAble)
            {
                rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
                jumpAble = false;
                SoundManager.Instance.SFXPlay("PlayerJump_1", player1Sounds[(int)PlayerSounds.Jump]);
                anim.Play("Jump");
            }

            if (Input.GetButtonDown("Push1"))
            {
                isCharging = true;
                pushCharge = 0;
            }
            if (Input.GetButton("Push1") && isCharging)
            {

                pushCharge += (maxPushCharge / chargeTime) * Time.deltaTime;
                if (pushCharge > maxPushCharge)
                {
                    pushCharge = maxPushCharge;
                }
            }
            if (Input.GetButtonUp("Push1") && isCharging)
            {
                isCharging = false;
                Push = true;
                SoundManager.Instance.SFXPlay("PlayerPush_1", player1Sounds[(int)PlayerSounds.Push]);
            }
        }
    }
    

    private void FixedUpdate()
    {
        if (Time.timeScale == 0)
            return;

        bool left = Input.GetKey(KeySetting.player1Keys[KeyAction.LEFT]);
        bool right = Input.GetKey(KeySetting.player1Keys[KeyAction.RIGHT]);

        if (Time.timeScale == 0)
            return;
        if (cantMove)
            return;
        if (!grab.grabing)
        {
            if (x > 0 && flip)
            {
                Flip();
            }
            else if (x < 0 && !flip)
            {
                Flip();
            }
        }

        if (!KeySetting.IsGamePad)
        {
            if (right && !left) x = 1;
            else if (left && !right) x = -1;
            else x = 0;

            if (Input.GetKey(KeySetting.player1Keys[KeyAction.LEFT]))
            {
                transform.Translate(moveSpeed * -1f * Time.deltaTime, 0, 0);
                if (jumpAble)
                    anim.Play("Move");
            }
            else if (Input.GetKey(KeySetting.player1Keys[KeyAction.RIGHT]))
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
                if (jumpAble)
                    anim.Play("Move");
            }
            else
            {
                if (jumpAble)
                    anim.Play("Idle");
            }
        }
        else
        {
            x = (int)Input.GetAxisRaw("JoyStick1");
            transform.Translate(x * moveSpeed * Time.deltaTime, 0, 0);
            if (jumpAble && x != 0)
                anim.Play("Move");
            else if (x == 0)
                anim.Play("Idle");
        }
    }

    public void Flip()
    {
        flip = !flip;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void GrabRot()
    {
        if (!KeySetting.IsGamePad)
        {
            if (Input.GetKeyDown(KeySetting.player1Keys[KeyAction.PULL]))
            {
                RPressTime = Time.time;
                Grab.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (Input.GetKey(KeySetting.player1Keys[KeyAction.PULL]))
            {
                if (Time.time - RPressTime >= .5f)
                {
                    float elapsed = Time.time - RPressTime - .5f;
                    float angle = Mathf.Sin(elapsed * RotSpeed) * maxAngle;
                    Grab.transform.rotation = Quaternion.Euler(0, 0, angle);
                }
                else
                {
                    Grab.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            /*
            if (Input.GetKeyUp(KeySetting.player1Keys[KeyAction.PULL]))
            {
                Grab.transform.rotation = Quaternion.Euler(0, 0, 0);
            }*/
        }
        else
        {
            if (Input.GetButtonDown("Pull1"))
            {
                RPressTime = Time.time;
                Grab.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (Input.GetButton("Pull1"))
            {
                Debug.Log($"{Time.time - RPressTime}");

                if (Time.time - RPressTime >= .5f)
                {
                    float elapsed = Time.time - RPressTime - .5f;
                    float angle = Mathf.Sin(elapsed * RotSpeed) * maxAngle;
                    Grab.transform.rotation = Quaternion.Euler(0, 0, angle);
                }
                else
                {
                    Grab.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }

            if (Input.GetButtonUp("Pull1"))
            {
                SoundManager.Instance.SFXPlay("PlayerPull_1", player1Sounds[(int)PlayerSounds.Pull]);
            }
/*
            if (Input.GetButtonUp("Pull1"))
            {
                Grab.transform.rotation = Quaternion.Euler(0, 0, 0);
            }*/
        }
    }

    public void Die()
    {
        SoundManager.Instance.SFXPlay("PlayerDied_1", player1Sounds[(int)PlayerSounds.Die]);
        anim.Play("Die");
        cantMove = true;
    }

    public void Cleared()
    {
        cantMove = true;
        anim.Play("Cleared");
    }
}
