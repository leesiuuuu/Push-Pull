using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanButton : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject wind;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            wind.SetActive(true);
            anim.Play("Push");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            wind.SetActive(false);
            anim.Play("Pull");
        }
    }
}
