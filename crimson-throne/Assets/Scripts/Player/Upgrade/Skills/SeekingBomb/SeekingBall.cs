using UnityEngine;

public class SeekingBomb : MonoBehaviour
{
    [SerializeField] private float projectileInterval = 0.1f;
    [SerializeField] private float knockback = 1f;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        
    }
}
