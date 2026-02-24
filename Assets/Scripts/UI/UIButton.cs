using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class UIButton : Button
{
    private EventSystem _eventSystem;

    protected override void Awake()
    {
        base.Awake();
        _eventSystem = EventSystem.current;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        Debug.Log($"{name} 버튼에 마우스 들어감 (PointerEnter)");
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        Debug.Log($"{name} 버튼 선택됨 (Highlighted)");
        // 여기에 선택 시 연출 추가
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        Debug.Log($"{name} 버튼 선택 해제됨");
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        Debug.Log($"{name} 버튼 클릭됨 (PointerClick)");
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        Debug.Log($"{name} 버튼 클릭됨 (Submit)");
    }
}