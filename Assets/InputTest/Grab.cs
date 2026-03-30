using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Grab : NetworkBehaviour
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

    [SyncVar(hook = nameof(OnGlovePosChanged))]
    private Vector3 syncLocalPos;

    private void OnGlovePosChanged(Vector3 oldVal, Vector3 newVal)
    {
        if (isLocalPlayer) return;
        transform.localPosition = newVal;
    }

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
            Target = null;
        }

        // 대상을 잡은 상태: 글러브 위치로 끌어당김
        if (holdGrab && grabing && Target != null)
        {
            if (GrabPlayer)
            {
                // 상대 플레이어를 잡은 경우 서버에 위치 동기화 요청
                CmdMoveTarget(Target.GetComponent<NetworkIdentity>().netId, gameObject.transform.position);
            }
            else
            {
                // 일반 오브젝트는 직접 이동
                Target.transform.position = gameObject.transform.position;
            }
        }

        if (isLocalPlayer && grabing)
        {
            CmdUpdateGlovePos(transform.localPosition);
        }
    }

    // ───────────────────────────────────────────
    // 글러브 위치 동기화
    // ───────────────────────────────────────────

    [Command]
    private void CmdUpdateGlovePos(Vector3 localPos)
    {
        syncLocalPos = localPos;
    }

    // ───────────────────────────────────────────
    // 상대 플레이어 위치 강제 이동 (서버 권한)
    // ───────────────────────────────────────────

    [Command]
    private void CmdMoveTarget(uint targetNetId, Vector3 targetPos)
    {
        if (NetworkServer.spawned.TryGetValue(targetNetId, out NetworkIdentity identity))
        {
            RpcMoveTarget(targetNetId, targetPos);
        }
    }

    [ClientRpc]
    private void RpcMoveTarget(uint targetNetId, Vector3 targetPos)
    {
        if (NetworkServer.spawned.TryGetValue(targetNetId, out NetworkIdentity identity))
        {
            identity.transform.position = targetPos;
        }
    }

    // ───────────────────────────────────────────
    // Grab 실행 (InputPlayer에서 호출)
    // ───────────────────────────────────────────

    public void DOGrab()
    {
        if (!isLocalPlayer) return; // 로컬만 실행

        if (!grabing)
        {
            StartCoroutine(GoGrab());
            grabing = true;
        }
    }

    // ───────────────────────────────────────────
    // 충돌 감지 → 서버에 알림
    // ───────────────────────────────────────────

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLocalPlayer) return; // 로컬 플레이어만 충돌 감지

        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponentInChildren<Grab>().GrabPlayer)
            {
                GrabPlayer = true;
                if (grabing && targetingable)
                {
                    Target = collision.gameObject.GetComponent<Transform>();
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
                Target = collision.gameObject.GetComponent<Transform>();
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

        // 복귀 완료 시점에 최종 위치를 한 번 더 동기화
        if (isLocalPlayer)
        {
            CmdUpdateGlovePos(StartPos);
        }
    }
}