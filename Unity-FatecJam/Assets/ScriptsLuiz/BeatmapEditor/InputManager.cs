using System;
using UnityEngine;

public class InputManager : MonoBehaviour {
    // Event to broadcast lane activation, passing the lane index
    public static event Action<int> OnLaneActivated;
    public static event Action OnEditorSave;

    // Mappable keys for each lane
    public KeyCode[] laneKeys = { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K, KeyCode.L };

    void Update()
    {
        // Poll for key presses each frame
        for (int i = 0; i < laneKeys.Length; i++)
        {
            if (Input.GetKeyDown(laneKeys[i]))
            {
                // Broadcast the event with the corresponding lane index
                OnLaneActivated?.Invoke(i);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnEditorSave?.Invoke();
        }
    }
}