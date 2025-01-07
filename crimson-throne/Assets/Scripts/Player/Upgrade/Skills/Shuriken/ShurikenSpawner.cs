using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShurikenSpawner : Ability
{
    public static ShurikenSpawner instance { get; private set; }
    [SerializeField] private GameObject prefab;
    [SerializeField] private float startkDistance = 0.5f;
    [SerializeField] private float attackDistance = 2.5f;
    [SerializeField] private float velocity = 5f;  
    [SerializeField] private string[] descriptions = {
        "",
        "Summon 4 shurikens, pass through enemies.",
        "Base Damage up by 5.",
        "Reduce Cooldown by 0.2s.",
        "Base Damage up by 5.",
        "Reduce Cooldown by 0.3s.",
        ""
    };
    [SerializeField] private float[] cooldown = {0f,2f,2f,1.8f,1.8f,1.5f}; 
    [SerializeField] private float[] damage = {0f,5f,10f,10f,15f,15f};

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
        abilityName = "Shuriken Skill";
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
            StartCoroutine(SpawnShurikens());
            SkillsManager.instance.MoveToActiveSkills(this);
        }
        if (currentLevel == maxLevel)
        {
            SkillsManager.instance.MoveToMaxSkills(this);
        }
    }

    private IEnumerator SpawnShurikens()
    {
        while (true)
        {
            StartCoroutine(AttackWithShurikens());
        }
    }

    private IEnumerator AttackWithShurikens()
    {
        Vector2 playerPosition = PlayerController.instance.transform.position;
        SpawnShuriken(playerPosition + Vector2.up * attackDistance, Vector2.up);   
        SpawnShuriken(playerPosition + Vector2.down * attackDistance, Vector2.down); 
        SpawnShuriken(playerPosition + Vector2.left * attackDistance, Vector2.left); 
        SpawnShuriken(playerPosition + Vector2.right * attackDistance, Vector2.right); 
        yield return new WaitForSeconds(cooldown[currentLevel] * PlayerAttributeBuffs.instance.cooldownRatio);
    }

    private void SpawnShuriken(Vector2 spawnPosition, Vector2 direction)
    {
        GameObject shuriken = Instantiate(prefab, spawnPosition, Quaternion.identity);
        StartCoroutine(MoveShuriken(shuriken, direction));
    }

    private IEnumerator MoveShuriken(GameObject shuriken, Vector2 direction)
    {
        float distanceTravelled = startkDistance;
        float totalDistance = attackDistance;

        while (distanceTravelled < totalDistance)
        {
            shuriken.transform.position += (Vector3)direction * velocity * PlayerAttributeBuffs.instance.velocityRatio * Time.deltaTime; 
            distanceTravelled += velocity * PlayerAttributeBuffs.instance.velocityRatio * Time.deltaTime;
            yield return null;
        }
        Destroy(shuriken);
    }
}