using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

public class KeyMap : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI displayText;
    private bool _awaitingInput = false;

    [SerializeField] private InputActionReference actionReference;
    [SerializeField] private int actionMapIndex;
    private InputAction _action;

    private void Awake()
    {
        _action = actionReference.action;
    }

    public void OnEnable()
    {
        UpdateDisplayText();
    }


    public void OpenInputKey()
    {
        if (_awaitingInput) return;

        _awaitingInput = true;
        displayText.text = "Pressione uma tecla...";
        
        Color newColor = displayText.color;
        newColor.a = 0.6f;
        displayText.color = newColor;

        _action.Disable();
        
        _action.PerformInteractiveRebinding()
            .WithTargetBinding(actionMapIndex)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation =>
            {
                operation.Dispose();
                _action.Enable();
                UpdateDisplayText();
            })
            .OnCancel(operation =>
            {
                operation.Dispose();
                _action.Enable();
                UpdateDisplayText();
            })
            .Start();
    }

    private void UpdateDisplayText()
    {
        Color newColor = displayText.color;
        newColor.a = 1f;
        displayText.color = newColor;
        
        displayText.text = _action.GetBindingDisplayString(actionMapIndex);
        _awaitingInput = false;
    }
}
