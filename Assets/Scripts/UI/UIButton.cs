using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIButton : Button
{
    public AudioClip clickSound;
    public AudioClip hoverSound;
    public UnityEvent OnClicked;

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
        OnClicked?.Invoke();
        Debug.Log($"{name} 버튼 클릭됨 (PointerClick)");
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        // 클릭 사운드 재생
        OnClicked?.Invoke();
        Debug.Log($"{name} 버튼 클릭됨 (Submit)");
    }
}