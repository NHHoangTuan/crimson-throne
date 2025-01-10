using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KatanaSpawner : Ability
{
    public static KatanaSpawner instance { get; private set; }
    [SerializeField] private GameObject prefab;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float velocity = 8f;
    [SerializeField] private float cooldown = 1.5f;
    [SerializeField] private string[] descriptions = {
        "",
        "Fires quickly in the faced direction.",
        "Base Damage up by 2.",
        "Passes through 1 more enemy.",
        "Base Damage up by 3.",
        "Passes through 1 more enemy.",
        "",
    };
    [SerializeField] private int[] pierce = {0,1,1,2,2,3};
    [SerializeField] private float[] damage = {0,4f,6f,6f,9f,9f};

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
        abilityName = "Katana Skill";
        maxLevel = 5;
        currentLevel = 0;
        description = descriptions[currentLevel + 1];
    }

    public int GetCurrentPierce()
    {
        return pierce[currentLevel];
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
            StartCoroutine(SpawnKatanas());
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

    private IEnumerator SpawnKatanas()
    {
        while (true)
        {
            Vector2 lookDirection = PlayerController.instance.lookDirection;
            Rigidbody2D characterRb2d = PlayerController.instance.GetComponent<Rigidbody2D>();
            GameObject katana = Instantiate(prefab, (Vector2)characterRb2d.position + lookDirection * 0.2f, Quaternion.identity); 
            Rigidbody2D rb2d = katana.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                float roundedAngle = Mathf.Round(angle / 45f) * 45f;
                roundedAngle -= 90f;
                katana.transform.rotation = Quaternion.Euler(0, 0, roundedAngle);
                rb2d.AddForce(lookDirection * velocity * PlayerAttributeBuffs.instance.velocityRatio, ForceMode2D.Impulse);
            }
            Destroy(katana, duration * PlayerAttributeBuffs.instance.durationRatio);
            yield return new WaitForSeconds(cooldown * PlayerAttributeBuffs.instance.cooldownRatio);
        }
    }
}