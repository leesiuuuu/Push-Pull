using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class RebindingManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private InputActionRebindingExtensions.RebindingOperation _rebindOperation;

    private readonly HashSet<string> _lockedActions = new HashSet<string>
    {
        "Move",
        "Menu",
        "GrabControll",
        "UIControll"
    };

    public void StartRebinding(InputActionReference actionReference, string controlScheme = "Keyboard&Mouse")
    {
        if (actionReference == null) return;

        Debug.Log("1");

        InputAction action = actionReference.action;

        if (_lockedActions.Contains(action.name)) return;

        Debug.Log("2");

        int bindingIndex = action.GetBindingIndex(
            InputBinding.MaskByGroup(controlScheme)
        );

        if (bindingIndex == -1)
        {
            Debug.LogWarning($"[{action.name}] {controlScheme} 바인딩 없음");
            return;
        }

        action.Disable();

        _rebindOperation = action
            .PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Mouse>/scroll")
            .WithCancelingThrough("<Keyboard>/escape")
            .WithoutGeneralizingPathOfSelectedControl()
            .OnMatchWaitForAnother(0.2f)
            .OnComplete(operation =>
            {
                Debug.Log(3);
                action.Enable();
                _rebindOperation?.Dispose();
                _rebindOperation = null;
                SaveBindings();
                Debug.Log($"[{action.name}] [{controlScheme}] 리바인딩 완료: {action.bindings[bindingIndex].effectivePath}");
            })
            .OnCancel(operation =>
            {
                action.Enable();
                _rebindOperation?.Dispose();
                _rebindOperation = null;
                Debug.Log($"[{action.name}] 리바인딩 취소");
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
        PlayerPrefs.SetString("InputBindings", json);
        PlayerPrefs.Save();
    }

    public void LoadBindings()
    {
        if (PlayerPrefs.HasKey("InputBindings"))
            inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString("InputBindings"));
    }

    private void OnDestroy() => _rebindOperation?.Dispose();
}