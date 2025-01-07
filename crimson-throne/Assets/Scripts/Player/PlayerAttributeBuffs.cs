using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributeBuffs : MonoBehaviour
{
    // BUFF
    public int maxHealthBonus = 0;
    public int armorBonus = 0;
    public float speedRatio = 1;
    public float magnetRatio = 1;
    public float growthRatio = 1;

    // SKILL
    public float damageRatio = 1;
    public int amountBonus = 0;
    public float cooldownRatio = 1;
    public float velocityRatio = 1;
    public float durationRatio = 1;
    public float areaRatio = 1;
    
    public static PlayerAttributeBuffs instance { get; private set; }

    void Awake() 
    {
        instance = this;
    }

    public void Reset() 
    {
        maxHealthBonus = 0;
        armorBonus = 0;
        amountBonus = 0;
        speedRatio = 1;
        magnetRatio = 1;
        damageRatio = 1;
        cooldownRatio = 1;
        velocityRatio = 1;
        durationRatio = 1;
        areaRatio = 1;
    }
}
