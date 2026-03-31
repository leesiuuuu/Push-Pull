using System.Collections;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class HoverMove : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	public GameObject obj;

	public Tilemap tMap;

	public CanvasGroup CG;
	public bool isSelected = false;

	private void Start()
	{
		Deselect();
	}

	public void Deselect()
	{
		StartCoroutine(Scale(new Vector3(1f, 1f, 1f)));
		StartCoroutine(Color(new UnityEngine.Color(1f, 1f, 1f, 0.5f)));
		StartCoroutine(CanvasAlpha(0.5f));
		isSelected = false;
	}
	public void OnDeselect(BaseEventData eventData)
	{
		Deselect();
	}

	public void OnSelect(BaseEventData eventData)
	{
		StartCoroutine(Scale(new Vector3(1.05f, 1.05f, 1.05f)));
		StartCoroutine(Color(new UnityEngine.Color(1f, 1f, 1f, 1f)));
		StartCoroutine(CanvasAlpha(1f));
		isSelected = true;
	}

	private IEnumerator Scale(Vector3 size)
	{
		float ElapsedTime = 0f;
		Vector3 s = obj.transform.localScale;

		while(ElapsedTime < 0.2f)
		{
			ElapsedTime += Time.deltaTime;
			float t = ElapsedTime / 0.2f;
			obj.transform.localScale = Vector2.Lerp(s, size, t);
			yield return null;
		}
		yield break;
	}

	private IEnumerator Color(Color color)
	{
		float ElapsedTime = 0f;
		Color c = tMap.color;

		while (ElapsedTime < 0.2f)
		{
			ElapsedTime += Time.deltaTime;
			float t = ElapsedTime / 0.2f;
			tMap.color = UnityEngine.Color.Lerp(c, color, t);
			yield return null;
		}
		yield break;
	}

	private IEnumerator CanvasAlpha(float value)
	{
		float ElapsedTime = 0f;
		float n = CG.alpha;

		while (ElapsedTime < 0.2f)
		{
			ElapsedTime += Time.deltaTime;
			float t = ElapsedTime / 0.2f;
			CG.alpha = Mathf.Lerp(n, value, t);
			yield return null;
		}
		yield break;
	}
}
