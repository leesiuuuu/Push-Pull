using System.Collections;
using System.Linq;
using UnityEngine;

public class EventTutorial : MonoBehaviour
{
	private CanvasGroup canvasGroup;

	private KeyCounter keyCounter;

	public Coroutine coroutine = null;
	private void Start()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		keyCounter = FindObjectOfType<KeyCounter>();
	}

	private void Update()
	{
		if (keyCounter.KeyCount > 0 && coroutine == null)
		{
			coroutine = StartCoroutine(AlphaByEvent(0.5f));
		}
		if(keyCounter.KeyCount == keyCounter._maxCount)
		{
			ITutorial[] tutorials = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<ITutorial>().ToArray();
			for(int i = 0; i < tutorials.Length; i++)
			{
				tutorials[i].WriteText("<size=24>이제 스테이지 클리어 문으로 \n이동하세요!</size>");
			}
		}
	}
	
	private IEnumerator AlphaByEvent(float duration)
	{
		float ElapsedTime = 0f;
		while (ElapsedTime < duration)
		{
			ElapsedTime += Time.deltaTime;
			float t = ElapsedTime / duration;
			canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		ElapsedTime = 0f;

		while (ElapsedTime < duration)
		{
			ElapsedTime += Time.deltaTime;
			float t = ElapsedTime / duration;
			canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
			yield return null;
		}
	}
}
