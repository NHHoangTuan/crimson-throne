using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossKatana : MonoBehaviour
{
    #region Variables
    private AudioSource audioSource;
    #endregion

    #region Controls
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(AudioManager.instance.katana);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && BossKatanaSpawner.instance != null)
        {
            Destroy(gameObject);
            player.ChangeHealth(-Mathf.FloorToInt(BossKatanaSpawner.instance.GetCurrentDamage()));
        }
    }
    #endregion
}