using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUIController : MonoBehaviour
{
    public static PowerUpUIController instance { get; private set; }
    [SerializeField] private List<PowerUpInfo> options;
    [SerializeField] private GameObject optionDisplayPrefab;
    [SerializeField] private Transform layoutGroup;
    private List<GameObject> activeOptions = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetUp()
    {
        foreach (var option in options)
        {
            GameObject clone = Instantiate(optionDisplayPrefab, layoutGroup);
            activeOptions.Add(clone);

            UIPowerUpDisplay powerUpDisplay = clone.GetComponent<UIPowerUpDisplay>();
            if (powerUpDisplay == null) continue;

            powerUpDisplay.UpdatePowerUp(option, PlayerPrefs.GetInt("PowerUp" + option.type, 0));
        }
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        UITotalCoins.instance.SetTotalCoinsText(totalCoins);
    }

    public bool Up(PowerUpInfo info)
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        if (totalCoins - info.cost < 0)
        {
            return false;
        }

        totalCoins -= info.cost;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.SetInt("PowerUp" + info.type, PlayerPrefs.GetInt("PowerUp" + info.type, 0) + 1);
        PlayerPrefs.Save();
        UITotalCoins.instance.SetTotalCoinsText(totalCoins);
        return true;
    }

    public void Reset()
    {
        if (activeOptions == null) return;

        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        foreach (var option in options)
        {
            totalCoins += PlayerPrefs.GetInt("PowerUp" + option.type, 0) * option.cost;
            PlayerPrefs.SetInt("PowerUp" + option.type, 0);
        }
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();
        UITotalCoins.instance.SetTotalCoinsText(totalCoins);
        
        foreach (var activeOption in activeOptions)
        {
            UIPowerUpDisplay display = activeOption.GetComponent<UIPowerUpDisplay>();
            display.Reset();
        }
    }

    public void Close()
    {
        UIManager.instance.powerUpPanel.SetActive(false);
        UIManager.instance.mainPanel.SetActive(true);
        Clear();
    }

    private void Clear()
    {
        foreach (var option in activeOptions)
        {
            Destroy(option);
        }
        activeOptions.Clear();
    }
}