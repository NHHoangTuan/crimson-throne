using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;     
    [SerializeField] private bool isFollowing = false;
    [SerializeField] public float followDistance = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform; 
            isFollowing = true;
        }
    }

    private void Update()
    {
        if (isFollowing && playerTransform != null)
        {
            Vector2 targetPosition = playerTransform.position + playerTransform.up * followDistance;
            transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
        }
    }
}
