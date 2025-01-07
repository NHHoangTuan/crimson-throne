using UnityEngine;

public class ExpItem : MonoBehaviour
{
    [SerializeField] private float expValue = 2f;
    [SerializeField] private float attractSpeed = 6f;
    [SerializeField] private bool isAdvanced = false;
    [SerializeField] private bool isAttracting = false;
    private Rigidbody2D rb2d;
    private Rigidbody2D characterRb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        characterRb2d = PlayerController.instance.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isAttracting)
        {
            AttractToTarget();
        }
    }

    private void AttractToTarget()
    {
        Vector2 direction = (characterRb2d.position - rb2d.position).normalized;
        rb2d.linearVelocity = direction * attractSpeed;
        if (Vector2.Distance(rb2d.position, characterRb2d.position) < 0.2f)
        {
            Collect();
        }
    }

    private void Collect()
    {   
        if (isAdvanced)
        {
            expValue = PlayerController.instance.GetRequiredExpToLevelUp();
        }
        PlayerController.instance.AddExperience(expValue);
        Destroy(gameObject);
    }

    public void StartAttracting()
    {
        if (!isAttracting)
        {
            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;
            isAttracting = true;
        }
    }

    public void SetExpValue(float value)
    {
        expValue = value;
    }
}
