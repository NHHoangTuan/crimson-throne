using UnityEngine;

public class CooldownBuff : Ability
{
    public static CooldownBuff instance { get; private set; }
    [SerializeField] private float cooldownRatio = 0.9f;

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
        abilityName = "Cooldown Buff";
        maxLevel = 3;
        currentLevel = 0;
        description = "Decrease cooldown by 10%";
    }

    public override void LevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
        PlayerAttributeBuffs.instance.cooldownRatio *= cooldownRatio;
        if (currentLevel == 1)
        {
            BuffsManager.instance.MoveToActiveBuffs(this);
        }
        if (currentLevel == maxLevel)
        {
            BuffsManager.instance.MoveToMaxBuffs(this);
        }
    }
}