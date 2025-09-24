using UnityEngine;
using System;
using System.Threading;

public class PlayerInput : MonoBehaviour {
    // Assign lane controllers in the inspector
    public Lane[] lanes;
    // Map keys to lane indices
    private KeyCode[] laneKeys = { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K, KeyCode.L };

    private float powerUpTimer = 8.0f;

    void Update()
    {
        for (int i = 0; i < lanes.Length; i++)
        {
            if (Input.GetKeyDown(laneKeys[i]))
            {
                lanes[i].OnInput();
            }
        }
        if (GameEvent.instance.canUsePowerUp && Input.GetKeyDown(KeyCode.Space))
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