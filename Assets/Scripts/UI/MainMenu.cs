using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public Button btn;
	public Button QuitBtn;

	private bool isBtnSelected = false;
	public void Quit()
	{
		Application.Quit();
	}
	public void Update()
	{
		if (Input.anyKey && !isBtnSelected)
		{
			btn.Select();
			isBtnSelected = true;
		}
	}
	public void NotQuit()
	{
		isBtnSelected = false;
		QuitBtn.Select();
	}
}
