using UnityEngine;

public class VelocityBuff : Ability
{
    public static VelocityBuff instance { get; private set; }
    [SerializeField] private float velocityRatio = 1.15f;

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
        abilityName = "Velocity Buff";
        maxLevel = 3;
        currentLevel = 0;
        description = "Increases skills speed by 15%";
    }

    public override void LevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
        PlayerAttributeBuffs.instance.velocityRatio *= velocityRatio;
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