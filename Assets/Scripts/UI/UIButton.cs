using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class UIButton : Button
{
    [SerializeField] private UISoundInfo soundInfo;

    [SerializeField] private ButtonCanvas disableCanvas;        // 비활성화 할(보이지 않게 할 캔버스) 오브젝트
    [SerializeField] private  ButtonCanvas enableCanvas;        // 활성화 할(보이게 할 캔버스) 오브젝트

    [SerializeField] private ButtonPanel disablePanel;          // 비활성화 할(보이지 않게 할 캔버스) 오브젝트
    [SerializeField] private ButtonPanel enablePanel;           // 활성화 할(보이게 할 캔버스) 오브젝트

    [SerializeField] private ButtonType buttonType;

    protected override void Start()
    {
        if(buttonType != ButtonType.ChangeCanvas)
        {
            disableCanvas = GetComponentInParent<ButtonCanvas>();
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (UIInputManager.instance != null &&
            UIInputManager.instance.currentDevice != InputDeviceType.Mouse)
        {
            eventData.selectedObject = null;
            return;
        }

        base.OnPointerEnter(eventData);
        eventData.selectedObject = gameObject;
        // 호버 사운드 재생
        SoundManager.Instance?.SFXPlay("UI_Hover", soundInfo.HoverSound);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        // 호버 사운드 재생
        SoundManager.Instance?.SFXPlay("UI_Hover", soundInfo.HoverSound);
    }
    #region Click/Submit
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        // 클릭 사운드 재생
        SoundManager.Instance?.SFXPlay("UI_Click", soundInfo.ClickSound);
        OnClicked();
    }
    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        // 클릭 사운드 재생
        SoundManager.Instance?.SFXPlay("UI_Click", soundInfo.ClickSound);
        OnClicked();
    }
    #endregion

    public void OnClicked()
    {
        switch (buttonType)
        {
            case ButtonType.ChangeCanvas:
                changeCanvas(); break;
            case ButtonType.ChangePanel:
                changePanel(); break;
            case ButtonType.GoMain:
                enableCanvas = FindObjectsOfType<ButtonCanvas>().Where(canvas => canvas.MainCanvas == true).First();
                changeCanvas(); break;
        }
    }

    private void changeCanvas() => StartCoroutine(changeCanvasCoroutine());
    private void changePanel() => StartCoroutine(changePanelCoroutine());

    private IEnumerator changeCanvasCoroutine()
    {
        disableCanvas.FadeIn();
        yield return new WaitForSeconds(0.2f);
        enableCanvas.EnableCanvas();
    }
    private IEnumerator changePanelCoroutine()
    {
        disablePanel.DisablePanel();
        yield return new WaitForSeconds(0.2f);
        enablePanel.EnablePanel();
    }
}

public enum ButtonType
{
    None,           // 기본 (소리만 나거나 단순 클릭 로그용)
    ChangeCanvas,   // 현재 창을 끄고 다른 창을 엶 (UI 이동)
    ChangePanel,    // 현재 패널을 끄고 다른 패널을 엶 (UI 이동)
    OpenPopup,      // 현재 창은 두고 위에 팝업을 띄움
    ClosePopup,     // 현재 팝업을 닫음
    Submit,         // 데이터 확인, 아이템 구매 등 서버/데이터 연동
    GameStart,      // 씬 전환 (Scene Load)
    GoMain,
    Quit            // 게임 종료
}
