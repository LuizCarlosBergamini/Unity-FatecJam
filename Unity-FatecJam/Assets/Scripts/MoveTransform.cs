using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MoveTransform : MonoBehaviour
{
    public Vector2 visible;
    public Vector2 hidden;
    public float transitionDuration = 0.5f;

    public async void Hide(int delay = 0)
    {
        var fadeCancellation = new CancellationTokenSource();
        await Task.Delay(delay);
        await transform.MoveTransform(transform.position, hidden, transitionDuration, fadeCancellation.Token);
        gameObject.SetActive(false);
    }

    public async void Show(int delay = 0)
    {
        var fadeCancellation = new CancellationTokenSource();
        gameObject.SetActive(true);
        await Task.Delay(delay);
        await transform.MoveTransform(transform.position, visible, transitionDuration, fadeCancellation.Token);
    }
}
