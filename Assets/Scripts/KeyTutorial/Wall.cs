using TMPro;
using UnityEngine;

public class Wall : MonoBehaviour
{
	public void Start()
	{
		TMP_Text text = GetComponent<TMP_Text>();
		string result = $"<b>이 벽은 <color=yellow>가짜 벽</color>입니다.\n" +
			$"</b>\n<size=24>플레이어는 지나갈 수 없지만,";

		string result2 = $"\n당길 때는 잡힌 물체가 벽을 통과합니다.";

		text.text = result + result2;
	}
}
