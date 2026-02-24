using UnityEngine;

public class HoldTileButton : MonoBehaviour
{
    Animator anim;

    [SerializeField] HoldTileController controller;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb == null) return;

        controller.AddHolder(rb);
        anim.Play("Push");
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb == null) return;

        controller.RemoveHolder(rb);
        anim.Play("Pull");
    }
}
