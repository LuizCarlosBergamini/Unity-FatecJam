// GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerLuiz : MonoBehaviour {

    public Conductor conductor;
    public NoteSpawner noteSpawner;
    //private ScoreManager scoreManager;
    //... other managers


    private string beatmapFileName = "beatmap.json";

    public enum GameState { Ready, Playing, Finished }
    public GameState currentState { get; private set; }

    private Beatmap loadedBeatmap;

    void Start()
    {
        // 1. Load Data
        string filePath = Application.streamingAssetsPath + "/" + beatmapFileName;
        loadedBeatmap = BeatmapParser.LoadBeatmap(filePath);

        if (loadedBeatmap == null)
        {
            Debug.LogError("Failed to load beatmap. Halting game.");
            return;
        }

        // 2. Initialize Systems
        // The Conductor is configured in the Inspector with the audio clip and BPM from the beatmap.
        // TODO: For a dynamic system, load the AudioClip here.
        //Conductor.instance.bpm = loadedBeatmap.bpm;
        noteSpawner.Initialize(loadedBeatmap);

        currentState = GameState.Ready;

        // In a real game, you would wait for player input or a countdown to start.
        // For this example, we start immediately.
        StartGameplay();
    }

    public void StartGameplay()
    {
        if (currentState == GameState.Ready)
        {
            conductor.StartSong();
            currentState = GameState.Playing;
        }
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            // Check for end-of-song condition
            // This is a simple check; a more robust version would use the last note's timestamp
            if (!conductor.GetComponent<AudioSource>().isPlaying && conductor.songPosition > 1f)
            {
                EndGameplay();
            }
        }
    }

    private void EndGameplay()
    {
        currentState = GameState.Finished;
        Debug.Log("Song Finished!");
        //Debug.Log("Final Score: " + scoreManager.currentScore);
        //Debug.Log("Highest Combo: " + scoreManager.highestCombo);
        //Debug.Log("Accuracy: " + scoreManager.GetCurrentAccuracy().ToString("F2") + "%");

        // Transition to results screen
        //...
    }
}