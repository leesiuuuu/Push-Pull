using System.Collections;
using UnityEngine;

public class PushGlove : MonoBehaviour
{
    private InputPlayer player;
    private NetworkPlayer networkPlayer;

    public float PushPower
    {
        get => _PushPower;
        set => _PushPower = value;
    }
    [SerializeField] private float _PushPower;
    public bool _canPush = true;

    private void Awake()
    {
        player = GetComponentInParent<InputPlayer>();
        networkPlayer = GetComponentInParent<NetworkPlayer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!player.IsLocal) return;
        if (!_canPush) return;
        if (!player.Push) return;

        if (collision.gameObject.CompareTag("interactive") || collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigid))
            {
                Vector2 dir = (gameObject.transform.position.x < collision.gameObject.transform.position.x)
                    ? Vector2.right
                    : Vector2.left;

                var targetIdentity = collision.gameObject.GetComponent<Mirror.NetworkIdentity>();

                if (targetIdentity != null)
                {
                    networkPlayer?.SyncApplyPush(targetIdentity.netId, dir, _PushPower);
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
    }

    private IEnumerator PushCooldown()
    {
        _canPush = false;
        yield return new WaitForSeconds(0.8f);
        _canPush = true;
    }
}