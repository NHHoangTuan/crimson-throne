using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    [SerializeField] private int healthValue = 20;

     private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (playerController != null)
        {
            if (!playerController.IsFullHealth())
            {
                playerController.ChangeHealth(healthValue);
                Destroy(gameObject);
            }
        }
    }
    
    public void SetHealthValue(int value)
    {
        healthValue = value;
    }
}
