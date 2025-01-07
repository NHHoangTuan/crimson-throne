using UnityEngine;
using System.IO;

public class MapsManager : MonoBehaviour
{
    public static MapsManager instance;
    public MapsData mapsData;
    private string saveFilePath;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "MapsListSaveData.json");
        LoadMapsData();
    }

    public void UnlockMap(int mapIndex)
    {
        mapsData.maps[mapIndex].isUnlocked = true;
        SaveMapsData();
    }

    public void SaveMapsData()
    {
        MapsSaveData saveData = new MapsSaveData
        {
            unlockedMaps = new bool[mapsData.maps.Length]
        };

        for (int i = 0; i < mapsData.maps.Length; i++)
        {
            saveData.unlockedMaps[i] = mapsData.maps[i].isUnlocked;
        }

        File.WriteAllText(saveFilePath, JsonUtility.ToJson(saveData));
    }

    public void LoadMapsData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            MapsSaveData saveData = JsonUtility.FromJson<MapsSaveData>(json);

            for (int i = 0; i < mapsData.maps.Length; i++)
            {
                mapsData.maps[i].isUnlocked = saveData.unlockedMaps[i];
            }
        }
    }
}