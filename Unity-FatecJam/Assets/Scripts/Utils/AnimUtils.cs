using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public static class AnimUtils
{
    public async static Task MoveAnchor(this RectTransform rect, Vector2 from, Vector2 to, float duration)
    {
        if (rect == null) return;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / duration);

            float easeT = Mathf.SmoothStep(0f, 1f, t);

            rect.anchoredPosition = Vector2.LerpUnclamped(from, to, easeT);
            await Task.Yield();
        }
    }

    public async static Task FadeCanvas(this CanvasGroup canvas, float from, float to, float duration)
    {
        if (canvas == null) return;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / duration);

            canvas.alpha = Mathf.SmoothStep(from, to, t);
            await Task.Yield();
        }
    }

    public async static Task MoveTransform(this Transform transform, Vector2 from, Vector2 to, float duration)
    {
        if (transform == null) return;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / duration);

            float easeT = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector2.LerpUnclamped(from, to, easeT);
            await Task.Yield();
        }
    }

    public async static Task FadeSpriteAsync(this SpriteRenderer sprite, float from, float to, float duration)
    {
        if (sprite == null) return;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / duration);

            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.SmoothStep(from, to, t));
            await Task.Yield();
        }
    }

    public async static void FadeSprite(this SpriteRenderer sprite, float from, float to, float duration)
    {
        if (sprite == null) return;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / duration);

            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.SmoothStep(from, to, t));
            await Task.Yield();
        }
    }

    public async static void SmoothParallaxVelocity(this Parallax parallax, float from, float to, float duration)
    {
        if (parallax == null) return;
        float time = 0f;

        while (time < duration)
        {
             time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / duration);

            parallax.velocityMultiplier = Mathf.SmoothStep(from, to, t);
            await Task.Yield();
        }
    }
}
