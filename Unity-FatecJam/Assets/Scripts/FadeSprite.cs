using System.Threading.Tasks;
using UnityEngine;

public class FadeSprite : MonoBehaviour
{
    public float visibleAlpha;
    public float hiddenAlpha;
    public float transitionDuration = 0.5f;

    private SpriteRenderer _sprite = null;

    public void OnEnable()
    {
        _sprite = GetComponent<SpriteRenderer>();
        Debug.Assert(_sprite != null, "Sprite Component is required to Fade Script");
    }

    public async void Hide(int delay = 0)
    {
        await Task.Delay(delay);
        await _sprite.FadeSprite(_sprite.color.a, hiddenAlpha, transitionDuration);
        gameObject.SetActive(false);
    }

    public async void Show(int delay = 0)
    {
        gameObject.SetActive(true);
        await Task.Delay(delay);
        await _sprite.FadeSprite(_sprite.color.a, visibleAlpha, transitionDuration);
    }
}
