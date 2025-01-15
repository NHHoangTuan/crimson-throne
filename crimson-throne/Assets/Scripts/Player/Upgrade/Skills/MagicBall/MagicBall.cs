using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    [SerializeField] private float knockback = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy?.TakeDamage(MagicBallSpawner.instance.GetCurrentDamage() * PlayerAttributeBuffs.instance.damageRatio, knockback);
        }
    }
}