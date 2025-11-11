using TMPro;
using UnityEngine;

public class Pull : MonoBehaviour, ITutorial
{
	public void Start()
	{
		WriteText();
	}

	public void WriteText()
	{
		TMP_Text text = GetComponent<TMP_Text>();
		string result = $"<b><size=24><color=yellow>{KeyManager.Instance.GetPlayer1Key(KeyAction.PUSH)}</color></size><size=18> 또는 </size><size=24><color=#00ffffff>{KeyManager.Instance.GetPlayer2Key(KeyAction.PUSH)}" +
			$"</color></size></b>\n<size=20>로 상대를 밀 수 있습니다.\n\n";

		string result2 = $"키를 꾹 누르면 <i>차징할 수 있습니다.</i>\n차징 시 밀치는 강도가 더욱 강해집니다.</size>";

		text.text = result + result2;
	}

	public void WriteText(string desc)
	{
		TMP_Text text = GetComponent<TMP_Text>();
		text.text = desc;
	}
}
