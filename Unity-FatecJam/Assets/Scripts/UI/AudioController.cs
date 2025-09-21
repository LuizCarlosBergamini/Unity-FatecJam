using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AudioController : MonoBehaviour
{
    private float _currentVolume = 1.0f;

    private Slider slider = null;

    private bool HasAudioMixer () => GameManager.Instance && GameManager.Instance.audioMixer != null && slider != null;

    public void Start()
    {
        slider = GetComponent<Slider>();

        Debug.Assert(slider != null, "Slider is Required in AudioController");
        Debug.Assert(slider != null, "AudioMixer Reference is Required in AudioController");

        if (HasAudioMixer())
        {
            slider.onValueChanged.AddListener(SetVolume);
            float currentVolume;
            GameManager.Instance.audioMixer.GetFloat("Volume", out currentVolume);
            slider.value = Mathf.Pow(10, currentVolume / 20);
        }
    }

    public void SetVolume(float volume)
    {
        var newVolume = volume / slider.maxValue;
        if (HasAudioMixer() && _currentVolume != newVolume)
        {
            _currentVolume = newVolume;
            GameManager.Instance.audioMixer.SetFloat("Volume", newVolume > 0 ? Mathf.Log10(volume) * 20 : -80);
        }
    }
}
