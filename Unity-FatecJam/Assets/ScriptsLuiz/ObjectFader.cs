using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    private CancellationTokenSource fadeCancellation;

    /// <summary>
    /// Fades the object to a target alpha over a set duration.
    /// </summary>
    public async void FadeTo(float targetAlpha, float duration)
    {
        // Cancela fade anterior se existir
        fadeCancellation?.Cancel();
        fadeCancellation = new CancellationTokenSource();

        try
        {
            // Tenta CanvasGroup primeiro
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                float startAlpha = canvasGroup.alpha;
                await FadeLogic(
                    alpha => canvasGroup.alpha = alpha,
                    startAlpha,
                    targetAlpha,
                    duration,
                    fadeCancellation.Token
                );
                return; // já terminou no CanvasGroup
            }

            // Fallback: SpriteRenderer
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color startColor = spriteRenderer.color;
                await FadeLogic(
                    alpha => spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha),
                    startColor.a,
                    targetAlpha,
                    duration,
                    fadeCancellation.Token
                );
            }
        }
        catch (TaskCanceledException)
        {
            // Se o fade for cancelado, não faz nada
        }
    }

    /// <summary>
    /// Lógica genérica de fade.
    /// </summary>
    private async Task FadeLogic(Action<float> setAlpha, float startAlpha, float targetAlpha, float duration, CancellationToken token)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            token.ThrowIfCancellationRequested();

            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            setAlpha(newAlpha);

            await Task.Yield(); // espera próximo frame
        }

        setAlpha(targetAlpha); // garante o valor final
    }
}
