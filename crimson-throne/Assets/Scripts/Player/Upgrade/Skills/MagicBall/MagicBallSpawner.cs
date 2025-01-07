using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MagicBallSpawner : Ability
{
    public static MagicBallSpawner instance { get; private set; }
    [SerializeField] private GameObject prefab; 
    [SerializeField] private float cooldown = 3f;
    [SerializeField] private float rotationSpeed = 400f;
    [SerializeField] private float radius = 1.7f; 
    [SerializeField] private string[] descriptions = {
        "",
        "Orbits around the character.",
        "Fires 1 more projectile.",
        "Effect lasts 0.5 seconds longer. Base Damage up by 6.",
        "Fires 1 more projectile.",
        "Effect lasts 0.5 seconds longer. Base Damage up by 6.",
        ""
    };
    [SerializeField] private int[] projectilesCount = {0,1,2,2,3,3};
    [SerializeField] private float[] duration = {0,3f,3f,3.5f,3.5f,4f};
    [SerializeField] private float[] damage = {0f,6f,6f,12f,12f,18f};

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
        abilityName = "Magic Ball Skill";
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
            StartCoroutine(SpawnMagicBalls());
            SkillsManager.instance.MoveToActiveSkills(this);
        }
        if (currentLevel == maxLevel)
        {
            SkillsManager.instance.MoveToMaxSkills(this);
        }
    }

    private IEnumerator SpawnMagicBalls()
    {
        while (true)
        {
            GameObject[] magicBalls = new GameObject[projectilesCount[currentLevel]];
            Vector2 playerPosition = PlayerController.instance.transform.position;
            for (int i = 0; i < projectilesCount[currentLevel]; i++)
            {
                float angle = i * Mathf.PI * 2f / projectilesCount[currentLevel];
                Vector2 spawnPosition = playerPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                magicBalls[i] = Instantiate(prefab, spawnPosition, Quaternion.identity);
            }
            float elapsedTime = 0f;
            while (elapsedTime < duration[currentLevel] * PlayerAttributeBuffs.instance.durationRatio)
            {
                elapsedTime += Time.deltaTime;
                playerPosition = PlayerController.instance.transform.position;
                for (int i = 0; i < projectilesCount[currentLevel]; i++)
                {
                    if (magicBalls[i] != null)
                    {
                        float angle = (i * Mathf.PI * 2f / projectilesCount[currentLevel]) + (elapsedTime * rotationSpeed * PlayerAttributeBuffs.instance.velocityRatio * Mathf.Deg2Rad / (duration[currentLevel]  * PlayerAttributeBuffs.instance.durationRatio));
                        Vector2 updatedPosition = playerPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                        magicBalls[i].transform.position = updatedPosition;
                    }
                }
                yield return null;
            }
            for (int i = 0; i < projectilesCount[currentLevel]; i++)
            {
                if (magicBalls[i] != null)
                {
                    Destroy(magicBalls[i]);
                }
            }
            yield return new WaitForSeconds(cooldown * PlayerAttributeBuffs.instance.cooldownRatio);
        }
    }
}
