// NoteSpawner.cs
using UnityEngine;

public class NoteSpawner : MonoBehaviour {
    [Header("Configuration")]
    //TO DO: Transform this into public variable to change game velocity/difficulty
    public float lookaheadTime = 4.0f; // How many seconds in advance to spawn notes
    public GameObject notePrefab;
    public Transform[] spawnPoints; // One transform for each lane's spawn position

    private Beatmap currentBeatmap;
    private int nextNoteIndex = 0;

    public Lane[] lanes;

    public void Initialize(Beatmap beatmap)
    {
        currentBeatmap = beatmap;
        nextNoteIndex = 0;
    }

    void Update()
    {
        if (currentBeatmap == null || nextNoteIndex >= currentBeatmap.notes.Count)
        {
            Debug.Log("currentBeatMap not initialized");
            return; // No beatmap loaded or all notes have been spawned
        }

        // Get current song time from the Conductor
        float songTime = Conductor.instance.songPosition;
        float secPerBeat = Conductor.instance.secPerBeat;

        // Check if the next note in the list is ready to be spawned
        NoteData nextNote = currentBeatmap.notes[nextNoteIndex];
        double noteTimestampInSeconds = nextNote.timestamp * secPerBeat;

        if (noteTimestampInSeconds < songTime + lookaheadTime)
        {
            // Spawn the note
            SpawnNote(nextNote);

            nextNoteIndex++;
        }
    }

    private void SpawnNote(NoteData noteData)
    {
        Debug.Log(noteData.laneIndex);
        Debug.Log(spawnPoints);
        if (noteData.laneIndex < 0 || noteData.laneIndex >= spawnPoints.Length)
        {
            Debug.LogError("Invalid lane index for note: " + noteData.laneIndex);
            return;
        }

        Transform spawnPoint = spawnPoints[noteData.laneIndex];
        GameObject noteObject = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);

        lanes[noteData.laneIndex].AddNoteToLane(noteObject.GetComponent<NoteMovement>());

        // Initialize the note object with its data
        noteObject.GetComponent<NoteMovement>().Initialize(noteData, lookaheadTime);
    }
}