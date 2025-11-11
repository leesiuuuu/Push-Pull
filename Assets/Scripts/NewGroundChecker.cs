using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGroundCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("interactive"))
        {
            if (transform.parent.TryGetComponent<NewPlayer1>(out NewPlayer1 player1))
            {
                player1.jumpAble = true;
            }
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("interactive"))
        {

            if (transform.parent.TryGetComponent<NewPlayer2>(out NewPlayer2 player2))
            {
                player2.jumpAble = true;
            }
        }
    }
}
