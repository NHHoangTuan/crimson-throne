using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffsManager : MonoBehaviour
{
    #region Singleton
    public static BuffsManager instance { get; private set; }    
    
    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Variables
    private const int MAX_BUFFS = 4;
    public List<Ability> activeBuffs;
    public List<Ability> inactiveBuffs;
    public List<Ability> maxBuffs;
    #endregion
    
    #region Controls
    public int GetUpgradableCount()
    {
        return MAX_BUFFS - maxBuffs.Count;
    }

    public void MoveToActiveBuffs(Ability ability)
    {
        activeBuffs.Add(ability);
        inactiveBuffs.Remove(ability);
        UIAbilities.instance.AddBuffIcon(ability.GetArtwork(), activeBuffs.Count - 1);
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

    public void DestroyCompletely()
    {
        if (instance == this)
        {
            instance = null;
            Destroy(gameObject);
        }
    }
    #endregion
}