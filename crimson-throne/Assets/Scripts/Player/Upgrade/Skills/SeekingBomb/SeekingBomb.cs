using UnityEngine;

public class SeekingBomb : MonoBehaviour
{
    [SerializeField] private float knockback = 1f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(AudioManager.instance.seekingBomb);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy?.TakeDamage(SeekingBombSpawner.instance.GetCurrentDamage() * PlayerAttributeBuffs.instance.damageRatio, knockback);
        }
    }
}
