using UnityEngine;
using System;
using System.Threading;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour {
    // Assign lane controllers in the inspector
    public Lane[] lanes;
    // Map keys to lane indices
    private List<InputActionReference> laneKeys;

    private float powerUpTimer = 8.0f;

    [SerializeField] InputActionReference lane1Action;
    [SerializeField] InputActionReference lane2Action;
    [SerializeField] InputActionReference lane3Action;
    [SerializeField] InputActionReference lane4Action;
    [SerializeField] InputActionReference lane5Action;
    [SerializeField] InputActionReference interactAction;

    private void OnEnable()
    {
        laneKeys = new List<InputActionReference>() { lane1Action, lane2Action, lane3Action, lane4Action, lane5Action };
    }

    void Update()
    {
        if (GameEvent.instance != null && (GameEvent.instance.isPaused || !GameEvent.instance.isStarted)) return;

        for (int i = 0; i < lanes.Length; i++)
        {
            if (laneKeys[i].action.WasPerformedThisFrame())
            {
                lanes[i].OnInput();
            }
        }

        if (GameEvent.instance.canUsePowerUp && interactAction.action.WasPerformedThisFrame())
        {
            GameEvent.instance.OnPowerUpUsed();
            StartTimer();
        }
    }

    private async void StartTimer()
    {
        while (powerUpTimer > 0)
        {
            await System.Threading.Tasks.Task.Delay(1000);
            powerUpTimer--;
            Debug.Log($"Power-up time remaining: {powerUpTimer} seconds");
        }
        GameEvent.instance.OnPowerUpEnded();
        powerUpTimer = 8.0f;
    }
}