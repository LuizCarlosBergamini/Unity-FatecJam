using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private float scaleHover = 1.1f;
    private float defaultScale;

    [Header("Interação com Áudio (Requer AudioSource)")]
    [SerializeField] private AudioClip hoverAudioClip;
    [SerializeField] private AudioClip clickAudioClip;

    private AudioSource _audioSource;

    public void Start()
    {
        defaultScale = transform.localScale.x;
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.activeInHierarchy)
        {
            if (_audioSource != null && clickAudioClip != null)
            {
                RandomizeAudioSettings();
                _audioSource.PlayOneShot(clickAudioClip);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.activeInHierarchy)
        {
            transform.localScale = Vector2.one * scaleHover;

            if (_audioSource != null && hoverAudioClip != null)
            {
                RandomizeAudioSettings();
                _audioSource.PlayOneShot(hoverAudioClip);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameObject.activeInHierarchy)
        {
            transform.localScale = Vector2.one * defaultScale;
        }
    }

    private void RandomizeAudioSettings()
    {
        if (_audioSource != null)
        {
            _audioSource.pitch = Random.Range(0.0f, 1.0f) * .4f + 0.8f;
            _audioSource.volume = Random.Range(0.0f, 1.0f) * .4f + 0.8f;
        }
    }
}
