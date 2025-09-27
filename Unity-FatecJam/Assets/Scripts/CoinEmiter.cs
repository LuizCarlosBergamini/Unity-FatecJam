using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CoinEmiter : MonoBehaviour
{
    public static CoinEmiter instance;
    private AudioSource _audioSource;

    void Start()
    {
        if (CoinEmiter.instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        _audioSource = GetComponent<AudioSource>();
    }

    async public void Emit(float delay)
    {
        await Task.Delay((int)(delay * 1000f));
        _audioSource.Play();
    }
}
