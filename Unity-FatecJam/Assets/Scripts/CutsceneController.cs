using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct AlmaMap
{
    public FadeSprite outBoat;
    public FadeSprite inBoat;
}

public class CutsceneController : MonoBehaviour
{
    [SerializeField] List<Parallax> parallaxList;
    [SerializeField] List<AlmaMap> almas;
    [SerializeField] List<Dialog_SO> dialogs;
    [SerializeField] private Animator caronte;

    private int currentParallax = 0;

    async public void StartNavigate(float delay)
    {
        await Task.Delay((int)(delay * 1000f));

        parallaxList[currentParallax].Go(0);
        if (caronte != null)
        {
            caronte.SetTrigger("Navegar");
        }
    }

    async public void StopNavigate(float delay)
    {
        await Task.Delay((int)(delay * 1000f));

        parallaxList[currentParallax].Stop(0);
        if (caronte != null)
        {
            caronte.SetTrigger("Parar");
        }
    }

    public void TriggerCutscene()
    {
        if (MenuController.instance)
            MenuController.instance.Hide(0f);

        if (currentParallax <= almas.Count && almas[currentParallax].outBoat != null && almas[currentParallax].outBoat.TryGetComponent(out MoveTransform move))
        {
            move.Show(0);

            StopNavigate(move.transitionDuration - 3f);
            if (DialogManager.instance != null)
            {
                DialogManager.instance.StartDialog(move.transitionDuration);
            }
        }
    }

    async public void FinishDialog()
    {
        if (CoinEmiter.instance)
        {
            CoinEmiter.instance.Emit(0f);
        }

        if (currentParallax <= almas.Count)
        {
            if (almas[currentParallax].outBoat != null)
                almas[currentParallax].outBoat.Hide(0f);
            if (almas[currentParallax].inBoat != null)
                almas[currentParallax].inBoat.Show(1f);
        }

        if (DialogManager.instance != null && currentParallax + 1 < dialogs.Count)
            DialogManager.instance.SetNewDialogData(dialogs[currentParallax + 1]);

        StartNavigate(2f);
        ChangeParallax(6f);
        // Debug.Log(currentParallax);
        // if (currentParallax == 0)
        // {
        //     await Task.Delay(10000);
        //     TriggerCutscene();
        // }
    }

    async public void ChangeParallax(float delay)
    {
        await Task.Delay((int)(delay * 1000f));

        if (currentParallax < parallaxList.Count)
        {
            parallaxList[currentParallax].Hide(0);
            parallaxList[++currentParallax].Show(0);
        }
    }
}
