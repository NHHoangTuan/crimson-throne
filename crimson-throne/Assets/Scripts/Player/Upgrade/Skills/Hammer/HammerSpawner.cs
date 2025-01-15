using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HammerSpawner : Ability
{
    public static HammerSpawner instance { get; private set; }
    [SerializeField] private GameObject prefab;
    [SerializeField] private float duration = 2f;
    [SerializeField] private float velocity = 6.5f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float maxAngleOffset = 45f;
    [SerializeField] private float cooldown = 4f;
    [SerializeField] private string[] descriptions = {
        "",
        "Summon 1 hammer with high damage.",
        "Fires 1 more projectile.",
        "Base Damage up by 10.",
        "Fires 1 more projectile.",
        "Base Damage up by 10.",
        ""
    };
    [SerializeField] private float[] damage = {0f,10f,10f,20f,20f,30f};
    [SerializeField] private int[] projectilesCount = {0,1,2,2,3,3};

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
        abilityName = "Hammer Skill";
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
            StartCoroutine(SpawnHammers());
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

    private IEnumerator SpawnHammers()
    {
        while (true)
        {
            float initialRandomAngle = Random.Range(-maxAngleOffset, maxAngleOffset);
            for (int i = 0; i < projectilesCount[currentLevel]; i++)
            {
                Rigidbody2D characterRb2d = PlayerController.instance.GetComponent<Rigidbody2D>();
                GameObject hammer = Instantiate(prefab, new Vector2(characterRb2d.position.x, characterRb2d.position.y + 0.8f), Quaternion.identity);
                Rigidbody2D rb2d = hammer.GetComponent<Rigidbody2D>();
                if (rb2d != null)
                {
                    float randomAngle = initialRandomAngle + (i * maxAngleOffset);
                    randomAngle = ((randomAngle + maxAngleOffset) % (maxAngleOffset * 2)) - maxAngleOffset;
                    Vector2 launchDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
                    rb2d.AddForce(launchDirection * velocity * PlayerAttributeBuffs.instance.velocityRatio, ForceMode2D.Impulse);
                    rb2d.AddTorque(rotationSpeed);
                }
                Destroy(hammer, duration * PlayerAttributeBuffs.instance.durationRatio);
            }
            yield return new WaitForSeconds(cooldown * PlayerAttributeBuffs.instance.cooldownRatio);
        }
    }
}