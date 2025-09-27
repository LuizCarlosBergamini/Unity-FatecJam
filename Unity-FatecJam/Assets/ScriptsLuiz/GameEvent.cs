using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using static Lane;

public class GameEvent : MonoBehaviour
{
    public static GameEvent instance;
    public static event Action<Judgment, int> onNoteJudged;
    public static event Action onPowerUpUsed;
    public static event Action onPowerUpEnded;

    private int powerUpThreshold = 100;
    private int currentPowerUpCount = 0;
    public bool canUsePowerUp = false;
    public bool usingPowerUp = false;

    [Header("UI")]
    public Slider slider;

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

    private void Start()
    {
        slider.maxValue = powerUpThreshold;
        slider.value = currentPowerUpCount;
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
        slider.value = currentPowerUpCount;
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
        slider.value = currentPowerUpCount;
        if (currentPowerUpCount >= powerUpThreshold)
        {
            Debug.Log("Power-up is now available!");
            canUsePowerUp = true;
            currentPowerUpCount = 0;
        }
    }
}
