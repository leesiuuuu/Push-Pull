using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearButton : MonoBehaviour
{
    [SerializeField] GameObject disappearObj;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Rigidbody2D>() != null)
        {
            disappearObj.SetActive(false);
            anim.Play("Push");
        }
    }
}
