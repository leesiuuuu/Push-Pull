using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    InputPlayer player;

    public Transform Target;

    public Vector3 addtargetPos = new Vector2(19f, 0);
    Vector3 StartPos = new Vector2(0.19f, -0.17f);

    public float moveSpeed;

    public bool holdGrab;
    public bool grabing;
    public bool targetingable;

    public bool GrabPlayer = false;


    private void Awake()
    {
        player = GetComponentInParent<InputPlayer>();
    }

    private void Start()
    {
        transform.localPosition = StartPos;
        holdGrab = false;
        moveSpeed = 5.5f;
        grabing = false;
        targetingable = true;
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            return;

        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= 1)
        {
            holdGrab = false;
            targetingable = true;
            Target = null;
        }

        if (holdGrab)
        {
            if (grabing)
            {
                Target.transform.position = gameObject.transform.position;
            }
        }
    }

    public void DOGrab()
    {
        if (grabing == false)
        {
            StartCoroutine(GoGrab());
            grabing = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponentInChildren<Grab>().GrabPlayer)
            {
                GrabPlayer = true;
                if (grabing)
                {
                    if (targetingable)
                    {
                        Target = collision.gameObject.GetComponent<Transform>();
                        targetingable = false;
                        StopAllCoroutines();
                        StartCoroutine(BackGrab());
                    }
                    holdGrab = true;
                }
            }
        }
        else if (collision.gameObject.CompareTag("interactive"))
        {
            if (grabing)
            {
                if (targetingable)
                {
                    Target = collision.gameObject.GetComponent<Transform>();
                    targetingable = false;
                    StopAllCoroutines();
                    StartCoroutine(BackGrab());
                }
                holdGrab = true;
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            StopAllCoroutines();
            StartCoroutine(BackGrab());
        }
    }

    public IEnumerator GoGrab()
    {
        //SoundManager.Instance.SFXPlay("PlayerPull_1", player.player1Sounds[(int)PlayerSounds.Pull]);

        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = startPos + addtargetPos;
        float duration = Vector3.Distance(startPos, targetPos) / moveSpeed;
        float t = 0f;
        float threshold = 0.01f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t * moveSpeed);

            if (Vector3.Distance(transform.localPosition, targetPos) < threshold)
            {
                break;
            }
            yield return null;
        }

        StartCoroutine(BackGrab());
    }

    IEnumerator BackGrab()
    {
        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = StartPos;
        float duration = Vector3.Distance(startPos, targetPos) / moveSpeed;
        float t = 0f;
        float threshold = 0.01f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t * moveSpeed);

            if (Vector3.Distance(transform.localPosition, targetPos) < threshold)
            {
                GrabPlayer = false;
                break;
            }
            yield return null;
        }
        grabing = false;
    }
}
