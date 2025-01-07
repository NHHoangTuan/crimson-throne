using UnityEngine;

public class MagnetBuff : Ability
{
    public static MagnetBuff instance { get; private set; }
    [SerializeField] private float magnetRatio = 1.15f;

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
        abilityName = "Magnet Buff";
        maxLevel = 3;
        currentLevel = 0;
        description = "Increases magnet range by 15%";
    }
    
    public override void LevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
        PlayerAttributeBuffs.instance.magnetRatio *= magnetRatio;
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