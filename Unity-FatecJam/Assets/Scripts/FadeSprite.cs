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

    public async void Hide(float delay = 0f)
    {
        await Task.Delay((int)(delay * 1000f));
        await _sprite.FadeSpriteAsync(_sprite.color.a, hiddenAlpha, transitionDuration);
        gameObject.SetActive(false);
    }

    public async void Show(float delay = 0f)
    {
        gameObject.SetActive(true);
        await Task.Delay((int)(delay * 1000f));
        await _sprite.FadeSpriteAsync(_sprite.color.a, visibleAlpha, transitionDuration);
    }
}
