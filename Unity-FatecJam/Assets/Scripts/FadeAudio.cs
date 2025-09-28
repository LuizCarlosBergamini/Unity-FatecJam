using System.Threading.Tasks;
using UnityEngine;

public class FadeAudio : MonoBehaviour
{
    public float visibleAlpha;
    public float hiddenAlpha;
    public float transitionDuration = 0.5f;

    private AudioSource _audioSource = null;

    public void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
        Debug.Assert(_audioSource != null, "CanvasGroup Component is required to FadeUI Script");
    }

    public async void Hide(float delay = 0f)
    {
        await Task.Delay((int)(delay * 1000f));
        await _audioSource.Fade(_audioSource.volume, hiddenAlpha, transitionDuration);
        _audioSource.Pause();
    }

    public async void Show(float delay = 0f)
    {
        _audioSource.Play();
        await Task.Delay((int)(delay * 1000f));
        await _audioSource.Fade(_audioSource.volume, visibleAlpha, transitionDuration);
    }
}
