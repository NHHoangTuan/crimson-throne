using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    // ATTRIBUTES
    [Header("PLAYER ATTRIBUTES")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth { get; set; } = 100;
    [SerializeField] private int armor = 1;
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float growth = 1f;
    [SerializeField] private float magnet = 1.5f;

    // MOVE
    private PlayerInputActions inputActions;
    private Animator animator;
    private Rigidbody2D rb2d;
    private Vector2 currentMoveInput = Vector2.zero;
    private float lookXDirection = 1;
    public Vector2 lookDirection = new Vector2(1, 0);

    // TAKE DAMAGE
    [SerializeField] private GameObject deathEffect;
    private float timeInvincible = 0.5f;
    private bool isInvicible;
    private float invicibleTimer;
    private bool isDefeated = false;

    // AUDIO
    private AudioSource audioSource;

    // LEVEL
    [Space(10)]
    [Header("PLAYER LEVELS")]
    [SerializeField] private int maxLevel = 31;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private float currentExperience = 0;
    [SerializeField] public string defaultSkill = "Katana Skill";
    private float timeAdding = 0.5f;
    private float addingTimer = 0.5f;

    // INSTANCE
    public static PlayerController instance{get; private set;}

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        inputActions = new PlayerInputActions();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        int healthPowerUp = PlayerPrefs.GetInt("PowerUp" + PowerUpType.HEALTH);
        int armorPowerUp = PlayerPrefs.GetInt("PowerUp" + PowerUpType.ARMOR);
        int speedPowerUp = PlayerPrefs.GetInt("PowerUp" + PowerUpType.SPEED);
        int magnetPowerUp = PlayerPrefs.GetInt("PowerUp" + PowerUpType.MAGNET);
        int growthPowerUp = PlayerPrefs.GetInt("PowerUp" + PowerUpType.GROWTH);
        maxHealth += 5 * healthPowerUp;
        currentHealth = maxHealth;
        armor += armorPowerUp;
        speed += 0.05f * speedPowerUp;
        magnet += 0.05f * magnetPowerUp;
        growth += 0.05f * growthPowerUp;

        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMovement;
        inputActions.Player.Move.canceled += OnMovement;
    }

    void Update()
    {
        if (isDefeated) return;
        AttractNearbyExpItems();
        if (isInvicible)
        {
            invicibleTimer -= Time.deltaTime;
            if (invicibleTimer < 0)
            {
                isInvicible = false;
            }
        }   
        addingTimer -= Time.deltaTime;
        if (addingTimer < 0)
        {
            addingTimer = timeAdding;
            LevelUp();
        }
    }

    void FixedUpdate()
    {
        if (currentMoveInput.magnitude > 0.01f)
        {
            Vector2 position = rb2d.position;
            position += currentMoveInput * speed * PlayerAttributeBuffs.instance.speedRatio * Time.deltaTime;
            rb2d.MovePosition(position);
        }

        Vector2 move = new Vector2(currentMoveInput.x, currentMoveInput.y);
        if (!Mathf.Approximately(move.x, 0.0f))
        {
            lookXDirection = Mathf.Sign(move.x);
        }
        animator.SetFloat("Look X", lookXDirection);
        animator.SetFloat("Speed", move.magnitude);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentMoveInput = context.ReadValue<Vector2>();
        }
        if (context.canceled)
        {
            currentMoveInput = Vector2.zero;
        }
    }
    
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvicible)
                return;
            animator.SetTrigger("Hit");
            audioSource.PlayOneShot(AudioManager.instance.playerTakeDamage);
            isInvicible = true;
            invicibleTimer = timeInvincible;
            amount = Mathf.Clamp(amount + armor + PlayerAttributeBuffs.instance.armorBonus, amount, 0);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public bool IsFullHealth()
    {
        return currentHealth == maxHealth;
    }

    void AttractNearbyExpItems()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(rb2d.position, magnet * PlayerAttributeBuffs.instance.magnetRatio, LayerMask.GetMask("Exp"));
        foreach (Collider2D collider in colliders)
        {
            ExpItem expItem = collider.GetComponent<ExpItem>();
            if (expItem != null)
            {
                expItem.StartAttracting();
            }
        }
    }

    float ExperienceRequiredForLevel(int level)
    {
        if (level > 0 && level <= 30)
        {
            return -0.1f * Mathf.Pow(level, 2) + 6f * level - 4f;
        }
        else if (level > 30) 
        {
            return 0.6f * level - 69;
        }
        return 1;
    }

    public float GetRequiredExpToLevelUp()
    {
        return ExperienceRequiredForLevel(currentLevel + 1) - currentExperience;
    }

    public void AddExperience(float amount)
    {
        if (currentLevel >= maxLevel)
        {
            return;
        }
        currentExperience += amount * growth * PlayerAttributeBuffs.instance.growthRatio;
    }

    private void LevelUp()
    {
        int upgradeCount = 0;
        while (currentLevel < maxLevel && currentExperience >= ExperienceRequiredForLevel(currentLevel + 1))
        {
            LevelUpController.instance.PauseGame();
            float experienceRequired = ExperienceRequiredForLevel(currentLevel + 1);
            currentExperience -= experienceRequired;
            ++currentLevel;
            ++upgradeCount;
        }
        UIExpBar.instance.SetLevelText(currentLevel);
        if (currentLevel == maxLevel)
        {
            UIExpBar.instance.SetValue(1);
        }
        else 
        {
            UIExpBar.instance.SetValue(currentExperience / ExperienceRequiredForLevel(currentLevel + 1));
        }
        LevelUpController.instance.ShowLevelUpUI(upgradeCount);
    }

    public int getCurrentLevel()
    {
        return currentLevel;
    }

    public void DestroyCompletely()
    {
        if (instance == this)
        {
            instance = null;
            Destroy(gameObject);
        }
    }

    private void Die()
    {
        isDefeated = true;
        rb2d.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        rb2d.simulated = false;
        animator.SetTrigger("Dead");     
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        }
        StartCoroutine(HandleDeathAnimation());
    }

    private IEnumerator HandleDeathAnimation()
    {
        AnimatorStateInfo animationInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = animationInfo.length;
        audioSource.PlayOneShot(AudioManager.instance.playerDie);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(animationDuration);
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.8f, spriteRenderer.color.g, spriteRenderer.color.b);
        }
        yield return new WaitForSeconds(2f);
        GameManager.instance.EndGame(false);
    }
}