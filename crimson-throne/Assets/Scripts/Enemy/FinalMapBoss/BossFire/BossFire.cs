using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossFire : MonoBehaviour
{
    #region Variables
    private AudioSource audioSource;
    #endregion

    #region Controls
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(AudioManager.instance.bossFire);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && BossFireSpawner.instance != null)
        {
            player.ChangeHealth(-Mathf.FloorToInt(BossFireSpawner.instance.GetCurrentDamage()));
        }
    }
    #endregion
}