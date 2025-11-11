using TMPro;
using UnityEngine;

public class JumpKey : MonoBehaviour, ITutorial
{
	public void Start()
	{
		WriteText();
	}

	public void WriteText()
	{
		TMP_Text text = GetComponent<TMP_Text>();
		string result = $"<b><color=yellow>{KeyManager.Instance.GetPlayer1Key(KeyAction.Jump)}</color></b>\n<size=28>로 점프할 수 있습니다.\n\n</size>";

		string result2 = $"<b><color=#00ffffff>{KeyManager.Instance.GetPlayer2Key(KeyAction.Jump)}</color></b>\n<size=28>로 점프할 수 있습니다.</size>";

		text.text = result + result2;
	}

	public void WriteText(string desc)
	{
		TMP_Text text = GetComponent<TMP_Text>();
		text.text = desc;
	}
}
