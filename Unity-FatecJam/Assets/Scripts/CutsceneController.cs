using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] List<Parallax> parallaxList;
    [SerializeField] private Animator caronte;

    private int currentParallax = 0;

    public void StartNavigate(float delay)
    {
        Task.Delay((int)(delay * 1000f));

        parallaxList[currentParallax].Go(0);
        if (caronte != null)
        {
            caronte.SetTrigger("Navegar");
        }
    }

    public void StopNavigate(float delay)
    {
        Task.Delay((int)(delay * 1000f));

        parallaxList[currentParallax].Stop(0);
        if (caronte != null)
        {
            caronte.SetTrigger("Parar");
        }
    }

    public void FinishDialog()
    {
        if (CoinEmiter.instance)
        {
            CoinEmiter.instance.Emit(0f);
        }
    }

    async public void ChangeParallax(float delay)
    {
        await Task.Delay((int)(delay * 1000f));

        if (currentParallax < parallaxList.ToArray().Length)
        {
            parallaxList[currentParallax].Hide(0);
            parallaxList[++currentParallax].Show(0);
        }
    }
}
