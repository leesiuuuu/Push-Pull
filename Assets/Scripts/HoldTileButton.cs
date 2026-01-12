using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HoldTileButton : MonoBehaviour
{
    Animator anim;
    [SerializeField] Transform holdTile;

    [SerializeField] float speed;

    // 버튼을 눌렀을 시 목표 지점
    [SerializeField] Vector3 pushPos;
    Vector3 desPos = Vector3.zero;
    private void Start()
    {
        holdTile.position = Vector3.zero;
        anim = GetComponent<Animator>();
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            yield return null;
            if ((desPos - holdTile.position).magnitude > 0.05f)
                holdTile.position += (desPos - holdTile.position).normalized * speed * Time.deltaTime;
            else
                yield break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            desPos = pushPos;
            StartCoroutine(Move());
            anim.Play("Push");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            desPos = Vector3.zero;
            StartCoroutine(Move());
            anim.Play("Pull");
        }
    }
}
