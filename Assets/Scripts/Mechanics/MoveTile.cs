using System.Collections;
using UnityEngine;

public class MoveTile : MonoBehaviour
{
    [SerializeField] Vector3 pos1;
    [SerializeField] Vector3 pos2;

    [SerializeField] float speed;
    [SerializeField] float waitTime;

    Vector3 desPos;

    private void Start()
    {
        desPos = pos1;
        StartCoroutine(Move()) ;
    }

    IEnumerator Move()
    {
        while (true)
        {
            transform.localPosition =
                Vector3.MoveTowards(
                    transform.localPosition,
                    desPos,
                    speed * Time.deltaTime
                );

            if (Vector3.Distance(transform.localPosition, desPos) < 0.01f * (speed + 1))
            {
                yield return new WaitForSeconds(waitTime);
                desPos = desPos == pos1 ? pos2 : pos1;
            }

            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.GetComponent<Rigidbody2D>() != null)
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Rigidbody2D>() != null)
        {
            collision.transform.parent = null;
        }
    }
}
