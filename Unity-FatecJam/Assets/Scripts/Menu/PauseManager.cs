using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{

    InputAction pauseAction;
    public UnityEvent onPause;
    public UnityEvent onUnpause;

    public bool isPaused = false;

    void Start()
    {
        pauseAction = InputSystem.actions.FindAction("Pause");
    }

    void Update()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            isPaused = !isPaused;

            if (isPaused) Pause();
            else Unpause();
        }
    }

    public void Pause()
    {
        onPause?.Invoke();
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        onUnpause?.Invoke();
        Time.timeScale = 1f;
    }
}
