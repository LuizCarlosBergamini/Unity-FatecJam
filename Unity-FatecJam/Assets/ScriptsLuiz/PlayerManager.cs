using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;
    public int life;
    public int LIFECAP = 100;

    private GameObject[] souls;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            life = 100; // Initial life
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Find all souls at the start
        souls = GameObject.FindGameObjectsWithTag("Soul");
        // Set their initial visibility based on full health
        UpdateSoulVisibility();
    }

    public void RemoveLife(int amount)
    {
        life -= amount;
        if (life < 0) life = 0; // Prevent life from going below zero

        // After changing life, update the visuals
        UpdateSoulVisibility();
    }

    public void AddLife(int amount)
    {
        life += amount;
        if (life > LIFECAP) life = LIFECAP; // Cap life at 100

        // After changing life, update the visuals
        UpdateSoulVisibility();
    }

    /// <summary>
    /// Determines the correct alpha for the souls based on current life and fades them.
    /// </summary>
    private void UpdateSoulVisibility()
    {
        float targetAlpha = 1f; // Default to fully visible

        // Use if/else if to handle the ranges correctly
        if (life > 75)
        {
            targetAlpha = 1f;
        }
        else if (life > 50) // Life is between 51 and 75
        {
            targetAlpha = 0.8f;
        }
        else if (life > 25) // Life is between 26 and 50
        {
            targetAlpha = 0.6f;
        }
        else if (life > 0) // Life is between 1 and 25
        {
            targetAlpha = 0.4f;
        }
        else // Life is 0
        {
            targetAlpha = 0f;
        }

        // Fade all souls to the calculated target alpha
        foreach (GameObject soul in souls)
        {
            // We can simplify the call since FadeIn and FadeOut do the same thing
            soul.GetComponent<ObjectFader>().FadeTo(targetAlpha, 0.5f);
        }
    }
}