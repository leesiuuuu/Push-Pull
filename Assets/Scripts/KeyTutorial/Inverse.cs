using TMPro;
using UnityEngine;

public class Inverse : MonoBehaviour
{
	public void Start()
	{
		TMP_Text text = GetComponent<TMP_Text>();
		string result = $"<b>이것은 <color=yellow>반전 장치</color>입니다.\n" +
			$"</b>\n<size=24>상호작용 시, 특정한 바닥을 이용할 수 있습니다.\n</size>";

		string result2 = $"<size=16><i>모든 색깔이 반전된다!</i></size>";

		text.text = result + result2;
	}
}
