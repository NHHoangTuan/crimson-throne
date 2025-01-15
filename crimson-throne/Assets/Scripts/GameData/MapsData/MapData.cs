using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Map/MapData")]
public class MapData : ScriptableObject
{
    public string title;
    public Sprite image;
    public bool isUnlocked;
}
