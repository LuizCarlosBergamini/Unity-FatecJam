using System;
using UnityEditorInternal;
using UnityEngine;
using static Lane;

public class GameEvent : MonoBehaviour
{
    public static GameEvent instance;
    public static event Action<Judgment, int> onNoteJudged;
    public static event Action onPowerUpUsed;
    public static event Action onPowerUpEnded;

    private int powerUpThreshold = 30;
    private int currentPowerUpCount = 0;
    public bool canUsePowerUp = false;
    public bool usingPowerUp = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnNoteJudged(Judgment judgment, int laneIndex)
    {
        onNoteJudged?.Invoke(judgment, laneIndex);
    }

    public void OnPowerUpUsed()
    {
        Debug.LogWarning("Power-up used event triggered");
        onPowerUpUsed?.Invoke();
        canUsePowerUp = false;
        usingPowerUp = true;
    }

    public void OnPowerUpEnded()
    {
        onPowerUpEnded?.Invoke();
        usingPowerUp = false;
    }

    public void CountToPowerUp(int amount)
    {
        if (canUsePowerUp) return;
        currentPowerUpCount += amount;
        if (currentPowerUpCount >= powerUpThreshold)
        {
            Debug.Log("Power-up is now available!");
            canUsePowerUp = true;
            currentPowerUpCount = 0;
        }
    }
}
