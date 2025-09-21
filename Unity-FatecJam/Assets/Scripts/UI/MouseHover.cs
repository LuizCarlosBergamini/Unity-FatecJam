using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleHover = 1.1f;
    private float defaultScale;

    public void Start()
    {
        defaultScale = transform.localScale.x;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.activeInHierarchy)
        {
            transform.localScale = Vector2.one * scaleHover;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameObject.activeInHierarchy)
        {
            transform.localScale = Vector2.one * defaultScale;
        }
    }
}
