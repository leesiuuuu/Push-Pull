using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldTileLine : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField] Transform desPos;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(Draw());
    }


    IEnumerator Draw()
    {
        while (true)
        {
            yield return null;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, desPos.position);
        }
    }
}
