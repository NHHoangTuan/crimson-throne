// using UnityEngine;

// public class SaveManager : MonoBehaviour
// {
//     public static void SaveData(string key, object data)
//     {
//         // Serialize data and save to PlayerPrefs or file
//         string jsonData = JsonUtility.ToJson(data);
//         PlayerPrefs.SetString(key, jsonData);
//         PlayerPrefs.Save();
//     }

//     public static T LoadData<T>(string key)
//     {
//         // Load data and deserialize it
//         if (PlayerPrefs.HasKey(key))
//         {
//             string jsonData = PlayerPrefs.GetString(key);
//             return JsonUtility.FromJson<T>(jsonData);
//         }
//         return default;
//     }
// }
