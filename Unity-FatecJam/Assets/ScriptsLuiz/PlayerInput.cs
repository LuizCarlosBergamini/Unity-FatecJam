using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour {
    // Assign lane controllers in the inspector
    public Lane[] lanes;
    // Map keys to lane indices
    private KeyCode[] laneKeys = { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K, KeyCode.L };

    void Update()
    {
        for (int i = 0; i < lanes.Length; i++)
        {
            if (Input.GetKeyDown(laneKeys[i]))
            {
                lanes[i].OnInput();
            }
        }
    }
}