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
            yield return null;
            if ((desPos - transform.position).magnitude < 0.05f)
            {
                desPos = desPos == pos1 ? pos2 : pos1;
                yield return new WaitForSeconds(0.3f);
            }
            else
            {
                transform.position += (desPos - transform.position).normalized * speed * Time.deltaTime;
            }
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
