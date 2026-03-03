using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum InputDeviceType { Mouse, Keyboard, Gamepad }

public class UIInputManager : MonoBehaviour
{
    public static UIInputManager instance;

    // 현재 입력 상태를 외부(UIButton)에서 확인할 수 있도록 속성 정의
    public InputDeviceType currentDevice { get; private set; } = InputDeviceType.Mouse;

    private GameObject lastSelected;

    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        CheckInputDevice();
        KeepSelection();
    }

    private void KeepSelection()
    {
        if (EventSystem.current == null) return;

        // 1. 현재 선택된 오브젝트가 있다면 기록해둠
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
        else
        {
            // 2. 만약 선택된 게 없는데(null), 마우스 클릭 중이라면? 
            // 빈 공간을 눌렀다는 뜻이므로 직전 오브젝트로 복구
            if (lastSelected != null && lastSelected.activeInHierarchy)
            {
                // 마우스 클릭 혹은 입력이 발생했을 때만 복구 (필요에 따라 조건 조절)
                EventSystem.current.SetSelectedGameObject(lastSelected);
            }
        }
    }

    private void CheckInputDevice()
    {
        // 1. 키보드 입력 체크 (방향키, 엔터, 스페이스 등 UI 조작 키 위주)
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            UpdateDevice(InputDeviceType.Keyboard);
        }

        // 2. 마우스 입력 체크 (움직임이 일정 수준 이상일 때만)
        if (Mouse.current != null)
        {
            // 아주 미세한 떨림에 반응하지 않도록 sqrMagnitude 사용 (최적화)
            bool isMouseMoving = Mouse.current.delta.ReadValue().sqrMagnitude > 0.1f;
            bool isMouseClicked = Mouse.current.leftButton.wasPressedThisFrame ||
                                 Mouse.current.rightButton.wasPressedThisFrame;

            if (isMouseMoving || isMouseClicked)
            {
                UpdateDevice(InputDeviceType.Mouse);
            }
        }
    }

    private void UpdateDevice(InputDeviceType newType)
    {        
        if (currentDevice == newType) return; // 이미 해당 모드라면 무시

        // 키보드 모드일 때 커서를 숨기고 싶다면:
        Cursor.visible = (newType == InputDeviceType.Mouse);

        currentDevice = newType;

        // 디버깅용 로그
        Debug.Log($"🔌 입력 기기 전환: {newType}");
    }
}