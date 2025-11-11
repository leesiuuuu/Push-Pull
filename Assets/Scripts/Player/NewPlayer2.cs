using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayer2 : MonoBehaviour
{
    Rigidbody2D rigid;
    NewPushGlove glove;

    NewPlayer2Grab grab;
    GameObject Grab;

    Animator anim;

    public float moveSpeed = 4f;
    public int x;

    public bool jumpAble = true;
    public bool Push;

    public bool flip;

    public bool cantMove;

    public float pushCharge = 0f;
    public float maxPushCharge = 35f;
    public float chargeTime = 1f;
    private bool isCharging = false;

    private float RPressTime = 0f;
    [SerializeField] private float RotSpeed = 1f;
    private float maxAngle = 20f;

    [Header("Sound Clips")]
    public List<AudioClip> player2Sounds = new List<AudioClip>();

    private void Awake()
    {
        glove = GetComponentInChildren<NewPushGlove>();
        rigid = GetComponent<Rigidbody2D>();

        anim = transform.parent.GetComponent<Animator>();
        grab = GetComponentInChildren<NewPlayer2Grab>();

        Grab = GameObject.Find("Grab2");

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

        glove.player2PushPower = pushCharge;

        if (!KeySetting.IsGamePad)
        {
            if (Input.GetKeyDown(KeySetting.player2Keys[KeyAction.PUSH]))
            {
                isCharging = true;
                pushCharge = 0f;
            }
            if (Input.GetKey(KeySetting.player2Keys[KeyAction.PUSH]) && isCharging)
            {
                pushCharge += (maxPushCharge / chargeTime) * Time.deltaTime;
                if (pushCharge > maxPushCharge)
                {
                    pushCharge = maxPushCharge;
                }
            }
            if (Input.GetKeyUp(KeySetting.player2Keys[KeyAction.PUSH]) && isCharging)
            {
                isCharging = false;
                Push = true;
                SoundManager.Instance.SFXPlay("PlayerPush_2", player2Sounds[(int)PlayerSounds.Push]);
            }

            if (Input.GetKeyDown(KeySetting.player2Keys[KeyAction.Jump]) && jumpAble)
            {
                rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
                jumpAble = false;
                SoundManager.Instance.SFXPlay("PlayerJump_2", player2Sounds[(int)PlayerSounds.Jump]);
                anim.Play("Jump");
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump2") && jumpAble)
            {
                rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
                jumpAble = false;
				SoundManager.Instance.SFXPlay("PlayerJump_2", player2Sounds[(int)PlayerSounds.Jump]);
				anim.Play("Jump");
            }

            if (Input.GetButtonDown("Push2"))
            {
                Debug.Log("차지 시작");

                isCharging = true;
                pushCharge = 0;
            }
            if (Input.GetButton("Push2") && isCharging)
            {
                Debug.Log("차징 중");

                pushCharge += (maxPushCharge / chargeTime) * Time.deltaTime;
                if (pushCharge > maxPushCharge)
                {
                    pushCharge = maxPushCharge;
                }
            }
            if (Input.GetButtonUp("Push2") && isCharging)
            {
                Debug.Log("차징 끝");

                isCharging = false;
                Push = true;
                SoundManager.Instance.SFXPlay("PlayerPush_2", player2Sounds[(int)PlayerSounds.Push]);

			}
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0)
            return;

        bool left = Input.GetKey(KeySetting.player2Keys[KeyAction.LEFT]);
        bool right = Input.GetKey(KeySetting.player2Keys[KeyAction.RIGHT]);

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

            if (Input.GetKey(KeySetting.player2Keys[KeyAction.LEFT]))
            {
                transform.Translate(moveSpeed * -1f * Time.deltaTime, 0, 0);
                if (jumpAble)
                    anim.Play("Move");
            }
            else if (Input.GetKey(KeySetting.player2Keys[KeyAction.RIGHT]))
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
                if (jumpAble)
                    anim.Play("Move");
            }
            else
            {
                if(jumpAble)
                 anim.Play("Idle");
            }
        }
        else
        {
            x = (int)Input.GetAxisRaw("JoyStick2");
            transform.Translate(x * moveSpeed * Time.deltaTime, 0, 0);

            if (jumpAble && x != 0)
                anim.Play("Move");
            else if(x == 0)
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
            if (Input.GetKeyDown(KeySetting.player2Keys[KeyAction.PULL]))
            {
                RPressTime = Time.time;
                Grab.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (Input.GetKey(KeySetting.player2Keys[KeyAction.PULL]))
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

/*            if (Input.GetKeyUp(KeySetting.player2Keys[KeyAction.PULL]))
            {
                Grab.transform.rotation = Quaternion.Euler(0, 0, 0);
            }*/
        }
        else
        {
            if (Input.GetButtonDown("Pull2"))
            {
                Debug.Log("그랩 차징 시작");
                RPressTime = Time.time;
                Grab.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (Input.GetButton("Pull2"))
            {
                Debug.Log("그랩 차징 중");

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

/*            if (Input.GetButtonUp("Pull2"))
            {
                Grab.transform.rotation = Quaternion.Euler(0, 0, 0);
                Debug.Log("그랩 차징 끝");
            }*/
        }
     }

    public void Die()
    {
        SoundManager.Instance.SFXPlay("PlayerDied_2", player2Sounds[(int)PlayerSounds.Die]);
        anim.Play("Die");
        cantMove = true;
    }

    public void Cleared()
    {
        cantMove = true;
        anim.Play("Cleared");
    }
}
