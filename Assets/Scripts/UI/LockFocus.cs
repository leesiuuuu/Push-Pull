using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LockFocus : MonoBehaviour
{
	private GameObject defaultSelected;

	private void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	void Update()
	{
		// 선택 상태 유지
		if (FindObjectOfType<EventSystem>() != null)
		{
			if (!EventSystem.current.currentSelectedGameObject)
			{
				EventSystem.current.SetSelectedGameObject(defaultSelected);
			}
			defaultSelected = EventSystem.current.currentSelectedGameObject;
		}
	}

}