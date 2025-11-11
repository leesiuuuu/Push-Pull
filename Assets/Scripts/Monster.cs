using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] Vector3 pos1;
    [SerializeField] Vector3 pos2;

    Rigidbody2D rigid;

    [SerializeField] float speed;
    Vector3 desPos;
    private void Start()
    {
        desPos = pos1;
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            yield return null;
            if ((desPos - transform.position).magnitude < 0.03f)
            {
                desPos = desPos == pos1 ? pos2 : pos1;
            }
            else
            {
                rigid.velocity = (desPos - transform.position).normalized * speed;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            desPos = desPos == pos1 ? pos2 : pos1;
        }
    }
}
