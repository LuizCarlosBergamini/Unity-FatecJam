using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ShowHideMenu : MonoBehaviour
{

    public Vector2 visiblePosition;
    public Vector2 hiddenPosition;
    public float transitionDuration = 0.5f;

    private RectTransform _rectTransform = null;

    public void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        Debug.Assert(_rectTransform != null, "RectTransform COmponent is required to ShowHideMenui Script");
    }

    public async void Hide(int delay = 0)
    {
        await Task.Delay(delay);
        await MoveTo(visiblePosition, hiddenPosition);
    }

    public async void Show(int delay = 0)
    {
        await Task.Delay(delay);
        await MoveTo(hiddenPosition, visiblePosition);
    }

    private async Task MoveTo(Vector2 start, Vector2 to)
    {
        if (_rectTransform == null) return;
        float time = 0f;

        while (time < transitionDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / transitionDuration);

            float easeT = Mathf.SmoothStep(0f, 1f, t);

            _rectTransform.anchoredPosition = Vector2.LerpUnclamped(start, to, easeT);
            await Task.Yield();
        }
    }
}
