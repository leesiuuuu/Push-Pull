using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

interface ITutorial
{
	/// <summary>
	/// 기본 문구를 출력함
	/// </summary>
	public void WriteText();
	public void WriteText(string desc);
}

public class TutorialAppear : MonoBehaviour
{
	public Transform Player1;
	public Transform Player2;

	[Header("Canvas Group")]
	public CanvasGroup Keyboard;
	public CanvasGroup GamePad;

	public float maxDistance;

	public bool isOnlyOne;

	private void Update()
	{
		if (!isOnlyOne)
		{
			if (PlayerPrefs.GetInt("ControlMode") == 1)
			{
				Keyboard.alpha = 0;
				AlphaByDistance(GamePad);
			}
			else
			{
				GamePad.alpha = 0;
				AlphaByDistance(Keyboard);
			}
		}
		else
		{
			//아무거나 해도 상관없음
			AlphaByDistance(Keyboard);
		}
	}
	private void AlphaByDistance(CanvasGroup cg)
	{
		if(cg.transform.position.y+2 < Player1.transform.position.y ||
			cg.transform.position.y+2 < Player2.transform.position.y)
		{
			return;
		}
		else
		{
			float distance1 = Mathf.Abs(cg.transform.position.x - Player1.position.x);
			float distance2 = Mathf.Abs(cg.transform.position.x - Player2.position.x);

			float distance = distance1 > distance2 ? distance2 : distance1;

			float alpha = Mathf.Clamp01(1f - (distance / maxDistance));

			cg.alpha = alpha;
		}
	}
}
