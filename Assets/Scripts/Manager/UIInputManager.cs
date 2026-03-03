using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputManager : MonoBehaviour
{
    private void Update()
    {
        // 1. 키보드 아무 키나 눌렸는지 확인
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Debug.Log("🎮 키보드 입력으로 전환");
        }

        // 2. 마우스가 움직였거나 버튼이 눌렸는지 확인
        else if (Mouse.current != null)
        {
            // 마우스 이동 감지 (Vector2.zero가 아니면 움직인 것)
            bool isMouseMoving = Mouse.current.delta.ReadValue() != Vector2.zero;
            // 마우스 버튼 클릭 감지
            bool isMouseClicked = Mouse.current.leftButton.wasPressedThisFrame ||
                                 Mouse.current.rightButton.wasPressedThisFrame;

            if (isMouseMoving || isMouseClicked)
            {
                Debug.Log("🖱️ 마우스 입력으로 전환");
            }
        }
    }
}