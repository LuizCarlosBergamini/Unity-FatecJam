using System;
using UnityEngine;
using static Lane;

public class GameEvent : MonoBehaviour
{
    public static GameEvent instance;
    public static event Action<Judgment, int> onNoteJudged;
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
}
