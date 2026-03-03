using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPos, length;
    public float parallaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<Collider2D>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
		float temp = transform.position.x - startPos;
		if (temp > startPos + length)
        {
            transform.position = new Vector3(transform.position.x - length, 0, 0);

		}
        else if(temp < startPos - length)
        {
            transform.position = new Vector3(transform.position.x + length, 0, 0);
        }
		transform.position += Vector3.left * parallaxEffect * Time.deltaTime;
	}
}
