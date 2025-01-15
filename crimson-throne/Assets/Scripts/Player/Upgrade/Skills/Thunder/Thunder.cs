using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(AudioManager.instance.thunder);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy?.TakeDamage(ThunderSpawner.instance.GetCurrentDamage() * PlayerAttributeBuffs.instance.damageRatio, 0.1f);
        }
    }
}