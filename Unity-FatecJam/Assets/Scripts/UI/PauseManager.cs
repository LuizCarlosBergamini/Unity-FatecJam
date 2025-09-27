using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{

    public InputActionReference pauseAction;
    public UnityEvent onPause;
    public UnityEvent onUnpause;

    public FadeUI countdownContainer;
    public TextMeshProUGUI countdownText;
    public int countdownDuration;

    public bool isPaused = false;
    
    private bool _disablePause = false;
    private int _disableDuration = 500; // Milliseconds


    void Update()
    {
        HandlePause();
    }

    private void HandlePause()
    {
        if (pauseAction.action.WasPressedThisFrame() && !_disablePause)
        {
            if (isPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    public async void Pause()
    {
        if (GameEvent.instance && !GameEvent.instance.isStarted) return;
        isPaused = true;
        _disablePause = true;
        onPause?.Invoke();
        Time.timeScale = 0f;
        GameEvent.instance.SetPause(true);
        Conductor.instance.Pause();
        await Task.Delay(_disableDuration);
        _disablePause = false;
    }

    public async void Unpause()
    {
        _disablePause = true;
        onUnpause?.Invoke();

        if (GameEvent.instance && GameEvent.instance.isStarted && countdownContainer != null && countdownText != null)
        {
            countdownContainer.Show();
            await Task.Delay(250);

            int time = 0;
            while (time <= countdownDuration)
            {
                countdownText.text = (countdownDuration - time).ToString();
                time++;
                await Task.Delay(1000);
            }

            GameEvent.instance.SetPause(false);
            Conductor.instance.Unpause();
            Time.timeScale = 1f;
            countdownContainer.InstantHide();
        } else
        {
            if (GameEvent.instance)
            {
                GameEvent.instance.SetPause(false);
                Conductor.instance.Unpause();
            }
            Time.timeScale = 1f;
            await Task.Delay(_disableDuration);
        }
        _disablePause = false;
        isPaused = false;
    }
}
