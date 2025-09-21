[System.Serializable]
public class NoteData {
    public double timestamp; // The time in beats when the note should be hit
    public int laneIndex;    // The zero-based index of the lane
    // Future expansion: public NoteType type; (e.g., Tap, HoldStart, HoldEnd)
}
