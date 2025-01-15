using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{

    private const int MAX_SKILLS = 4;
    public static SkillsManager instance { get; private set; }
    public List<Ability> activeSkills;
    public List<Ability> inactiveSkills;
    public List<Ability> maxSkills;
    
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

    public int GetUpgradableCount()
    {
        return MAX_SKILLS - maxSkills.Count;
    }
    
    public void MoveToActiveSkills(Ability ability)
    {
        activeSkills.Add(ability);
        inactiveSkills.Remove(ability);
        UIAbilities.instance.AddSkillIcon(ability.GetArtwork(), activeSkills.Count - 1);
    }

    public void MoveToMaxSkills(Ability ability)
    {
        activeSkills.Remove(ability);
        maxSkills.Add(ability);
    }

    public bool IsFull()
    {
        return (activeSkills.Count + maxSkills.Count  == MAX_SKILLS);
    }

    public void DestroyCompletely()
    {
        if (instance == this)
        {
            instance = null;
            Destroy(gameObject);
        }
    }
}