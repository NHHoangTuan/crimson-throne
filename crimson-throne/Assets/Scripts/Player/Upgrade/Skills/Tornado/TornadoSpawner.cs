using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TornadoSpawner : Ability
{
    public static TornadoSpawner instance { get; private set; }
    [SerializeField] private GameObject prefab;
    [SerializeField] private float duration = 2.25f;
    [SerializeField] private float velocity = 7f;
    [SerializeField] private string[] descriptions = {
        "",
        "Passes through enemies.",
        "Base Damage up by 5. Cooldown down by 0.3 seconds.",
        "Fires 1 more projectile.",
        "Base Damage up by 5. Cooldown down by 0.3 seconds.",
        "Fires 1 more projectile.",
        ""
    };
    [SerializeField] private int[] projectilesCount = {0,1,1,2,2,3};
    [SerializeField] private float[] cooldown = {0f,3f,2.7f,2.7f,2.4f,2.4f};
    [SerializeField] private float[] damage = {0f,8f,13f,13f,18f,18f};

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
        abilityName = "Tornado Skill";
        maxLevel = 5;
        currentLevel = 0;
        description = descriptions[currentLevel + 1];
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
            StartCoroutine(SpawnTornados());
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

    private IEnumerator SpawnTornados()
    {
        while (true)
        {
            for (int i = 0; i < projectilesCount[currentLevel]; i++)
            {
                Vector2 spawnPosition = (Vector2)PlayerController.instance.transform.position;
                GameObject tornado = Instantiate(prefab, spawnPosition, Quaternion.identity);
                Rigidbody2D rb2d = tornado.GetComponent<Rigidbody2D>();
                if (rb2d != null)
                {
                    Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                    rb2d.linearVelocity = randomDirection * velocity * PlayerAttributeBuffs.instance.velocityRatio;
                }
                Destroy(tornado, duration * PlayerAttributeBuffs.instance.durationRatio);
            }
            yield return new WaitForSeconds(cooldown[currentLevel] * PlayerAttributeBuffs.instance.cooldownRatio); 
        }
    }
}
