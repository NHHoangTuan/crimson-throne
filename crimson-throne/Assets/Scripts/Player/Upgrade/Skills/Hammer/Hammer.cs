using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private float knockback = 2f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(AudioManager.instance.hammer);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy?.TakeDamage(HammerSpawner.instance.GetCurrentDamage() * PlayerAttributeBuffs.instance.damageRatio, knockback);
        }
    }
}
