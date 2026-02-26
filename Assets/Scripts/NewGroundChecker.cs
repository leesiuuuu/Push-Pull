using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGroundCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("interactive"))
        {
            if (transform.parent.TryGetComponent<InputPlayer>(out InputPlayer player))
            {
                player.jumpAble = true;
            }
        }
    }
}
