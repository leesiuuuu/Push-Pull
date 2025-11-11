using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    Rigidbody2D rigid;
    PushGlove glove;

    Player2Grab grab;
    GameObject Grab;

    public float moveSpeed = 4f;
    public float x;

    public bool jumpAble = true;
    public bool Push;

    public bool flip;

    public float pushCharge = 0f;      
    public float maxPushCharge = 35f;  
    public float chargeTime = 1f;
    private bool isCharging = false;

    private float RPressTime = 0f;
    [SerializeField] private float RotSpeed = 1f;
    private float maxAngle = 20f;

    private void Awake()
    {
        glove = GetComponentInChildren<PushGlove>();
        rigid = GetComponent<Rigidbody2D>();

        grab = GetComponentInChildren<Player2Grab>();

        Grab = GameObject.Find("Grab2");

    }

    private void Update()
    {
        if (!grab.grabing)
        {
            GrabRot();
        }

        glove.player2PushPower = pushCharge;

        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpAble)
        {
            rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
            jumpAble = false;
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {
            isCharging = true;
            pushCharge = 0f;
        }
        if (Input.GetKey(KeyCode.Slash) && isCharging)
        {
            pushCharge += (maxPushCharge / chargeTime) * Time.deltaTime;
            if (pushCharge > maxPushCharge)
            {
                pushCharge = maxPushCharge;
            }
        }
        if (Input.GetKeyUp(KeyCode.Slash) && isCharging)
        {
            isCharging = false;
            Push = true;
        }
    }

    private void FixedUpdate()
    {
        if (!grab.grabing)
        {
            x = Input.GetAxisRaw("Horizontal2");
            if (x > 0 && flip)
            {
                Flip();
            }
            else if (x < 0 && !flip)
            {
                Flip();
            }
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(moveSpeed * -1f * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
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
        if (Input.GetKeyDown(KeyCode.Period))
        {
            RPressTime = Time.time;
            Grab.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.Period))
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

        if (Input.GetKeyUp(KeyCode.Period))
        {
            Grab.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
