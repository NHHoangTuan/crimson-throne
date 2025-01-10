using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ThunderSpawner : Ability
{
    public static ThunderSpawner instance { get; private set; }
    [SerializeField] private GameObject prefab;
    [SerializeField] private LayerMask enemyLayer; 
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float duration = 0.7f;
    [SerializeField] private float cooldown = 4f; 
    [SerializeField] private string[] descriptions = {
        "",
        "Strikes at random areas.",
        "Fires 1 more projectile.",
        "Base Damage up by 10.",
        "Fires 1 more projectile.",
        "Base Damage up by 20.",
        "",
    };
    [SerializeField] private int[] projectilesCount = {0,2,3,3,4,4};
    [SerializeField] private float[] damage = {0,15f,15f,25f,25f,55f};

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
        abilityName = "Thunders Skill";
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
            StartCoroutine(SpawnThunders());
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

    private IEnumerator SpawnThunders()
    {
        while (true)
        {
            Vector2 playerPosition = PlayerController.instance.transform.position;
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(playerPosition, maxDistance, enemyLayer);
            int thunderCounts = enemiesInRange.Length;
            if (thunderCounts > 0)
            {
                System.Array.Sort(enemiesInRange, (a, b) =>
                    Vector2.Distance(playerPosition, a.transform.position)
                    .CompareTo(Vector2.Distance(playerPosition, b.transform.position)));

                int count = Mathf.Min(projectilesCount[currentLevel], enemiesInRange.Length);
                thunderCounts = count;
                for (int i = 0; i < count; i++)
                {
                    Vector2 targetPosition = enemiesInRange[i].transform.position;
                    SpawnThunder(targetPosition);
                    yield return new WaitForSeconds(0.02f);
                }
            }
            for (int i = 0; i < projectilesCount[currentLevel] - thunderCounts; i++)
            {
                Vector2 randomPosition = playerPosition + Random.insideUnitCircle * maxDistance;
                SpawnThunder(randomPosition);
                yield return new WaitForSeconds(0.02f);
            }
            yield return new WaitForSeconds(cooldown * PlayerAttributeBuffs.instance.cooldownRatio);
        }
    }

    private void SpawnThunder(Vector2 position)
    {
        GameObject thunder = Instantiate(prefab, new Vector2(position.x, position.y + 2.94f), Quaternion.identity);
        Destroy(thunder, duration);
    }
}
