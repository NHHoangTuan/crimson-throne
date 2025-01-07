using UnityEngine;

public class GrowthBuff : Ability
{
    public static GrowthBuff instance { get; private set; }
    [SerializeField] private float growthRatio = 1.15f;

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
        abilityName = "Growth Buff";
        maxLevel = 3;
        currentLevel = 0;
        description = "Increases experience received by 15%";
    }

    public override void LevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
        PlayerAttributeBuffs.instance.growthRatio *= growthRatio;
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