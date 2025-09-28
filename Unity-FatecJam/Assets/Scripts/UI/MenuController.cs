using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;
    [SerializeField] List<GameObject> menuElements;

    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    async public void Hide(float delay)
    {
        await Task.Delay((int)(delay * 1000f));
        menuElements.ForEach(element =>
        {
            if (element.TryGetComponent(out MoveUI move_ui))
                move_ui.Hide();
            else if (element.TryGetComponent(out FadeUI fade_ui))
                fade_ui.Hide();
            else if (element.TryGetComponent(out MoveTransform move))
                move.Hide();
            else if (element.TryGetComponent(out FadeSprite fade))
                fade.Hide();
        });
    }

    async public void Show(float delay)
    {
        await Task.Delay((int)(delay * 1000f));
        menuElements.ForEach(element =>
        {
            if (element.TryGetComponent(out MoveUI move_ui))
                move_ui.Show();
            else if (element.TryGetComponent(out FadeUI fade_ui))
                fade_ui.Show();
            else if (element.TryGetComponent(out MoveTransform move))
                move.Show();
            else if (element.TryGetComponent(out FadeSprite fade))
                fade.Show();
        });
    }
}
