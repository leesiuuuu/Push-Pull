using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldTileController : MonoBehaviour
{
    [SerializeField] Transform holdTile;
    [SerializeField] float speed;
    [SerializeField] Vector3 pushPos;

    Vector3 desPos;

    HashSet<Rigidbody2D> holders = new HashSet<Rigidbody2D>();

    void Start()
    {
        holdTile.localPosition = Vector3.zero;
        desPos = Vector3.zero;

        StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        while (true)
        {
            Vector3 dir = desPos - holdTile.localPosition;

            if (dir.magnitude > 0.01f * (speed + 1))
            {
                holdTile.localPosition += dir.normalized * speed * Time.deltaTime;
            }
            else
            {
                holdTile.localPosition = desPos;
            }

            yield return null;
        }
    }

    public void AddHolder(Rigidbody2D rb)
    {
        bool wasEmpty = holders.Count == 0;

        holders.Add(rb);

        if (wasEmpty && holders.Count > 0)
        {
            desPos = pushPos;
        }
    }

    public void RemoveHolder(Rigidbody2D rb)
    {
        holders.Remove(rb);

        if (holders.Count == 0)
        {
            desPos = Vector3.zero;
        }
    }
}
