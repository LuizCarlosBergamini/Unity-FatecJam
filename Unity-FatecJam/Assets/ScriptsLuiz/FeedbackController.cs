// FeedbackController.cs
using TMPro; // For TextMeshPro UI elements
using UnityEngine;
using static Lane;

public class FeedbackController : MonoBehaviour {
    [Header("Hit Feedback Effects")]
    public GameObject[] hitExplosionPrefabs; // One for each judgment type
    public AudioClip hitSoundClip; // Corresponding sounds
    public AudioClip missSoundClip;
    private Transform[] feedbackPositions; // Position for effects in each lane

    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI judgmentText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI scoreText;

    [Header("Lira Feedback Effects")]
    public GameObject lira;
    public Sprite[] liraImages;
    public GameObject liraEffectAnimations;

    [Header("AudioSource")]
    public AudioSource feedbackAudioSource;

    private bool powerUpActive = false;

    void Start()
    {
        feedbackAudioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        // Subscribe to game events
        GameEvent.onNoteJudged += OnNoteJudged;
        GameEvent.onPowerUpUsed += PowerUpUsed;
        GameEvent.onPowerUpEnded += PowerUpEnded;
    }

    void OnDisable()
    {
        GameEvent.onNoteJudged -= OnNoteJudged;
        GameEvent.onPowerUpUsed -= PowerUpUsed;
        GameEvent.onPowerUpEnded -= PowerUpEnded;
    }

    private void OnNoteJudged(Judgment judgment, int laneIndex)
    {
        //if (judgment == Judgment.Miss) return;

        // Play visual effect
        // NOTE: Use an object pool in a real project instead of Instantiate for performance
        Instantiate(hitExplosionPrefabs[(int)judgment], feedbackPositions[laneIndex].position, Quaternion.identity);

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

    private void PowerUpUsed()
    {
        powerUpActive = true;
        lira.GetComponent<SpriteRenderer>().sprite = liraImages[1];
        lira.transform.position = new Vector3(4, lira.transform.position.y, lira.transform.position.z);
        comboText.color = new Color32(0, 253, 219, 255);
        // Lira Effect Animations
        liraEffectAnimations.SetActive(true);
        liraEffectAnimations.GetComponent<Animator>().enabled = true;
        liraEffectAnimations.GetComponent<Animator>().Play("RayAnimation");
    }

    private void PowerUpEnded()
    {
        powerUpActive = false;
        lira.GetComponent<SpriteRenderer>().sprite = liraImages[0];
        lira.transform.position = new Vector3(4.20f, lira.transform.position.y, lira.transform.position.z);
        comboText.color = Color.white;
        // Lira Effect Animations
        liraEffectAnimations.SetActive(false);
        liraEffectAnimations.GetComponent<Animator>().enabled = false;
    }
}