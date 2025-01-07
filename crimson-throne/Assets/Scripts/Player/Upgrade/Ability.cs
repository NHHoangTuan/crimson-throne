using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected string abilityName;
    protected int maxLevel;
    protected int currentLevel;
    protected string description;
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

    public abstract void LevelUp();
}