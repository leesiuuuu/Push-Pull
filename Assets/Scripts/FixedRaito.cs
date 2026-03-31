using UnityEngine;

public class FixedRaito : MonoBehaviour
{
	public float targetAspect = 16f / 9f;

	void Start()
	{
		float windowAspect = (float)Screen.width / (float)Screen.height;
		float scaleHeight = windowAspect / targetAspect;

		Camera camera = GetComponent<Camera>();

		if (scaleHeight < 1.0f)
		{
			// 레터박스 (위, 아래 검정)
			Rect rect = camera.rect;

			rect.width = 1.0f;
			rect.height = scaleHeight;
			rect.x = 0;
			rect.y = (1.0f - scaleHeight) / 2.0f;

			camera.rect = rect;
		}
		else
		{
			// 필러박스 (좌우 검정)
			float scaleWidth = 1.0f / scaleHeight;

			Rect rect = camera.rect;

			rect.width = scaleWidth;
			rect.height = 1.0f;
			rect.x = (1.0f - scaleWidth) / 2.0f;
			rect.y = 0;

			camera.rect = rect;
		}
	}
}
