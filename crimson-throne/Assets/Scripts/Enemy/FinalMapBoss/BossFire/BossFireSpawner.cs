using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossFireSpawner : MonoBehaviour
{
    public static BossFireSpawner instance { get; private set; }
    private Rigidbody2D rb2d;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float cooldown = 15f;
    [SerializeField] private float damage = 15f;
    private Animator animator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public float GetCurrentDamage()
    {
        return damage;
    }

    public void StartAbility()
    {
        StartCoroutine(SpawnFire());
    }
    
    private IEnumerator SpawnFire()
    {
        yield return new WaitForSeconds(cooldown);
        while (true)
        {
            if (PlayerController.instance == null) yield break;

            rb2d.simulated = false;
            animator.SetTrigger("attack2");
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float animationDuration = stateInfo.length;
            Vector2 playerPosition = PlayerController.instance.transform.position;
            yield return new WaitForSeconds(animationDuration);
            for (int i = -3; i <= 3; i++)
            {
                Vector2 spawnPosition = new Vector2(playerPosition.x + i * 3, playerPosition.y + 8);
                GameObject fireClone = Instantiate(prefab, spawnPosition, Quaternion.identity);
                Destroy(fireClone, 4f);
            }
            rb2d.simulated = true;

            yield return new WaitForSeconds(cooldown);
        }
    }
}