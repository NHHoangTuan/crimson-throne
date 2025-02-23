using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Katana : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private float knockback = 0.5f;
    [SerializeField] private int pierce = 2;

    private void Start()
    {
        pierce = KatanaSpawner.instance.GetCurrentPierce();
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(AudioManager.instance.katana);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            pierce -= 1;
            if (pierce == 0) 
            {
                Destroy(gameObject);
            }
            enemy?.TakeDamage(KatanaSpawner.instance.GetCurrentDamage() * PlayerAttributeBuffs.instance.damageRatio, knockback);
        }
    }
}