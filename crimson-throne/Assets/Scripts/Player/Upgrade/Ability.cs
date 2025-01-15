using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected int maxLevel;
    [SerializeField] protected string abilityName;
    [SerializeField] protected string description;
    [SerializeField] protected int currentLevel;
    [SerializeField] protected Sprite artwork;

    public string GetAbilityName()
    {
        return abilityName;
    }

    public int GetNextLevel()
    {
        if (currentLevel == maxLevel)
        {
            return currentLevel;
        }
        return currentLevel + 1;
    }

    public string GetDescription()
    {
        return description;
    }

	public Sprite GetArtwork()
    {
        return artwork;
    }

	public int GetMaxLevel()
    {
        return maxLevel;
    }

    public abstract void LevelUp();
}