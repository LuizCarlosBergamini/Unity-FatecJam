using UnityEngine;

public class NoteController : MonoBehaviour
{
    public enum NoteType { Tap, Hold };
    // The precise time in the song (in seconds) when this note should be hit.
    public float timeStamp;
    // The lane index (e.g., 0 for left, 4 for right in a 5-lane game).
    public int laneIndex;
    // The type of note, such as a tap, hold, or strum.

    public void Initialize(float timestamp, Vector3 hitZonePos)
    {
        this.timeStamp = timestamp;
    }
}
