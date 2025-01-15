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
    public float cooldownRatio = 1;
    public float velocityRatio = 1;
    public float durationRatio = 1;
    
    public static PlayerAttributeBuffs instance { get; private set; }

    void Awake() 
    {
        if (instance == null) 
        {
            instance = this;
        }
    }
}
