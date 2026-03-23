using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputUI : MonoBehaviour
{
    [SerializeField] private RebindingManager rebindingManager;

    [Header("리바인딩 가능한 액션만 등록")]
    [SerializeField] private InputActionReference jumpAction;     
    [SerializeField] private InputActionReference uiSelectAction;  
    [SerializeField] private InputActionReference grabAction;       
    [SerializeField] private InputActionReference pushAction;      

    public void OnClickRebindGrab() => rebindingManager.StartRebinding(grabAction, "Keyboard, Mouse");
}
