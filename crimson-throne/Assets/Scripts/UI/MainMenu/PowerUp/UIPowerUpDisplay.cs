using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPowerUpDisplay : MonoBehaviour 
{
    [SerializeField] public TMP_Text titleText;
    [SerializeField] public TMP_Text numberText;
    [SerializeField] public TMP_Text costText;
    [SerializeField] public Button addButton;

    public void UpdatePowerUp(PowerUpInfo powerUp, int number)
    {
        if (powerUp == null) return;
        titleText.text = powerUp.title.ToString();
        numberText.text = number.ToString();
        costText.text = powerUp.cost.ToString() + "$";
        addButton.onClick.AddListener(() => 
        {
            if (PowerUpUIController.instance.Up(powerUp))
            {   
                int currentNumber = int.Parse(numberText.text);
                ++currentNumber;
                numberText.text = currentNumber.ToString();
            }
        });
    }
    
    public void Reset()
    {
        numberText.text = "0";
    }
}