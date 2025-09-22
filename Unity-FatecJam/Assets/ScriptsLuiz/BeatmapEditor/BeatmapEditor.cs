using System.Collections.Generic;
using UnityEngine;

public class BeatmapEditor : MonoBehaviour
{
    // Inside the BeatmapEditor class
    void OnEnable()
    {
        InputManager.OnLaneActivated += HandleLaneActivation;
        InputManager.OnEditorSave += SaveBeatmap;
    }

    void OnDisable()
    {
        InputManager.OnLaneActivated -= HandleLaneActivation;
        InputManager.OnEditorSave -= SaveBeatmap;
    }

    public Conductor conductor;

    private List<NoteData> notes = new List<NoteData>();

    private void HandleLaneActivation(int laneIndex)
    {
        // 1. Immediately capture the precise timestamp from the Conductor
        double currentTimestamp = conductor.songPositionInBeats;

        // 2. Create a new Note data object
        NoteData newNote = new NoteData
        {
            timestamp = currentTimestamp,
            laneIndex = laneIndex
        };

        // 3. Add the new note to the in-memory list
        notes.Add(newNote);

        // 4. (Optional but recommended) Provide immediate user feedback
        Debug.Log($"Note created at Lane {laneIndex}, Time: {currentTimestamp}");
    }

    public void SaveBeatmap()
    {
        // 1. Create the top-level beatmap object
        Beatmap beatmapData = new Beatmap();

        // 2. Populate beatmap
        beatmapData.artistName = "Toby Fox";
        beatmapData.bpm = 240;
        beatmapData.songName = "Megalovania";

        // 3. Assign the recorded notes
        beatmapData.notes = this.notes;

        // 4. Serialize the C# object to a JSON string with pretty printing
        string jsonString = JsonUtility.ToJson(beatmapData, true);

        // 5. Write the string to a file (see next section)
        WriteToFile("beatmap.json", jsonString);
    }

    private void WriteToFile(string fileName, string content)
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
        System.IO.File.WriteAllText(path, content);
        Debug.Log($"Beatmap saved to: {path}");
    }
}
