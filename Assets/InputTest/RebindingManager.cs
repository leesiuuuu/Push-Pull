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
        InputAction action = actionReference.action;
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var b = action.bindings[i];
            Debug.Log($"bindings[{i}] name={b.name}, path={b.path}, group={b.groups}, isComposite={b.isComposite}, isPartOfComposite={b.isPartOfComposite}");
        }
        if (actionReference == null)
        {
            Debug.LogWarning("[Rebind] actionReference is null");
            return;
        }

        //InputAction action = actionReference.action;
        Debug.Log($"[Rebind] Action: {action.name}, Type: {action.type}, ControlScheme: {controlScheme}");

        if (_lockedActions.Contains(action.name))
        {
            Debug.LogWarning($"[Rebind] '{action.name}' is locked");
            return;
        }

        int bindingIndex = action.GetBindingIndex(
            InputBinding.MaskByGroup(controlScheme)
        );
        Debug.Log($"[Rebind] bindingIndex (from MaskByGroup): {bindingIndex}");
        if (bindingIndex == -1)
        {
            Debug.LogWarning($"[Rebind] No binding found for scheme '{controlScheme}'");
            // 전체 바인딩 목록 출력
            for (int i = 0; i < action.bindings.Count; i++)
            {
                var b = action.bindings[i];
                Debug.Log($"  bindings[{i}] name={b.name}, path={b.path}, group={b.groups}, isComposite={b.isComposite}, isPartOfComposite={b.isPartOfComposite}");
            }
            return;
        }

        int finalIndex = bindingIndex + compositePartOffset;
        Debug.Log($"[Rebind] compositePartOffset: {compositePartOffset}, finalIndex: {finalIndex}");

        if (finalIndex >= action.bindings.Count)
        {
            Debug.LogWarning($"[Rebind] finalIndex {finalIndex} out of range (bindings.Count={action.bindings.Count})");
            return;
        }

        // finalIndex 바인딩 정보 출력
        var targetBinding = action.bindings[finalIndex];
        Debug.Log($"[Rebind] Target binding → name={targetBinding.name}, path={targetBinding.path}, group={targetBinding.groups}, isComposite={targetBinding.isComposite}, isPartOfComposite={targetBinding.isPartOfComposite}");

        action.Disable();
        _rebindOperation = action
            .PerformInteractiveRebinding(finalIndex)
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Mouse>/scroll")
            //.WithControlsExcluding("<Mouse>/leftButton")   // 추가
            //.WithControlsExcluding("<Mouse>/rightButton")  // 추가
            //.WithControlsExcluding("<Mouse>/middleButton") // 추가
            .WithCancelingThrough("<Keyboard>/escape")
            .WithoutGeneralizingPathOfSelectedControl()
            .OnMatchWaitForAnother(0.2f)
            .OnComplete(operation =>
            {
                Debug.Log($"[Rebind] Complete → new path: {operation.selectedControl?.path}");
                action.Enable();
                _rebindOperation?.Dispose();
                _rebindOperation = null;
                SaveBindings();
            })
            .OnCancel(operation =>
            {
                Debug.Log("[Rebind] Cancelled");
                action.Enable();
                _rebindOperation?.Dispose();
                _rebindOperation = null;
            })
            .Start();

        Debug.Log("[Rebind] PerformInteractiveRebinding started, waiting for input...");
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