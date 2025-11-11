using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    LineRenderer line;
    public Transform TargetGO;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        //TargetGO = transform.Find("LinePos");
    }

    private void Start()
    {
        line.positionCount = 2;
    }

    private void Update()
    {
        line.SetPosition(0, TargetGO.transform.position);
        line.SetPosition(1, gameObject.transform.position);
    }
}
