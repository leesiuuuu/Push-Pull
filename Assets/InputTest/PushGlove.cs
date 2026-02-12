using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        player = gameObject.GetComponentInParent<InputPlayer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (!_canPush)
            return;

        if (!player.Push)
            return;

        if (collision.gameObject.CompareTag("interactive") || collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigid))
            {
                if (gameObject.transform.position.x < collision.gameObject.transform.position.x)
                {
                    rigid.AddForce(Vector2.right * _PushPower, ForceMode2D.Impulse);
                    Debug.Log("Push");
                }
                else if (gameObject.transform.position.x > collision.gameObject.transform.position.x)
                {
                    rigid.AddForce(Vector2.left * _PushPower, ForceMode2D.Impulse);
                }
                rigid.AddForce(Vector2.up * _PushPower / 2f, ForceMode2D.Impulse);
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
