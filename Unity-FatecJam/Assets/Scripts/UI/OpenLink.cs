using UnityEngine;
using UnityEngine.EventSystems;

public class OpenLink : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private string url = "https://unity.com";

    public void Open()
    {
        Application.OpenURL(url);
    }

    public void OnPointerDown(PointerEventData ev)
    {
        Application.OpenURL(url);
    }
}
