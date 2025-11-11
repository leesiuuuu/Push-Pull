using System.Collections;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public GameObject trampoline;
    public float JumpPower = 2;
    [SerializeField]
    AudioClip clip;

    public Animator spriteAnimator;
/*    private void Awake()
    {
        trampoline = transform.parent.parent.gameObject;
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            SoundManager.Instance.SFXPlay("Trampoline",clip);
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            spriteAnimator.SetTrigger("Jump");
            //StartCoroutine(ScaleSet());
        }
    }

    IEnumerator ScaleSet()
    {
        float t = 0;
        Vector3 scale = trampoline.transform.localScale;
        float duration = 0.15f;

        while (t <= duration)
        {
            t += Time.deltaTime;
            scale.y = Mathf.Lerp(1, 0.8f, t / duration);
            trampoline.transform.localScale = scale;
            yield return null;
        }
        scale.y = 0.8f;

        yield return StartCoroutine(ScaleReset());
    }

    IEnumerator ScaleReset()
    {
        float t = 0;
        Vector3 scale = trampoline.transform.localScale;
        float duration = 0.3f;

        while (t <= duration)
        {
            t += Time.deltaTime;
            scale.y = Mathf.Lerp(0.8f, 1, t / duration);
            trampoline.transform.localScale = scale;
            yield return null;
        }
        scale.y = 1;
    }


}
