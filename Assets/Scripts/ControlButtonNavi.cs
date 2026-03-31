using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlButtonNavi : MonoBehaviour, ISelectHandler
{
	[SerializeField]
	private Button curBtn;
	[SerializeField]
	private Button nextBtn;
	[SerializeField]
	private Button endBtn;
	public void OnSelect(BaseEventData eventData)
	{
		if(PlayerPrefs.GetInt("ControlMode") == 1)
		{
			Navigation n = curBtn.navigation;
			n.selectOnDown = endBtn;
			curBtn.navigation = n;
		}
		else
		{
			Navigation n = curBtn.navigation;
			n.selectOnDown = nextBtn;
			curBtn.navigation = n;
		}
	}
}
