using UnityEngine;

public class NoteEffectAnimation : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("NoteEffectAnimation");
        // Destroy the effect after its duration
        Destroy(gameObject, gameObject.CompareTag("Miss") ? 0.21f : 0.11f); // Adjust the time as needed
    }
}
