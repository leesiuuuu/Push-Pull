using TMPro;
using UnityEngine;

public class KeyDescription : MonoBehaviour, ITutorial
{

	public void Start()
	{
		WriteText();
	}

	public void WriteText()
	{
		TMP_Text text = GetComponent<TMP_Text>();
		string result = $"<b><color=yellow>{KeyManager.Instance.GetPlayer1Key(KeyAction.LEFT)}</color>와 " +
			$"<color=yellow>{KeyManager.Instance.GetPlayer1Key(KeyAction.RIGHT)}</color></b>\n<size=28>로 이동할 수 있습니다.\n\n</size>";

		string result2 = $"<b><color=#00ffffff>{KeyManager.Instance.GetPlayer2Key(KeyAction.LEFT)}</color>와 " +
			$"<color=#00ffffff>{KeyManager.Instance.GetPlayer2Key(KeyAction.RIGHT)}</color></b>\n<size=28>로 이동할 수 있습니다.</size>";

		text.text = result + result2;
	}

	public void WriteText(string desc)
	{
		TMP_Text text = GetComponent<TMP_Text>();
		text.text = desc;
	}
}
