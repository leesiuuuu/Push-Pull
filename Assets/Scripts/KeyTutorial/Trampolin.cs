using TMPro;
using UnityEngine;

public class Trampolin : MonoBehaviour
{
	public void Start()
	{
		TMP_Text text = GetComponent<TMP_Text>();
		string result = $"<b>이것은 <color=yellow>트램펄린</color>입니다.\n" +
			$"</b>\n<size=24>위로 올라가면, 기존 점프보다\n<b>훨씬 높이 점프할 수 </b>있습니다.\n</size>";


		text.text = result;
	}
}
