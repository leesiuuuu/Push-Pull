using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PushGlove : NetworkBehaviour
{
    private InputPlayer player;

    public float PushPower
    {
        get => _PushPower;
        set => _PushPower = value;
    }
    [SerializeField] private float _PushPower;
    public bool _canPush = true;

    private void Awake()
    {
        player = gameObject.GetComponentInParent<InputPlayer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!player.isLocalPlayer) return;
        if (!_canPush) return;
        if (!player.Push) return;

        if (collision.gameObject.CompareTag("interactive") || collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigid))
            {
                Vector2 dir = (gameObject.transform.position.x < collision.gameObject.transform.position.x)
                    ? Vector2.right
                    : Vector2.left;

                NetworkIdentity targetIdentity = collision.gameObject.GetComponent<NetworkIdentity>();

                if (targetIdentity != null)
                {
                    CmdApplyPush(targetIdentity.netId, dir, _PushPower);
                }
                else
                {
                    ApplyForce(rigid, dir, _PushPower);
                }

                player.PushCharge = 0f;
                StartCoroutine(PushCooldown());
            }
        }
    }

    // ───────────────────────────────────────────
    // 서버에서 힘 적용 후 모든 클라이언트에 전달
    // ───────────────────────────────────────────

    [Command]
    private void CmdApplyPush(uint targetNetId, Vector2 dir, float power)
    {
        RpcApplyPush(targetNetId, dir, power);
    }

    [ClientRpc]
    private void RpcApplyPush(uint targetNetId, Vector2 dir, float power)
    {
        if (!NetworkServer.spawned.TryGetValue(targetNetId, out NetworkIdentity identity)) return;

        if (identity.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigid))
        {
            ApplyForce(rigid, dir, power);
        }
    }

    // ───────────────────────────────────────────
    // 실제 힘 적용
    // ───────────────────────────────────────────

    private void ApplyForce(Rigidbody2D rigid, Vector2 dir, float power)
    {
        rigid.AddForce(dir * power, ForceMode2D.Impulse);
        rigid.AddForce(Vector2.up * power / 2f, ForceMode2D.Impulse);
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