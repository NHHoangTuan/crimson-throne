using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ThunderSpawner : Ability
{
    public static ThunderSpawner instance { get; private set; }
    [SerializeField] private GameObject prefab;
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
    
    public override void LevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            description = descriptions[currentLevel + 1];
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
    }

    private IEnumerator SpawnThunders()
    {
        while (true)
        {
            Vector2 playerPosition = PlayerController.instance.transform.position;

            for (int i = 0; i < projectilesCount[currentLevel]; i++)
            {
                yield return new WaitForSeconds(0.02f);
                Vector2 randomPosition = playerPosition + Random.insideUnitCircle * maxDistance;
                GameObject thunder = Instantiate(prefab, randomPosition, Quaternion.identity);
                Destroy(thunder, duration);
            }
            yield return new WaitForSeconds(cooldown * PlayerAttributeBuffs.instance.cooldownRatio);
        }
    }
}
