using System.Collections;
using UnityEngine;

public class Grab : MonoBehaviour
{
    InputPlayer player;

    public Transform Target;

    private uint cachedTargetNetId = 0;
    private bool hasNetworkTarget = false;

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
        if (Time.timeScale == 0) return;

        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= 1)
        {
            holdGrab = false;
            targetingable = true;
            ClearTarget();
        }

        if (holdGrab && grabing && Target != null)
        {
            if (GrabPlayer)
            {
                if (hasNetworkTarget)
                    player.SyncMoveTarget(cachedTargetNetId, gameObject.transform.position);
            }
            else
            {
                Target.transform.position = gameObject.transform.position;
            }
        }
    }

    // ───────────────────────────────────────────
    // Target 캐싱
    // ───────────────────────────────────────────

    private void SetTarget(GameObject targetObj)
    {
        Target = targetObj.transform;

        var netIdentity = targetObj.GetComponentInParent<Mirror.NetworkIdentity>();
        if (netIdentity != null)
        {
            cachedTargetNetId = netIdentity.netId;
            hasNetworkTarget = true;
        }
        else
        {
            cachedTargetNetId = 0;
            hasNetworkTarget = false;
        }
    }

    private void ClearTarget()
    {
        Target = null;
        cachedTargetNetId = 0;
        hasNetworkTarget = false;
    }

    // ───────────────────────────────────────────
    // Grab 실행
    // ───────────────────────────────────────────

    public void DOGrab()
    {
        if (!player.isLocalPlayer) return;

        if (!grabing)
        {
            StartCoroutine(GoGrab());
            grabing = true;
        }
    }

    // ───────────────────────────────────────────
    // 충돌 감지
    // ───────────────────────────────────────────

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!player.isLocalPlayer) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            var otherGrab = collision.gameObject.GetComponentInChildren<Grab>();
            if (otherGrab != null && !otherGrab.GrabPlayer)
            {
                GrabPlayer = true;
                if (grabing && targetingable)
                {
                    SetTarget(collision.gameObject);
                    targetingable = false;
                    StopAllCoroutines();
                    StartCoroutine(BackGrab());
                    holdGrab = true;
                }
            }
        }
        else if (collision.gameObject.CompareTag("interactive"))
        {
            if (grabing && targetingable)
            {
                SetTarget(collision.gameObject);
                targetingable = false;
                StopAllCoroutines();
                StartCoroutine(BackGrab());
                holdGrab = true;
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            StopAllCoroutines();
            StartCoroutine(BackGrab());
        }
    }

    // ───────────────────────────────────────────
    // 코루틴
    // ───────────────────────────────────────────

    public IEnumerator GoGrab()
    {
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
                break;

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
        ClearTarget();
    }
}