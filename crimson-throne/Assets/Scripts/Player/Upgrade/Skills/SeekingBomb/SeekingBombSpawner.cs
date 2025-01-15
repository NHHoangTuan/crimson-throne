using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SeekingBombSpawner : Ability
{
    public static SeekingBombSpawner instance { get; private set; }
    [SerializeField] private GameObject prefab; 
    [SerializeField] private LayerMask enemyLayer; 
    [SerializeField] private float duration = 2f;
    [SerializeField] private float velocity = 7f;
    [SerializeField] private float searchRadius = 100f;
    [SerializeField] private string[] descriptions = {
        "",
        "Fires at the nearest enemy.",
        "Fires 1 more projectile.",
        "Cooldown reduced by 0.2 seconds.",
        "Base Damage up by 10.",
        "Fires 1 more projectile.",
        ""
    };
    [SerializeField] private int[] projectilesCount = {0,1,2,2,2,3};
    [SerializeField] private float[] cooldown = {0,1.7f,1.7f,1.5f,1.5f,1.5f};
    [SerializeField] private float[] damage = {0f,5f,5f,5f,15f,15f};

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Initialize();
        }
    }

    private void Initialize() 
    {
        abilityName = "Seeking Bomb Skill";
        maxLevel = 5;
        currentLevel = 0;
        description = descriptions[currentLevel + 1];
        Image imageComponent = prefab.GetComponentInChildren<Image>();
    }
    
    public float GetCurrentDamage()
    {
        return damage[currentLevel];
    }

    public override void LevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
        if (currentLevel == 1)
        {
            StartCoroutine(SpawnSeekingBombs());
            SkillsManager.instance.MoveToActiveSkills(this);
        }
        if (currentLevel == maxLevel)
        {
            SkillsManager.instance.MoveToMaxSkills(this);
        }
        else 
        {
            description = descriptions[currentLevel + 1];
        }
    }

    private IEnumerator SpawnSeekingBombs()
    {
        while (true)
        {
            for (int i = 0; i < projectilesCount[currentLevel]; i++) 
            {
                yield return new WaitForSeconds(0.2f);
                Vector2 playerPosition = PlayerController.instance.transform.position;
                Vector2 targetPosition = FindNearestEnemy(playerPosition);
                SpawnSeekingBall(playerPosition, targetPosition);
            }
            yield return new WaitForSeconds(cooldown[currentLevel] * PlayerAttributeBuffs.instance.cooldownRatio);
        }
    }

    private Vector2 FindNearestEnemy(Vector2 playerPosition)
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(playerPosition, searchRadius, enemyLayer);
        Vector2 nearestEnemy = playerPosition;
        if (enemiesInRange.Length > 0)
        {
            float nearestDistance = float.MaxValue;
            foreach (Collider2D enemy in enemiesInRange)
            {
                float distanceToEnemy = Vector2.Distance(playerPosition, enemy.transform.position);
                if (distanceToEnemy < nearestDistance)
                {
                    nearestDistance = distanceToEnemy;
                    nearestEnemy = enemy.transform.position;
                }
            }
        }
        else
        {
            nearestEnemy = playerPosition + new Vector2(Random.Range(-searchRadius, searchRadius), Random.Range(-searchRadius, searchRadius));
        }
        return nearestEnemy;
    }

    private void SpawnSeekingBall(Vector2 spawnPosition, Vector2 targetPosition)
    {
        GameObject seekingBall = Instantiate(prefab, spawnPosition, Quaternion.identity);
        Vector2 direction = (targetPosition - spawnPosition).normalized;
        Rigidbody2D rb = seekingBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float forceMagnitude = velocity * PlayerAttributeBuffs.instance.velocityRatio;
            rb.AddForce(direction * forceMagnitude, ForceMode2D.Impulse);
        }
        Destroy(seekingBall, duration * PlayerAttributeBuffs.instance.durationRatio);
    }
}