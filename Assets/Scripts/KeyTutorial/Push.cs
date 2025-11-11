using TMPro;
using UnityEngine;

public class Push : MonoBehaviour, ITutorial
{
	public void Start()
	{
		WriteText();
	}

	public void WriteText()
	{
		TMP_Text text = GetComponent<TMP_Text>();
		string result = $"<b><size=24><color=yellow>{KeyManager.Instance.GetPlayer1Key(KeyAction.PULL)}</color></size><size=18> 또는 </size><size=24><color=#00ffffff>{KeyManager.Instance.GetPlayer2Key(KeyAction.PULL)}" +
			$"</color></size></b>\n<size=20>로 상대를 당길 수 있습니다.\n\n";

		string result2 = $"키를 꾹 누르면\n<i>각도를 조절할 수 있습니다.</i></size>";

		text.text = result + result2;
	}

	public void WriteText(string desc)
	{
		TMP_Text text = GetComponent<TMP_Text>();
		text.text = desc;
	}
}
