using UnityEngine;

public class DurationBuff : Ability
{
    public static DurationBuff instance { get; private set; }
    [SerializeField] private float durationRatio = 1.1f;

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
        abilityName = "Duration Buff";
        maxLevel = 3;
        currentLevel = 0;
        description = "Increases duration by 10%";
    }

    public override void LevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
        PlayerAttributeBuffs.instance.durationRatio *= durationRatio;
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