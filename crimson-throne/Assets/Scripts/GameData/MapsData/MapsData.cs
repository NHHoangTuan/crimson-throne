using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Map/MapsData")]
public class MapsData : ScriptableObject
{
    public MapData[] maps;
}
