using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTitle : MonoBehaviour
{
	public TMP_Text text;
	public RectTransform rt;
	public Canvas canvas;

	private void OnEnable()
	{
		canvas.worldCamera = Camera.main;
		string name = SceneManager.GetActiveScene().name;
		switch (name)
		{
			case "Tutorial":
				text.text = "튜토리얼\r\n<size=36><b>기본중의 기본"; break;
			case "Stage1":
				text.text = "스테이지 1\r\n<size=36><b>가벼운 시작"; break;
			case "Stage2":
				text.text = "스테이지 2\r\n<size=36><b>한 걸음 더"; break;
			case "Stage3":
				text.text = "스테이지 3\r\n<size=36><b>본 게임 시작"; break;
			case "Stage4":
				text.text = "스테이지 4\r\n<size=36><b>곧 도착"; break;
			case "Stage5":
				text.text = "스테이지 5\r\n<size=36><b>마지막"; break;
		}
		StartCoroutine(TitleMove());
		StartCoroutine(TitleScale());
	}
	private IEnumerator TitleMove()
	{
		Vector2 pos = rt.anchoredPosition;

		float ElapsedTime = 0f;
		float Duration = 1f;
		float WaitTime = 2f;
		while(ElapsedTime < Duration)
		{
			ElapsedTime += Time.deltaTime;
			float t = ElapsedTime / Duration;
			t = easeOutQuad(t);

			float y = Mathf.Lerp(800, pos.y, t);
			rt.anchoredPosition = new Vector2(pos.x, y);
			yield return null;
		}
		yield return new WaitForSeconds(WaitTime);
		ElapsedTime = 0f;

		pos = rt.anchoredPosition;
		while (ElapsedTime < Duration)
		{
			ElapsedTime += Time.deltaTime;
			float t = ElapsedTime / Duration;
			t = easeInQuad(t);

			float y = Mathf.Lerp(pos.y, 800, t);
			rt.anchoredPosition = new Vector2(pos.x, y);
			yield return null;
		}
		yield break;
	}
	private IEnumerator TitleScale()
	{
		yield return new WaitForSeconds(0.3f);
		Vector2 scale = rt.sizeDelta;

		float ElapsedTime = 0f;
		float Duration = 0.8f;
		float WaitTime = 1.8f;

		while (ElapsedTime < Duration)
		{
			ElapsedTime += Time.deltaTime;
			float t = ElapsedTime / Duration;
			t = easeOutQuad(t);

			float x = Mathf.Lerp(scale.x, 720, t);
			rt.sizeDelta = new Vector2(x, scale.y);
			yield return null;
		}
		yield return new WaitForSeconds(WaitTime);
		ElapsedTime = 0f;


		scale = rt.sizeDelta;
		while (ElapsedTime < Duration)
		{
			ElapsedTime += Time.deltaTime;
			float t = ElapsedTime / Duration;
			t = easeInQuad(t);

			float x = Mathf.Lerp(scale.x, 100, t);
			rt.sizeDelta = new Vector2(x, scale.y);
			yield return null;
		}
	}
	private float easeOutQuad(float x)
	{
		return 1 - (1 - x) * (1 - x);
	}
	private float easeInQuad(float x)
	{
		return x * x;
	}
}
