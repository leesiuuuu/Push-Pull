using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCanvas : MonoBehaviour
{
    public bool MainCanvas
    {
        get
        {
            return isMainCanva;
        }
    }
    [SerializeField] private bool isMainCanva = false;
    [SerializeField] private float fadeDuration = 0.2f;

    private CanvasGroup canvasGroup;
    private Canvas canvas;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        if (isMainCanva) EnableCanvas();
        else DisableCanvas();
    }

    public void EnableCanvas()
    {
        canvas.enabled = true;
        canvasGroup.alpha = 0f;
        FadeOut();
    }

    public void DisableCanvas()
    {
        canvas.enabled = false;
        canvasGroup.alpha = 0f;
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
