using UnityEngine;

public class DamageBuff : Ability
{
    public static DamageBuff instance { get; private set; }
    [SerializeField] private float damageRatio = 1.1f;

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
        abilityName = "Damage Buff";
        maxLevel = 3;
        currentLevel = 0;
        description = "Increases damage by 10%";
    }

    public override void LevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
        PlayerAttributeBuffs.instance.damageRatio *= damageRatio;
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