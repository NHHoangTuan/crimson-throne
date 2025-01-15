using UnityEngine;

public class SpeedBuff : Ability
{
    public static SpeedBuff instance { get; private set; }
    [SerializeField] private float speedRatio = 1.1f;

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
        abilityName = "Speed Buff";
        maxLevel = 3;
        currentLevel = 0;
        description = "Increases move speed by 10%";
    }

    public override void LevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
        PlayerAttributeBuffs.instance.speedRatio *= speedRatio;
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