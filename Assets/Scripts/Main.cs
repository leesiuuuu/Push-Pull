using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
