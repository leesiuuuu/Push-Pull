using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputUI : MonoBehaviour
{
    [SerializeField] private RebindingManager rebindingManager;

    [SerializeField] private InputActionReference jumpAction;     
    [SerializeField] private InputActionReference moveAction;     
    [SerializeField] private InputActionReference uiSelectAction;  
    [SerializeField] private InputActionReference grabAction;       
    [SerializeField] private InputActionReference pushAction;

    [SerializeField] private ControlScheme scheme;


    public void OnClickRebindGrab() => rebindingManager.StartRebinding(grabAction, schemeToString(scheme));

    public void OnClickRebindMoveRight()
    {
        rebindingManager.StartRebinding(moveAction, schemeToString(scheme), 1);
    }

    public void OnClickRebindMoveLeft()
    {
        rebindingManager.StartRebinding(moveAction, schemeToString(scheme), 0);
    }

    private string schemeToString(ControlScheme scheme)
    {
        if (scheme == ControlScheme.Keyboard) return "Keyboard, Mouse";
        return "Gamepad";
    }
}

public enum ControlScheme
{
    Keyboard,
    Gamepad
}