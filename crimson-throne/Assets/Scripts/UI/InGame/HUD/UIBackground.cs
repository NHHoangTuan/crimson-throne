using UnityEngine;

public class UIBackground : MonoBehaviour
{
    public static UIBackground instance { get; private set; }
    private Animator animator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            animator = GetComponent<Animator>();
        }
    }

    public void PlayAnimation()
    {
        if (animator != null)
        {
            animator.Play("OpenHud");
        }
    }
}