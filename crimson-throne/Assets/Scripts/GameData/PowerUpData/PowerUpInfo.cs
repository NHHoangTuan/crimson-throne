using UnityEngine;

public enum PowerUpType
{
    HEALTH,
    ARMOR,
    SPEED,
    MAGNET,
    GROWTH
}

[CreateAssetMenu(fileName = "New PowerUp", menuName = "PowerUpSystem/PowerUp")]
public class PowerUpInfo : ScriptableObject
{
    public string title;
    public PowerUpType type;
    public int cost;
}