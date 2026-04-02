using System.Collections;
using UnityEngine;

public class PushGlove : MonoBehaviour
{
    private InputPlayer player;

    public float PushPower
    {
        get => _PushPower;
        set => _PushPower = value;
    }
    [SerializeField] private float _PushPower;
    public bool _canPush = true;

    [SerializeField] private Vector3 startLocalPos;
    private bool isAnimating = false;

    private void Awake()
    {
        player = GetComponentInParent<InputPlayer>();
    }

    // ───────────────────────────────────────────
    // 충돌 감지
    // ───────────────────────────────────────────

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!player.isLocalPlayer) return;
        if (!_canPush) return;
        if (!player.Push) return;

        if (collision.gameObject.CompareTag("interactive") || collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rigid = collision.attachedRigidbody;
            if (rigid == null) return;

            Vector2 dir = (player.gameObject.transform.position.x < collision.gameObject.transform.position.x)
                ? Vector2.right
                : Vector2.left;

            var targetIdentity = collision.gameObject.GetComponent<Mirror.NetworkIdentity>();

            if (targetIdentity != null)
            {
                player.SyncApplyPush(targetIdentity.netId, dir, _PushPower);
            }
            else
            {
                rigid.AddForce(dir * _PushPower, ForceMode2D.Impulse);
                rigid.AddForce(Vector2.up * _PushPower / 2f, ForceMode2D.Impulse);
            }

            player.PushCharge = 0f;
            StartCoroutine(PushCooldown());
        }
    }

    // ───────────────────────────────────────────
    // 밀치기 장갑 이동 애니메이션
    // ───────────────────────────────────────────

    public void DoPunchAnim()
    {
        if (!isAnimating)
            StartCoroutine(PunchAnim());
    }

    private IEnumerator PunchAnim()
    {
        isAnimating = true;

        Vector3 targetLocalPos = startLocalPos + new Vector3(1.03f, 0f, 0f);
        float duration = 0.17f;
        float elapsed = 0f;

        // 앞으로 나가기
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startLocalPos, targetLocalPos, elapsed / duration);
            yield return null;
        }
        transform.localPosition = targetLocalPos;

        elapsed = 0f;

        // 원래 위치로 복귀
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(targetLocalPos, startLocalPos, elapsed / duration);
            yield return null;
        }
        transform.localPosition = startLocalPos;

        player.Push = false;
        isAnimating = false;
    }

    // ───────────────────────────────────────────
    // 쿨다운
    // ───────────────────────────────────────────

    private IEnumerator PushCooldown()
    {
        _canPush = false;
        yield return new WaitForSeconds(0.8f);
        _canPush = true;
    }
}