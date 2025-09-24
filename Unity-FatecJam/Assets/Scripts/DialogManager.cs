using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;
using System.Threading.Tasks;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [Header("Configurações")]
    public Dialog_SO dialogData;
    public float typeSpeed = 0.05f;
    public InputActionReference skipAction;

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

    private void ShowDialog()
    {
        if (currentDialogIndex >= dialogData.dialogs.Count)
        {
            EndDialog();
            return;
        }

        var dialog = dialogData.dialogs[currentDialogIndex];

        // Atualiza informações da entidade
        if (entityNameText != null)
            entityNameText.text = dialog.entity.name;

        if (entityIcon != null)
            entityIcon.sprite = dialog.entity.icon;

        TypeText(dialog.text);
    }

    async private void TypeText(string text)
    {
        isTyping = true;
        skipRequested = false;
        dialogText.maxVisibleCharacters = 0;
        dialogText.text = text;

        for (int i = 0; i < text.Length; i++)
        {
            if (skipRequested)
            {
                dialogText.maxVisibleCharacters = int.MaxValue;
                dialogText.text = text;
                break;
            }
            dialogText.maxVisibleCharacters++;
            await Task.Delay((int)(typeSpeed * 1000));
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
