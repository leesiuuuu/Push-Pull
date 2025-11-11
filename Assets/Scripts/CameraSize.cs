using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    public Vector3 addPos = new Vector3(0, 3, 0);

    public Transform object1;
    public Transform object2;
    public float margin = 2f;

    private void Awake()
    {
        object1 = GameObject.Find("Player1").GetComponent<Transform>();
        object2 = GameObject.Find("Player2").GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 pos = new Vector3((object1.position.x + object2.position.x) / 2, (object1.position.y + object2.position.y) / 2, -10);

        transform.position = pos + addPos;

        float distance = Vector2.Distance(object1.position, object2.position);
        Camera.main.orthographicSize = Mathf.Max((distance / 2f) + margin, 7f);
    }
}
