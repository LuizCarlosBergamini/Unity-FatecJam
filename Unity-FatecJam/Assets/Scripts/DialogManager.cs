using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEditor.Rendering;

public class DialogManager : MonoBehaviour
{
    [Header("Configurações")]
    public Dialog_SO dialogData;
    public InputActionReference skipAction;
    public int typeSpeed = 50;
    public int clearTypeSpeed = 1;
    public bool instantClear = false;

    [Header("Referências UI")]
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI entityNameText;
    public Image entityIcon;

    [Header("Eventos")]
    public UnityEvent onDialogStart;
    public UnityEvent onDialogFinish;

    private int currentDialogIndex = 0;
    private bool isTyping = false;
    private bool skipRequested = false;

    private void OnEnable()
    {
        dialogText.text = "";
        entityNameText.text = "";
        StartDialog();
        skipAction.action.Enable();
    }

    private void OnDisable()
    {
        skipAction.action.Disable();
    }

    private void Update()
    {
        if (skipAction.action.WasPressedThisFrame())
        {
            if (isTyping)
            {
                skipRequested = true;
            }
            else
            {
                NextDialog();
            }
        }
    }

    public void StartDialog()
    {
        if (dialogData == null) return;
        onDialogStart?.Invoke();
        currentDialogIndex = 0;
        ShowDialog();
    }

    public void SetNewDialogData(Dialog_SO newDialog)
    {
        dialogData = newDialog;
    }

    async private void ShowDialog()
    {
        if (currentDialogIndex >= dialogData.dialogs.Count)
        {
            EndDialog();
            return;
        }
        if (currentDialogIndex > 0 && !instantClear)
        {
            await CleanTypeText();
        }

        var dialog = dialogData.dialogs[currentDialogIndex];


        if (entityIcon != null)
            entityIcon.sprite = dialog.entity.icon;

        TypeText(dialog.text, dialog.entity.name);
    }


    async private void TypeText(string text, string entityName)
    {
        isTyping = true;
        skipRequested = false;
        dialogText.maxVisibleCharacters = 0;
        dialogText.text = text;
        entityNameText.maxVisibleCharacters = 0;
        entityNameText.text = entityName;

        for (int i = 0; i < Mathf.Max(text.Length, entityName.Length); i++)
        {
            if (skipRequested)
            {
                dialogText.maxVisibleCharacters = int.MaxValue;
                entityNameText.maxVisibleCharacters = int.MaxValue;
                break;
            }
            dialogText.maxVisibleCharacters++;
            entityNameText.maxVisibleCharacters++;
            await Task.Delay(typeSpeed);
        }

        isTyping = false;
    }

    async private Task CleanTypeText()
    {
        isTyping = true;
        skipRequested = false;
        int maxVisibleCharacters = Mathf.Max(dialogText.text.Length, entityNameText.text.Length);
        dialogText.maxVisibleCharacters = maxVisibleCharacters;
        entityNameText.maxVisibleCharacters = maxVisibleCharacters;

        for (int i = 0; i < maxVisibleCharacters; i++)
        {
            if (skipRequested)
            {
                dialogText.maxVisibleCharacters = 0;
                entityNameText.maxVisibleCharacters = 0;
                break;
            }
            dialogText.maxVisibleCharacters--;
            entityNameText.maxVisibleCharacters--;
            await Task.Delay(clearTypeSpeed);
        }

        isTyping = false;
    }

    private void NextDialog()
    {
        currentDialogIndex++;
        ShowDialog();
    }

    private void EndDialog()
    {
        onDialogFinish?.Invoke();
    }
}
