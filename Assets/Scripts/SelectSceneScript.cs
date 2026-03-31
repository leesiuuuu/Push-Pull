using UnityEngine;
using UnityEngine.UI;

public class SelectSceneScript : MonoBehaviour
{
	[SerializeField]
	private LevelLoader levelLoader;
	[SerializeField]
	private BGMScript bs;
	[SerializeField]
	private Button[] btns;
	private bool isSelected = false;
	private void Update()
	{
/*		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
		{
			bs.FadeOut();
			levelLoader.LoadScene("Main");
		}*/
		if(Input.anyKey && !isSelected)
		{
			btns[0].Select();
			isSelected = true;
		}
	}
}
