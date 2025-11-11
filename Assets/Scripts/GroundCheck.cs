using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("interactive"))
        {
            if(transform.parent.TryGetComponent<Player1>(out Player1 player1))
            {
                player1.jumpAble = true;
            }
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("interactive"))
        {

            if(transform.parent.TryGetComponent<Player2>(out Player2 player2))
            {
                player2.jumpAble = true;                
            }
        }
    }
}
