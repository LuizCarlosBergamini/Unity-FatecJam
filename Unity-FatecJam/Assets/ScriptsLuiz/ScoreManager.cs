using System;
using UnityEngine;
using static Lane;

public class ScoreManager : MonoBehaviour {
    public static ScoreManager instance;

    public int currentScore { get; private set; }
    public int currentCombo { get; private set; }
    public int highestCombo { get; private set; }
    public float accuracyRate { get; private set; }

    // Counters for each judgment type for final accuracy calculation
    private int perfects, greats, goods, misses;
    // flag to know if power-up is active
    private bool powerUpActive = false;

    void Awake()
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

    void OnEnable()
    {
        // Subscribe to the judgment event
        GameEvent.onNoteJudged += OnNoteJudged;
        GameEvent.onPowerUpUsed += () => { powerUpActive = true; };
        GameEvent.onPowerUpEnded += () => { powerUpActive = false; };
    }

    void OnDisable()
    {
        GameEvent.onNoteJudged -= OnNoteJudged;
        GameEvent.onPowerUpUsed -= () => { powerUpActive = true; };
        GameEvent.onPowerUpEnded -= () => { powerUpActive = false; };
    }

    private void OnNoteJudged(Judgment judgment, int laneIndex)
    {
        UpdateScore(judgment, laneIndex);
        UpdateCombo(judgment, laneIndex);
        UpdateJudgmentCounts(judgment);
        GetCurrentAccuracy();
    }

    private void UpdateScore(Judgment judgment, int laneIndex)
    {

        int scoreToAdd = 0;
        switch (judgment)
        {
            case Judgment.Perfect:
                scoreToAdd = (powerUpActive ? 600 : 300) * currentCombo;
                break;
            case Judgment.Great:
                scoreToAdd = (powerUpActive ? 400 : 200) * currentCombo;
                break;
            case Judgment.Good:
                scoreToAdd = (powerUpActive ? 200 : 100) * currentCombo;
                break;
            default:
                scoreToAdd = 0;
                break;
        }
        currentScore += scoreToAdd;
    }

    private void UpdateCombo(Judgment judgment, int laneIndex)
    {
        if (judgment == Judgment.Perfect || judgment == Judgment.Great || judgment == Judgment.Good)
        {
            currentCombo = powerUpActive ? currentCombo + 2 : currentCombo + 1;
            if (currentCombo > highestCombo)
            {
                highestCombo = currentCombo;
            }
        }
        else
        {
            // Combo breaks on Miss
            currentCombo = 0;
        }
    }

    private void UpdateJudgmentCounts(Judgment judgment)
    {
        switch (judgment)
        {
            case Judgment.Perfect: perfects++; break;
            case Judgment.Great: greats++; break;
            case Judgment.Good: goods++; break;
            case Judgment.Miss: misses++; break;
        }
    }

    public void GetCurrentAccuracy()
    {
        int totalNotes = perfects + greats + goods + misses;
        if (totalNotes == 0) accuracyRate = 100f;

        // Weighted accuracy calculation
        double totalScore = (perfects * 1.0) + (greats * 0.75) + (goods * 0.5);
        accuracyRate = (float)(totalScore / totalNotes) * 100f;
    }
}