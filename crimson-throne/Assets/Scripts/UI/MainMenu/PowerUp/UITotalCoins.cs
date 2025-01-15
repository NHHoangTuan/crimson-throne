using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITotalCoins : MonoBehaviour
{
    public static UITotalCoins instance { get; private set; }
    [SerializeField] public TMP_Text totalCoinsText;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetTotalCoinsText(int number)
    {
        totalCoinsText.GetComponent<TMP_Text>().text = number.ToString() + "$";
    }
}