using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [SerializeField] private float projectileInterval = 0.2f;
    [SerializeField] private float knockback = 1f;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        
    }
}