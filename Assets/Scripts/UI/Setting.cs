using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
	FullScreenMode fullScreenMode;

	//[SerializeField]
	private List<Resolution> resolutions = new List<Resolution>();

	[SerializeField]
	private TMP_Text text;

	[SerializeField]
	private Toggle isFullScreen;

	[SerializeField]
	private int resolutionNum;

	private void OnEnable()
	{
		resolutions.Clear();
		for(int i =0; i < Screen.resolutions.Length; i++)
		{
			if (Screen.resolutions[i].refreshRateRatio.value >= 30 && (Screen.resolutions[i].width * 9 == Screen.resolutions[i].height * 16) && Screen.resolutions[i].width >= 1280)
			{
				resolutions.Add(Screen.resolutions[i]);
			}
		}
		resolutionNum = resolutions.Count-1;
		isFullScreen.isOn = Screen.fullScreen;
		for (int i = 0; i < resolutions.Count; i++)
		{
			if (resolutions[i].width == Screen.width &&
				resolutions[i].height == Screen.height &&
				Mathf.Approximately((float)resolutions[i].refreshRateRatio.value, (float)Screen.currentResolution.refreshRateRatio.value))
			{
				resolutionNum = i;
				break;
			}
		}
		UpdateScreenText();
	}
	private void UpdateScreenText()
	{
		text.text = $"{resolutions[resolutionNum].width} x {resolutions[resolutionNum].height} {(int)resolutions[resolutionNum].refreshRateRatio.value}hz";
	}

	public void MoveScreenListLeft()
	{
		resolutionNum--;
		resolutionNum = Mathf.Clamp(resolutionNum, 0, resolutions.Count-1);
		UpdateScreenText();
	}

	public void MoveScreenListRight()
	{
		resolutionNum++;
		resolutionNum = Mathf.Clamp(resolutionNum, 0, resolutions.Count - 1);
		UpdateScreenText();
	}

	public void FullScreenBtn(bool isFull)
	{
		fullScreenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
	}

	public void SetScreenSize()
	{
		Screen.SetResolution(resolutions[resolutionNum].width,
			resolutions[resolutionNum].height,
			fullScreenMode);
	}
}
