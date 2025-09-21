using UnityEngine;

public class Conductor : MonoBehaviour {
    // Singleton instance to ensure only one Conductor exists
    public static Conductor instance;


    public float bpm;
    public float songStartOffset; // Any initial delay in the audio file


    public AudioSource musicSource;

    // The precise time the song started, according to the audio engine's clock
    private double dspSongTime;

    // Calculated properties
    public float secPerBeat { get; private set; }
    public float songPosition { get; private set; }
    public float songPositionInBeats { get; private set; }

    void Awake()
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

    void Start()
    {
        // Calculate the duration of a single beat in seconds
        secPerBeat = 60f / bpm;
    }

    public void StartSong()
    {
        // Record the precise start time from the audio engine's clock
        dspSongTime = AudioSettings.dspTime;

        // Start playing the music
        musicSource.Play();
    }

    void Update()
    {
        if (!musicSource.isPlaying) return;

        // Calculate the current song position in seconds
        // This is the core of the timing engine: current DSP time minus the start time
        songPosition = (float)(AudioSettings.dspTime - dspSongTime) - songStartOffset;

        // Calculate the current song position in beats
        songPositionInBeats = songPosition / secPerBeat;
    }
}