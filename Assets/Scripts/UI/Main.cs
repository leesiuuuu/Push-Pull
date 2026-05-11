using UnityEngine;
using UnityEngine.UI;


// 필요없는 스크립트
// 추후 삭제할 것
public class Main : MonoBehaviour
{
	private bool isSelected = false;
	public Button[] btns;
	private void Update()
	{
		if (Input.anyKey && !isSelected)
		{
			btns[0].Select();
			isSelected = true;
		}
	}
}
