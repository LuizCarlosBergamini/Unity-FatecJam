// Lane.cs
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;

public class Lane : MonoBehaviour {
    private Queue<NoteMovement> notesInLane = new Queue<NoteMovement>();

    // Called by NoteSpawner when a note for this lane is created
    public void AddNoteToLane(NoteMovement note)
    {
        notesInLane.Enqueue(note);
    }

    // Called by PlayerInput on key press
    public void OnInput()
    {
        if (notesInLane.Count > 0)
        {
            NoteMovement upcomingNote = notesInLane.Peek();
            float accuracy = GetHitAccuracy(upcomingNote);

            // Only process hits within a reasonable time window
            // Allow for early hits within -GOOD_WINDOW time
            if (accuracy > -GOOD_WINDOW)
            {
                Judgment judgment = JudgeHit(accuracy);
                ProcessHit(upcomingNote, judgment);
                GameEvent.instance.OnNoteJudged(judgment, upcomingNote.noteData.laneIndex);
            }
            else
            {
                Debug.Log($"Hit too early: {accuracy}");
            }
        }
    }

    private float GetHitAccuracy(NoteMovement note)
    {
        float songTime = Conductor.instance.songPosition;
        float secPerBeat = Conductor.instance.secPerBeat;
        double noteTargetTime = note.noteData.timestamp * secPerBeat;

        // Returns the time difference in seconds
        return (float)(songTime - noteTargetTime);
    }

    private void ProcessHit(NoteMovement note, Judgment judgment)
    {
        // Remove the note from the queue
        notesInLane.Dequeue();

        // Notify other systems (ScoreManager, FeedbackController)
        // GameEvents.instance.OnNoteJudged(note.noteData.laneIndex, judgment);
        if (note != null)
        {
            // Destroy the note object
            Destroy(note.gameObject);
        }
    }

    public enum Judgment { None, Miss, Good, Great, Perfect }

    // Timing windows in seconds
    private const float PERFECT_WINDOW = 0.032f;
    private const float GREAT_WINDOW = 0.055f;
    private const float GOOD_WINDOW = 0.100f;

    void Update()
    {
        // Only check for misses if there's a note in the lane.
        if (notesInLane.Count > 0)
        {
            NoteMovement upcomingNote = notesInLane.Peek();
            float noteTargetTime = (float)(upcomingNote.noteData.timestamp * Conductor.instance.secPerBeat);
            float songTime = Conductor.instance.songPosition;

            // Check if the note has passed the judgment line by more than the miss threshold.
            // A note is missed if the current time is past its target time plus the lenient "Good" window.
            if (songTime > noteTargetTime + GOOD_WINDOW)
            {
                // Dequeue the note and process it as a Miss.
                NoteMovement missedNote = notesInLane.Dequeue();
                GameEvent.instance.OnNoteJudged(Judgment.Miss, missedNote.noteData.laneIndex);
                PlayerManager.instance.RemoveLife(3);
                Destroy(missedNote.gameObject);
            }
        }
    }

    private Judgment JudgeHit(float accuracy)
    {
        float absAccuracy = Mathf.Abs(accuracy);

        if (absAccuracy <= PERFECT_WINDOW)
        {
            PlayerManager.instance.AddLife(3);
            GameEvent.instance.CountToPowerUp(3);
            return Judgment.Perfect;
        }
        if (absAccuracy <= GREAT_WINDOW)
        {
            PlayerManager.instance.AddLife(2);
            GameEvent.instance.CountToPowerUp(2);
            return Judgment.Great;
        }
        if (absAccuracy <= GOOD_WINDOW)
        {
            PlayerManager.instance.AddLife(1);
            GameEvent.instance.CountToPowerUp(1);
            return Judgment.Good;
        }

        // Note: This implementation doesn't penalize early hits outside the window.
        // A full implementation would need to handle that case, possibly by ignoring the input.
        // For now, we assume any hit outside the 'Good' window is not a valid hit on that note.
        PlayerManager.instance.RemoveLife(3);
        return Judgment.Miss; // Or Miss, depending on game rules for early hits
    }
}