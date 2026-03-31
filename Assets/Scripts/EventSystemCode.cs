using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemCode : MonoBehaviour
{
	[SerializeField]
	private StandaloneInputModule sim;
	private void Start()
	{
		if (PlayerPrefs.HasKey("ControlMode"))
		{
			if(PlayerPrefs.GetInt("ControlMode") == 1)
			{
				sim.horizontalAxis = "JoyStick1";
			}
			else
			{
				sim.horizontalAxis = "Horizontal1";
			}
		}
	}
}
