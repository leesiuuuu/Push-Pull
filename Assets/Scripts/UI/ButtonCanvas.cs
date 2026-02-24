using System.Collections.Generic;
using UnityEngine;

public class ButtonCanvas : MonoBehaviour
{
    // 버튼을 모아둔 캔버스를 관리하는 코드
    // 방향키 / 패드 선택 시 처음으로 선택될 버튼을 관리
    // 또한 다음 캔버스로 이동 시 애니메이션을 관리

    [Tooltip("캔버스에 있는 버튼들을 오름차순으로 할당할 것.")]
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
