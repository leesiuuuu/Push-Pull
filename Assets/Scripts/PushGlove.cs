using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushGlove : MonoBehaviour
{
    public float player1PushPower
    {
        get => _Player1PushPower;
        set => _Player1PushPower = value;
    }
    [SerializeField] private float _Player1PushPower;

    public float player2PushPower
    {
        get => _Player2PushPower;
        set => _Player2PushPower = value;
    }
    [SerializeField] private float _Player2PushPower;

    public bool _canPushPlayer1 = true;
    public bool _canPushPlayer2 = true;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player1 player1 = gameObject.GetComponentInParent<Player1>();
        if (player1)
        {
            if (!_canPushPlayer1)
                return;

            if (!player1.Push)
                return;

            if (collision.gameObject.CompareTag("interactive"))
            {
                if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigid))
                {
                    if (gameObject.transform.position.x < collision.gameObject.transform.position.x)
                    {
                        rigid.AddForce(Vector2.right * _Player1PushPower, ForceMode2D.Impulse);
                        Debug.Log("Push");
                    }
                    else if (gameObject.transform.position.x > collision.gameObject.transform.position.x)
                    {
                        rigid.AddForce(Vector2.left * _Player1PushPower, ForceMode2D.Impulse);
                    }
                    rigid.AddForce(Vector2.up * _Player1PushPower / 2f, ForceMode2D.Impulse);
                    player1.pushCharge = 0f;
                    StartCoroutine(PushCooldownPlayer1());
                }
            }
        }

        Player2 player2 = gameObject.GetComponentInParent<Player2>();
        if (player2)
        {
            if (!_canPushPlayer2)
                return;

            if (!player2.Push)
                return;

            if (collision.gameObject.CompareTag("interactive"))
            {
                if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigid))
                {
                    if (gameObject.transform.position.x < collision.gameObject.transform.position.x)
                    {
                        rigid.AddForce(Vector2.right * _Player2PushPower, ForceMode2D.Impulse);
                        Debug.Log("Push");
                    }
                    else if (gameObject.transform.position.x > collision.gameObject.transform.position.x)
                    {
                        rigid.AddForce(Vector2.left * _Player2PushPower, ForceMode2D.Impulse);
                    }
                    rigid.AddForce(Vector2.up * _Player2PushPower / 2.9166f, ForceMode2D.Impulse);
                    player2.pushCharge = 0f;
                    StartCoroutine(PushCooldownPlayer2());
                }
            }
        }
    }

    private IEnumerator PushCooldownPlayer1()
    {
        _canPushPlayer1 = false;
        yield return new WaitForSeconds(0.8f);
        _canPushPlayer1 = true;
    }

    private IEnumerator PushCooldownPlayer2()
    {
        _canPushPlayer2 = false;
        yield return new WaitForSeconds(0.8f);
        _canPushPlayer2 = true;
    }
}
