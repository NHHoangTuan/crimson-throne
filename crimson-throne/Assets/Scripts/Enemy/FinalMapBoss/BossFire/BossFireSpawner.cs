using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossFireSpawner : MonoBehaviour
{
    #region Singleton
    public static BossFireSpawner instance { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    #region Variables
    [SerializeField] private GameObject prefab;
    [SerializeField] private float cooldown = 15f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float duration = 4f;
    private Rigidbody2D rb2d;
    private Animator animator;
    #endregion

    #region Initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public float GetCurrentDamage()
    {
        return damage;
    }

    public float GetDuration()
    {
        return duration;
    }

    public void StartAbility()
    {
        StartCoroutine(SpawnFire());
    }
    #endregion

    #region Spawn Controls    
    private IEnumerator SpawnFire()
    {
        yield return new WaitForSeconds(cooldown);
        if (PlayerController.instance == null) yield break;
        Transform playerTransform = PlayerController.instance.transform;
        while (true)
        {
            if (playerTransform == null) yield break;

            rb2d.simulated = false;
            animator.SetTrigger("attack2");
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float animationDuration = stateInfo.length;
            Vector2 playerPosition = playerTransform.position;
            yield return new WaitForSeconds(animationDuration);
            for (int i = -2; i <= 2; i++)
            {
                Vector2 spawnPosition = new Vector2(playerPosition.x + i * 4, playerPosition.y + 7);
                GameObject fireClone = Instantiate(prefab, spawnPosition, Quaternion.identity);
                Destroy(fireClone, duration);
            }
            rb2d.simulated = true;

            yield return new WaitForSeconds(cooldown);
        }
    }
    #endregion
}