using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
	[SerializeField]
	private GameObject[] panels;

	private void Update()
	{
		if (PlayerPrefs.GetInt("ControlMode") == 1)
		{
			panels[0].SetActive(false);
			panels[1].SetActive(true);
		}
		else
		{
			panels[0].SetActive(true);
			panels[1].SetActive(false);
		}
	}
}
