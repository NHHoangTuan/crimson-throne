using UnityEngine;

public class UIBackground : MonoBehaviour
{
    public static UIBackground instance { get; private set; }
    [SerializeField] private GameObject loadingScene;
    [SerializeField] private Animator animator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public bool IsAnimationDone()
    {
        if (animator != null)
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !animator.IsInTransition(0);
        }
        return true;
    }

    public void Show()
    {
        loadingScene.SetActive(true);
        animator = loadingScene.GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        if (animator != null)
        {
            animator.Play("StartLoading");
        }
    }

    public void Hide()
    {
        loadingScene.SetActive(false);
    }
}