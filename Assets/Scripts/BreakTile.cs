using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakTile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<NewPlayer1Anim>() || collision.transform.GetComponent<NewPlayer2Anim>())
        {
            Destroy(gameObject);
        }
    }
}
