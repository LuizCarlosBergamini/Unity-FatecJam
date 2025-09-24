// FeedbackController.cs
using TMPro; // For TextMeshPro UI elements
using UnityEngine;
using static Lane;

public class FeedbackController : MonoBehaviour {
    [Header("Feedback Effects")]
    public GameObject[] hitExplosionPrefabs; // One for each judgment type
    public AudioClip hitSoundClip; // Corresponding sounds
    public AudioClip missSoundClip;
    //private Transform[] feedbackPositions; // Position for effects in each lane

    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI judgmentText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI scoreText;

    public AudioSource feedbackAudioSource;

    public bool powerUpActive = false;

    void Start()
    {
        feedbackAudioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        // Subscribe to game events
        GameEvent.onNoteJudged += OnNoteJudged;
        GameEvent.onPowerUpUsed += () => { powerUpActive = true; };
        GameEvent.onPowerUpEnded += () => { powerUpActive = false; };
    }

    void OnDisable()
    {
        GameEvent.onNoteJudged -= OnNoteJudged;
        GameEvent.onPowerUpUsed -= () => { powerUpActive = true; };
        GameEvent.onPowerUpEnded -= () => { powerUpActive = false; };
    }

    private void OnNoteJudged(Judgment judgment, int laneIndex)
    {
        //if (judgment == Judgment.Miss) return;

        // Play visual effect
        // NOTE: Use an object pool in a real project instead of Instantiate for performance
        //Instantiate(hitExplosionPrefabs[(int)judgment], feedbackPositions[laneIndex].position, Quaternion.identity);

        // Play sound effect
        //if (judgment != Judgment.Miss)
        //feedbackAudioSource.PlayOneShot(hitSoundClip);
        //else
        //feedbackAudioSource.PlayOneShot(missSoundClip);

        // Update UI Text
        judgmentText.text = judgment.ToString().ToUpper() + "!";
        accuracyText.text = ScoreManager.instance.accuracyRate.ToString("F2") + "%";
        scoreText.text = "Score: " + ScoreManager.instance.currentScore.ToString();
        // Animate the text (e.g., pop in and fade out)
        //...

        int currentCombo = ScoreManager.instance.currentCombo;
        if (powerUpActive) comboText.color = new Color32(0, 253, 219, 255); // Cyan color when power-up is active
        else comboText.color = Color.white;
        if (currentCombo > 1)
        {
            comboText.text = currentCombo.ToString() + "x";
            comboText.gameObject.SetActive(true);
        }
        else
        {
            comboText.gameObject.SetActive(false);
        }
    }
}