using System.Collections;
using UnityEngine;

public class ObjectFader : MonoBehaviour {
    private Coroutine runningFade = null;

    /// <summary>
    /// Fades the object to a target alpha over a set duration.
    /// </summary>
    public void FadeTo(float targetAlpha, float duration)
    {
        // If a fade is already running, stop it
        if (runningFade != null)
        {
            StopCoroutine(runningFade);
        }
        // Start the new fade and store a reference to it
        runningFade = StartCoroutine(FadeCoroutine(targetAlpha, duration));
    }

    private IEnumerator FadeCoroutine(float targetAlpha, float duration)
    {
        // Try to get a CanvasGroup first (best for UI)
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            float startAlpha = canvasGroup.alpha;
            yield return FadeLogic(
                (alpha) => canvasGroup.alpha = alpha,
                startAlpha,
                targetAlpha,
                duration
            );
            yield break; // Exit after handling CanvasGroup
        }

        // Fallback to SpriteRenderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color startColor = spriteRenderer.color;
            yield return FadeLogic(
                (alpha) => spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha),
                startColor.a,
                targetAlpha,
                duration
            );
        }
    }

    // A generic helper to handle the core fade logic (to avoid repeating code)
    private IEnumerator FadeLogic(System.Action<float> setAlpha, float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            setAlpha(newAlpha);
            yield return null;
        }
        setAlpha(targetAlpha); // Ensure the final alpha is exact
    }
}