using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MoveUI : MonoBehaviour
{

    public Vector2 visiblePosition;
    public Vector2 hiddenPosition;
    public float transitionDuration = 0.5f;

    private RectTransform _rectTransform = null;

    public void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        Debug.Assert(_rectTransform != null, "RectTransform Component is required to MoveUI Script");
    }

    public async void Hide(int delay = 0)
    {
        await Task.Delay(delay);
        await _rectTransform.MoveAnchor(_rectTransform.anchoredPosition, hiddenPosition, transitionDuration);
    }

    public async void Show(int delay = 0)
    {
        await Task.Delay(delay);
        await _rectTransform.MoveAnchor(_rectTransform.anchoredPosition, visiblePosition, transitionDuration);
    }
}
