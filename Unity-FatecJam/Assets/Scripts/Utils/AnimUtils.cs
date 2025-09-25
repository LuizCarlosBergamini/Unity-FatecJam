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
}
