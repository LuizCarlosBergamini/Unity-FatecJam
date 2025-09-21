// Lane.cs
using UnityEngine;
using System.Collections.Generic;

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
            // Get the note at the front of the queue without removing it yet
            NoteMovement upcomingNote = notesInLane.Peek();

            // Calculate the accuracy of the hit
            float accuracy = GetHitAccuracy(upcomingNote);
            Judgment judgment = JudgeHit(accuracy);

            //// If the hit was valid (not too early)
            //if (judgment != Judgment.None)
            //{
            // Process the hit
            ProcessHit(upcomingNote, judgment);
            //}
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

        // Destroy the note object
        Destroy(note.gameObject);
    }

    public enum Judgment { None, Miss, Good, Great, Perfect }

    // Timing windows in seconds
    private const float PERFECT_WINDOW = 0.022f;
    private const float GREAT_WINDOW = 0.045f;
    private const float GOOD_WINDOW = 0.090f;

    private Judgment JudgeHit(float accuracy)
    {
        float absAccuracy = Mathf.Abs(accuracy);

        if (absAccuracy <= PERFECT_WINDOW)
            return Judgment.Perfect;
        if (absAccuracy <= GREAT_WINDOW)
            return Judgment.Great;
        if (absAccuracy <= GOOD_WINDOW)
            return Judgment.Good;

        // Note: This implementation doesn't penalize early hits outside the window.
        // A full implementation would need to handle that case, possibly by ignoring the input.
        // For now, we assume any hit outside the 'Good' window is not a valid hit on that note.
        return Judgment.Miss; // Or Miss, depending on game rules for early hits
    }
}