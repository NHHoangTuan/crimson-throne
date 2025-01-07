using UnityEngine;
using UnityEngine.UI;

public class UICollectItemCount : MonoBehaviour
{
    public static UICollectItemCount instance { get; private set; }
    [SerializeField] private GameObject killsCount;
    [SerializeField] private GameObject coinsCount;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetKillsCount(int count)
    {
        killsCount.GetComponent<Text>().text = count.ToString();
    }
    
    public void SetCoinsCount(int count)
    {
        coinsCount.GetComponent<Text>().text = count.ToString();
    }
}

