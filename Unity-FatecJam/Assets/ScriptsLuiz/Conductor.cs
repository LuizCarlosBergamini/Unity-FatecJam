using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor instance;

    public float bpm;
    public float songStartOffset; // Qualquer atraso inicial no áudio

    public AudioSource musicSource;

    private double dspSongTime;       // Tempo real em que a música começou
    private double pauseStartTime;    // Momento em que o pause começou
    private double totalPauseDuration; // Soma de todos os períodos em pausa

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
        secPerBeat = 60f / bpm;
    }

    public void StartSong()
    {
        dspSongTime = AudioSettings.dspTime;
        totalPauseDuration = 0;
        musicSource.Play();
    }

    public void Pause()
    {
        if (!musicSource.isPlaying) return;

        musicSource.Pause();
        pauseStartTime = AudioSettings.dspTime; // Marca quando pausou
    }

    public void Unpause()
    {
        if (musicSource.isPlaying) return;

        musicSource.UnPause();
        // Soma ao total o tempo que ficou pausado
        totalPauseDuration += AudioSettings.dspTime - pauseStartTime;
    }

    void Update()
    {
        if (!musicSource.isPlaying) return;

        // Ajuste: desconta o tempo total de pause
        songPosition = (float)((AudioSettings.dspTime - dspSongTime) - totalPauseDuration - songStartOffset);
        songPositionInBeats = songPosition / secPerBeat;
    }
}