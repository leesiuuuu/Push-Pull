using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldTileButton : MonoBehaviour
{
    Animator anim;
    [SerializeField] Transform holdTile;

    [SerializeField] float speed;

    [SerializeField] Vector3 pushPos;
    Vector3 desPos = Vector3.zero;
    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            yield return null;
            if ((desPos - holdTile.position).magnitude > 0.03)
                holdTile.position += (desPos - holdTile.position).normalized * speed * Time.deltaTime;
            else
                yield break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            StartCoroutine(Move());
            desPos = pushPos;
            anim.Play("Push");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            StartCoroutine(Move());
            desPos = Vector3.zero;
            anim.Play("Pull");
        }
    }
}
