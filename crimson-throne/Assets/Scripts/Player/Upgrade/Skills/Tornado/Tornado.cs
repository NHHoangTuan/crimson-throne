using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    [SerializeField] private float knockback = 1.75f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(AudioManager.instance.tornado);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy?.TakeDamage(TornadoSpawner.instance.GetCurrentDamage() * PlayerAttributeBuffs.instance.damageRatio, knockback);
        }
    }
}