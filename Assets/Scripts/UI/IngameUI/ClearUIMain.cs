using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClearUIMain : MonoBehaviour
{
	public Button[] btns;
	public Button lastbtn;
	private bool isSelected = false;


	private void Update()
	{
		if (Input.anyKey && !isSelected)
		{
			btns[0].Select();
			isSelected = true;
		}
	}
	public void Cancel()
	{
		isSelected = false;
		lastbtn.Select();
	}
	public void AddEvent(UnityAction ua)
	{
		btns[0].onClick.RemoveAllListeners();
		btns[0].onClick.AddListener(ua);
	}
}
