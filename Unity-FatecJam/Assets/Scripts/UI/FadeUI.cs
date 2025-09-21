using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeUI : MonoBehaviour
{

    public float visibleAlpha;
    public float hiddenAlpha;
    public float transitionDuration = 0.5f;

    private CanvasGroup _canvas = null;

    public void Start()
    {
        _canvas = GetComponent<CanvasGroup>();
        Debug.Assert(_canvas != null, "CanvasGroup Component is required to FadeUI Script");
    }

    public async void Hide(int delay = 0)
    {
        await Task.Delay(delay);
        await _canvas.FadeCanvas(_canvas.alpha, hiddenAlpha, transitionDuration);
    }

    public async void Show(int delay = 0)
    {
        await Task.Delay(delay);
        await _canvas.FadeCanvas(_canvas.alpha, visibleAlpha, transitionDuration);
    }


    public void InstantHide()
    {
        if (_canvas == null) return;

        _canvas.alpha = hiddenAlpha;
    }
}
