using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	public GameObject SettingPrefab;
	private GameObject settingCanvas;
	private bool isSettingMenu;

	[SerializeField]
	private ClearUIMain[] clearUIs;

	[SerializeField]
	private Button SettingButton;

	[SerializeField]
	private Button FirstBtn;

	private LevelLoader levelLoader;

	private bool isSelect = false;

	private void OnEnable()
	{
		GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
		GetComponent<Canvas>().sortingLayerName = "UI";
		GetComponent<Canvas>().sortingOrder = 2;

		levelLoader = FindObjectOfType<LevelLoader>();
	}

	public void ActiveSetting()
	{
		settingCanvas = Instantiate(SettingPrefab);
		settingCanvas.GetComponent<SettingMenu>().AddEvent(() =>
		{
			settingCanvas.SetActive(false);
		});
		isSettingMenu = true;
	}
	private void Update()
	{
		if (!settingCanvas && isSettingMenu)
		{
			SettingButton.Select();
			isSettingMenu = false;
		}


		if (Input.anyKey && !isSelect)
		{
			isSelect = true;
			FirstBtn.Select();
		}
	}
	public void Resume()
	{
		if(Time.timeScale == 0) Time.timeScale = 1;

		BGMScript bs = FindObjectOfType<PauseController>().bs;
		bs.FadeInCustom(0.2f);
		Destroy(settingCanvas);
		isSettingMenu = false;
		Destroy(gameObject);
	}
	public void StageReset()
	{
		clearUIs[0].AddEvent(() =>
		{
			Time.timeScale = 1;
			Destroy(gameObject);
			levelLoader.LoadScene(SceneManager.GetActiveScene().name);
		});
	}
	public void GoMain()
	{
		clearUIs[1].AddEvent(() =>
		{
			Time.timeScale = 1;
			Destroy(gameObject);
			levelLoader.LoadScene("SelectScene");
		});
	}
}
