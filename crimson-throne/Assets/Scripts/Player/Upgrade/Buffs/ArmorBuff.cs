using UnityEngine;

public class ArmorBuff : Ability
{
    public static ArmorBuff instance { get; private set; }
    [SerializeField] private int armorBonus = 1;

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
        abilityName = "Armor Bonus";
        maxLevel = 3;
        currentLevel = 0;
        description = "Increases armor by 1";
    }

    public override void LevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
        PlayerAttributeBuffs.instance.armorBonus += armorBonus;
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