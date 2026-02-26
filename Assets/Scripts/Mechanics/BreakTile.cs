using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakTile : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<NewPlayer1Anim>() || collision.transform.GetComponent<NewPlayer2Anim>())
        {
            StartCoroutine(Shake(transform, 1, 0.1f));
        }
    }

    public IEnumerator Shake(Transform target, float duration, float strength)
    {
        Vector3 origin = target.localPosition;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float damper = 1f - (time / duration);

            Vector3 randomOffset = Random.insideUnitSphere * strength * damper;

            target.localPosition = origin + randomOffset;

            yield return null;
        }

        Destroy(gameObject);
    }
}
