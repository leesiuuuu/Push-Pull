using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class RebindingManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private InputActionRebindingExtensions.RebindingOperation _rebindOperation;

    private const string InputBindingsKey = "InputBindings";

    private readonly HashSet<string> _lockedActions = new HashSet<string>
    {
        //"Move",
        "Menu",
        "GrabControll",
        "UIControll"
    };

    public void StartRebinding(InputActionReference actionReference, string controlScheme, int compositePartOffset = 0)
    {
        if (actionReference == null) return;

        InputAction action = actionReference.action;

        if (_lockedActions.Contains(action.name)) return;

        int bindingIndex = action.GetBindingIndex(
            InputBinding.MaskByGroup(controlScheme)
        );
        if (bindingIndex == -1) return;

        int finalIndex = bindingIndex + compositePartOffset;

        if (finalIndex >= action.bindings.Count) return;

        action.Disable();
        _rebindOperation = action
            .PerformInteractiveRebinding(finalIndex)
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Mouse>/scroll")
            //.WithControlsExcluding("<Mouse>/leftButton")
            //.WithControlsExcluding("<Mouse>/rightButton")
            //.WithControlsExcluding("<Mouse>/middleButton")
            .WithCancelingThrough("<Keyboard>/escape")
            .WithoutGeneralizingPathOfSelectedControl()
            .OnMatchWaitForAnother(0.2f)
            .OnComplete(operation =>
            {
                action.Enable();
                _rebindOperation?.Dispose();
                _rebindOperation = null;
                SaveBindings();
            })
            .OnCancel(operation =>
            {
                action.Enable();
                _rebindOperation?.Dispose();
                _rebindOperation = null;
            })
            .Start();
    }

    public void ResetBinding(InputActionReference actionReference, int bindingIndex = 0)
    {
        if (actionReference == null) return;

        InputAction action = actionReference.action;

        if (_lockedActions.Contains(action.name)) return;

        action.RemoveBindingOverride(bindingIndex);
    }

    private void SaveBindings()
    {
        string json = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(InputBindingsKey, json);
        PlayerPrefs.Save();
    }

    public void LoadBindings()
    {
        if (PlayerPrefs.HasKey(InputBindingsKey))
            inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(InputBindingsKey));
    }

    private void OnDestroy() => _rebindOperation?.Dispose();
}