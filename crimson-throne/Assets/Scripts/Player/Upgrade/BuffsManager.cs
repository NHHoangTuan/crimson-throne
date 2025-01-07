using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffsManager : MonoBehaviour
{

    private const int MAX_BUFFS = 4;
    public static BuffsManager instance { get; private set; }    
    public List<Ability> activeBuffs;
    public List<Ability> inactiveBuffs;
    public List<Ability> maxBuffs;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public int GetUpgradableCount()
    {
        return MAX_BUFFS - maxBuffs.Count;
    }

    public void MoveToActiveBuffs(Ability ability)
    {
        activeBuffs.Add(ability);
        inactiveBuffs.Remove(ability);
        UIAbilityIcon.instance.AddBuffIcon(ability.GetArtwork(), activeBuffs.Count - 1);
    }

    public void MoveToMaxBuffs(Ability ability)
    {
        activeBuffs.Remove(ability);
        maxBuffs.Add(ability);
    }

    public bool IsFull()
    {
        return (activeBuffs.Count + maxBuffs.Count == MAX_BUFFS);
    }
}

