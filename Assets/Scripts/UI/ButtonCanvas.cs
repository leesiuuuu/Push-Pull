using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCanvas : MonoBehaviour
{
    public List<UIButton> Buttons = new List<UIButton>();

    [SerializeField] private bool isMainCanva = false;
    [SerializeField] private float fadeDuration = 0.2f;

    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private UIButton firstSelectButton;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        if (isMainCanva) EnableCanvas();
    }

    public void EnableCanvas()
    {
        canvas.enabled = true;
        firstSelectButton = Buttons[0];
        FadeOut();
    }
    
    public void FadeOut()
    {
        canvasGroup.DOFade(1f, fadeDuration);
    }
    public void FadeIn()
    {
        canvasGroup.DOFade(0f, fadeDuration)
            .OnComplete(() => 
            { 
                canvas.enabled = false; 
            });
    }
}
