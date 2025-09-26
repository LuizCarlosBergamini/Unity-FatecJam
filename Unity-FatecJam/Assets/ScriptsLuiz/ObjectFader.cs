using System.Collections;
using UnityEngine;

public class ObjectFader : MonoBehaviour {
    // Public method to start the fade-in process
    public void FadeIn(float duration)
    {
        StartCoroutine(FadeCoroutine(1f, duration));
    }

    // Public method to start the fade-out process
    public void FadeOut(float duration)
    {
        StartCoroutine(FadeCoroutine(0f, duration));
    }

    private IEnumerator FadeCoroutine(float targetAlpha, float duration)
    {
        // --- For UI Elements (Best Method) ---
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            float startAlpha = canvasGroup.alpha;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
                yield return null;
            }
            canvasGroup.alpha = targetAlpha;
            yield break; // Exit after handling CanvasGroup
        }

        // --- For Sprites and 3D Objects ---
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Debug.Log("SpriteRenderer: " + renderer);
        if (renderer != null)
        {
            // Note: This creates a new material instance
            Color startColor = renderer.color;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startColor.a, targetAlpha, elapsedTime / duration);
                renderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
                yield return null;
            }
            renderer.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        }
    }
}