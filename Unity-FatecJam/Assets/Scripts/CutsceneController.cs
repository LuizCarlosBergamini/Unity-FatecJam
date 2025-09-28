using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField] public List<UnityEvent> OnFisish;
    [SerializeField] public AudioSource cutsceneAudio;
    [SerializeField] public AudioSource corvo;

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

    async public void TriggerCutscene()
    {
        if (MenuController.instance)
            MenuController.instance.Hide(0f);

        if (currentParallax <= almas.Count && almas[currentParallax].outBoat != null)
        {
            MoveTransform move = almas[currentParallax].outBoat.GetComponent<MoveTransform>();
            if (move == null)
                move = almas[currentParallax].outBoat.GetComponentInParent<MoveTransform>();

            move.Show(0);

            StopNavigate(move.transitionDuration - 3f);
            if (DialogManager.instance != null)
            {
                DialogManager.instance.StartDialog(move.transitionDuration);
            }

            GameManagerLuiz.instance.inCutscene = true;
            GameManagerLuiz.instance.isPaused = true;
            await Conductor.instance.GetComponent<AudioSource>().Fade(0.25f, 0f, 1f);
            Conductor.instance.Pause();
            cutsceneAudio.volume = 0;
            cutsceneAudio.Play();
            cutsceneAudio.Fade(0f, 0.25f, 1f);
        }
    }

    async public void FinishDialog()
    {
        Debug.Log("current: " + currentParallax);
        if (currentParallax == 3)
        {
            // Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            corvo.Play();
            LevelManager.instance.ReloadScene();
            return;
        }
        if (CoinEmiter.instance)
        {
            CoinEmiter.instance.Emit(0f);
        }

        if (currentParallax <= almas.Count)
        {
            if (almas[currentParallax].outBoat != null)
            {
                almas[currentParallax].outBoat.Hide(0f);
                if (almas[currentParallax].outBoat.transform.parent.GetChild(1).TryGetComponent(out FadeSprite fade2))
                {
                    fade2.Hide(0f);
                }
            }
            if (almas[currentParallax].inBoat != null)
                almas[currentParallax].inBoat.Show(1f);
        }

        if (DialogManager.instance != null && currentParallax + 1 < dialogs.Count)
            DialogManager.instance.SetNewDialogData(dialogs[currentParallax + 1]);

        StartNavigate(2f);
        ChangeParallax(6f);
        await Task.Delay(6000);
        GameManagerLuiz.instance.isPaused = false;
        GameManagerLuiz.instance.inCutscene = false;
        Conductor.instance.Unpause();
        await Conductor.instance.GetComponent<AudioSource>().Fade(0f, 0.25f, 1f);
        cutsceneAudio.Fade(0.25f, 0f, 1f);
        cutsceneAudio.Stop();
        if (OnFisish[currentParallax - 1] != null)
        {
            OnFisish[currentParallax - 1]?.Invoke();
        }
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
