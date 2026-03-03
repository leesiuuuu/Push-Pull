using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;
using System.Collections;

public class UIButton : Button
{
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip hoverSound;

    [SerializeField] private ButtonCanvas disableCanvas;        // 비활성화 할(보이지 않게 할 캔버스) 오브젝트
    [SerializeField] private  ButtonCanvas enableCanvas;        // 활성화 할(보이게 할 캔버스) 오브젝트

    [SerializeField] private ButtonType buttonType;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        // 호버 사운드 재생
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        // 호버 사운드 재생
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        // 클릭 사운드 재생
        OnClicked();
        Debug.Log($"{name} 버튼 클릭됨 (PointerClick)");
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        // 클릭 사운드 재생
        OnClicked();
        Debug.Log($"{name} 버튼 클릭됨 (Submit)");
    }

    public void OnClicked()
    {
        switch (buttonType)
        {
            case ButtonType.ChangeCanvas:
                changeCanvas(); break;
        }
    }

    private void changeCanvas() => StartCoroutine(changeCanvasCoroutine());

    private IEnumerator changeCanvasCoroutine()
    {
        disableCanvas.FadeIn();
        yield return new WaitForSeconds(0.2f);
        enableCanvas.EnableCanvas();
    }
}

public enum ButtonType
{
    None,           // 기본 (소리만 나거나 단순 클릭 로그용)
    ChangeCanvas,   // 현재 창을 끄고 다른 창을 엶 (UI 이동)
    OpenPopup,      // 현재 창은 두고 위에 팝업을 띄움
    ClosePopup,     // 현재 팝업을 닫음
    Submit,         // 데이터 확인, 아이템 구매 등 서버/데이터 연동
    GameStart,      // 씬 전환 (Scene Load)
    Toggle,         // On/Off 상태 전환 (체크박스 스타일)
    Quit            // 게임 종료
}