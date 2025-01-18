using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    #region Variables
    [SerializeField] private int healthValue = 20;
    #endregion

    #region Controls
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (playerController != null)
        {
            if (!playerController.IsFullHealth())
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.healthCollect);
                playerController.ChangeHealth(healthValue);
                SpawnItemManager.instance?.OnHealthItemCollected();
                Destroy(gameObject);
            }
        }
    }
    
    public void SetHealthValue(int value)
    {
        healthValue = value;
    }
    #endregion
}