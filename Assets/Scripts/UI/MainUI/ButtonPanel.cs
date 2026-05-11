using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanel : MonoBehaviour
{
    [SerializeField] private bool baseEnabled = false;
    [SerializeField] private float fadeDuration = 0.2f;
    [SerializeField] private Button firstSelectButton;

    private bool isDisabled = false;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if(!baseEnabled) DisablePanel();
    }

    private void Update()
    {
        if (isDisabled) return;
        canvasGroup.blocksRaycasts = UIInputManager.instance.currentDevice == InputDeviceType.Mouse;
    }

    // 패널을 활성화시키고 효과 및 첫 선택 버튼을 설정
    public void EnablePanel()
    {
        firstSelectButton.Select();
        canvasGroup.alpha = 0f;
        FadeOut();
    }

    // 패널을 비활성화 시키고 알파를 0으로 바꿈
    public void DisablePanel()
    {
        isDisabled = true;
        FadeIn();
    }

    public void FadeOut()
    {
        canvasGroup.DOFade(1f, fadeDuration).OnComplete(() =>
        {
            canvasGroup.blocksRaycasts = true;
        });
    }
    public void FadeIn()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0f, fadeDuration);
    }
}
