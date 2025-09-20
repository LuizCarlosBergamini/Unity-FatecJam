using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // TUDO TEMPORARIO, PODE EXLUIR OU USAR DE BASE

    public static GameManager Instance { get; private set; }

    public int totalScore = 0;
    public int streakLevel = 0;

    public static event Action<int> OnScoreChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddScore(int amount)
    {
        totalScore += amount;
        OnScoreChanged?.Invoke(totalScore);
    }
}
