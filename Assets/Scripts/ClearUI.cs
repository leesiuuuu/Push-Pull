using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearUI : MonoBehaviour
{
	[SerializeField]
	private Button firstButton;
	private bool isSelect = false;
	private void Update()
	{
		if (Input.anyKey && !isSelect)
		{
			firstButton.Select();
			isSelect = true;
		}
	}
}
