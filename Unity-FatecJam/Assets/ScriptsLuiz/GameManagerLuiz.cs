// GameManager.cs
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum GameStage {
    Stage1,
    TransitionTo2,
    Stage2,
    TransitionTo3,
    Stage3,
    Finished
}

public class GameManagerLuiz : MonoBehaviour {
    public GameObject Lira;
    public static GameManagerLuiz instance;

    // Define the time boundaries for each stage in seconds
    private const float STAGE1_END = 115f;      // 1:55 -- 115f
    private const float STAGE2_START = 134f;    // 2:14 -- 134f
    private const float STAGE2_END = 211f;      // 3:31 -- 211f
    private const float STAGE3_START = 230f;    // 3:50 -- 230f
    private const float STAGE3_END = 307f;      // 5:07 -- 307f

    // A property to track the current stage
    public GameStage CurrentStage { get; private set; }

    public Conductor conductor;
    public NoteSpawner noteSpawner;

    [SerializeField] private UnityEvent OnStage1;
    [SerializeField] private UnityEvent OnTransitionTo2;
    [SerializeField] private UnityEvent OnStage2;
    [SerializeField] private UnityEvent OnTransitionTo3;
    [SerializeField] private UnityEvent OnStage3;
    [SerializeField] private UnityEvent OnFinish;

    private string beatmapFileName = "beatmap.json";

    public enum GameState { Ready, Playing, Finished }
    public GameState currentState { get; private set; }

    public bool isPaused = false;
    public bool isStarted = false;
    public bool inCutscene = false;
    public bool isGameOver = false;

    private Beatmap loadedBeatmap;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Debug.Log("Lira: " + Lira);
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
        //StartGameplay();
    }

    public void StartGameplay()
    {
        if (currentState == GameState.Ready)
        {
            isStarted = true;
            conductor.StartSong();
            currentState = GameState.Playing;
            // Set the initial stage
            CurrentStage = GameStage.Stage1;
            OnStageChanged(CurrentStage);
        }
    }

    async public void GameOver()
    {
        isGameOver = true;

        await Task.Delay(2000);

        if (conductor.TryGetComponent(out AudioSource _audio)) {
            _audio.Fade(_audio.volume, 0f, 3f);
        }

        if (LevelManager.instance)
        {
            LevelManager.instance.GameOver(0f);
            if (LevelManager.instance.TryGetComponent(out FadeAudio fader)) {
                fader.Show(0f);
            }
        }
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            // Check for end-of-song condition
            // This is a simple check; a more robust version would use the last note's timestamp
            if (!conductor.GetComponent<AudioSource>().isPlaying && isStarted && !isPaused)
            {
                EndGameplay();
            }

            // Get the current song position from the Conductor
            float songPosition = Conductor.instance.songPosition;

            // Determine the new stage based on the song position
            GameStage newStage = DetermineStage(songPosition);

            // If the stage has changed, update it and trigger the change logic
            if (newStage != CurrentStage)
            {
                Debug.LogError("Stage changed from " + CurrentStage + " to " + newStage);
                CurrentStage = newStage;
                OnStageChanged(CurrentStage);
            }
        }
    }

    // This function checks the time and returns the correct stage
    private GameStage DetermineStage(float time)
    {
        if (time < STAGE1_END) return GameStage.Stage1;
        if (time < STAGE2_START) return GameStage.TransitionTo2;
        if (time < STAGE2_END) return GameStage.Stage2;
        if (time < STAGE3_START) return GameStage.TransitionTo3;
        if (time < STAGE3_END) return GameStage.Stage3;

        return GameStage.Finished;
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

    // This function is called ONLY when the stage changes
    private void OnStageChanged(GameStage newStage)
    {
        Debug.Log("New Stage: " + newStage);
        inCutscene = false;
        // Use a switch statement to run your game logic for each stage
        switch (newStage)
        {
            case GameStage.Stage1:
                // Your logic for Stage 1
                // Lira.GetComponent<ObjectFader>().FadeTo(1f, 2f);
                if (LevelManager.instance && LevelManager.instance.TryGetComponent(out FadeAudio fader))
                {
                    fader.Hide(0f);
                }
                Lane.damageOnMiss = 7;
                OnStage1?.Invoke();
                break;

            case GameStage.TransitionTo2:
                if (PlayerManager.instance)
                    PlayerManager.instance.AddLife(100);
                inCutscene = true;
                // Your logic for the transition
                // Lira.GetComponent<ObjectFader>().FadeTo(0f, 2f);
                OnTransitionTo2?.Invoke();
                break;

            case GameStage.Stage2:
                // Your logic for Stage 2
                // Lira.GetComponent<ObjectFader>().FadeTo(1f, 2f);
                Lane.damageOnMiss = 12;
                OnStage2?.Invoke();
                break;

            case GameStage.TransitionTo3:
                if (PlayerManager.instance)
                    PlayerManager.instance.AddLife(100);
                inCutscene = true;
                // Your logic for the next transition
                // Lira.GetComponent<ObjectFader>().FadeTo(0f, 2f);
                OnTransitionTo3?.Invoke();
                break;

            case GameStage.Stage3:
                // Your logic for Stage 3
                // Lira.GetComponent<ObjectFader>().FadeTo(1f, 2f);
                Lane.damageOnMiss = 17;
                OnStage3?.Invoke();
                break;

            case GameStage.Finished:
                // Your logic for when the song is over
                // Lira.GetComponent<ObjectFader>().FadeTo(0f, 2f);
                OnFinish?.Invoke();
                break;
        }
    }
}
