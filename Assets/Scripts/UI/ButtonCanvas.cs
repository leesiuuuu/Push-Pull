using System.Collections.Generic;
using UnityEngine;

public class ButtonCanvas : MonoBehaviour
{
    public List<UIButton> Buttons = new List<UIButton>();

    private CanvasGroup canvasGroup;
    private UIButton firstSelectButton;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnEnable()
    {
        firstSelectButton = Buttons[0];
    }
}
