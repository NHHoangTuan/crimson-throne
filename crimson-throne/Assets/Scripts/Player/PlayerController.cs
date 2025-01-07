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
    private float timeInvincible = 0.5f;
    private bool isInvicible;
    private float invicibleTimer;

    // AUDIO
    private AudioSource audioSource;

    // LEVEL
    [Space(10)]
    [Header("PLAYER LEVELS")]
    [SerializeField] private int maxLevel = 31;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private float currentExperience = 0;

    // INSTANCE
    public static PlayerController instance{get; private set;}

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        inputActions = new PlayerInputActions();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        maxHealth += PlayerAttributeBuffs.instance.maxHealthBonus;
        currentHealth = maxHealth;
        armor += PlayerAttributeBuffs.instance.armorBonus;
        speed *= PlayerAttributeBuffs.instance.speedRatio;
        magnet *= PlayerAttributeBuffs.instance.magnetRatio;
        growth *= PlayerAttributeBuffs.instance.growthRatio;

        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMovement;
        inputActions.Player.Move.canceled += OnMovement;

        UIExpBar.instance.SetValue(currentExperience / ExperienceRequiredForLevel(currentLevel + 1));
        UIDialog.instance.SetLevelText(currentLevel);
    }

    void Update()
    {
        AttractNearbyExpItems();
        if (isInvicible)
        {
            invicibleTimer -= Time.deltaTime;
            if (invicibleTimer < 0)
            {
                isInvicible = false;
            }
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
            isInvicible = true;
            invicibleTimer = timeInvincible;
            amount = Mathf.Clamp(amount + armor + PlayerAttributeBuffs.instance.amountBonus, amount, 0);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
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
        if (currentLevel < maxLevel && currentExperience >= ExperienceRequiredForLevel(currentLevel + 1))
        {
            float experienceRequired = ExperienceRequiredForLevel(currentLevel + 1);
            currentExperience -= experienceRequired;
            ++currentLevel;

            LevelUpController.instance.ShowLevelUpUI();

            experienceRequired = ExperienceRequiredForLevel(currentLevel + 1);
            if (currentExperience >= experienceRequired)
            {
                currentExperience = experienceRequired - 1;
            }
        }
        UIDialog.instance.SetLevelText(currentLevel);
        if (currentLevel == maxLevel)
        {
            UIExpBar.instance.SetValue(1);
        }
        else 
        {
            UIExpBar.instance.SetValue(currentExperience / ExperienceRequiredForLevel(currentLevel + 1));
        }
    }
}

// public void Die()
// {
//     inputActions.Player.Disable();
//     animator.SetTrigger("Die");
//     rb2d.simulated = false;
//     Destroy(gameObject, 1f);
//     PlayerAttributeBuffs.instance.Reset();
// }
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerController : MonoBehaviour
// {
//     // Singleton instance.
//     public static PlayerController instance;

//     [Space(10)]
//     // Animator component.
//     public Animator animator;
//     // SpriteRenderer component.
//     public SpriteRenderer spriteRenderer;
//     // List of footstep particles.
//     public List<ParticleSystem> footParticles;

//     [Space(10)]
//     // Player movement speed.
//     public float speed = 3f;
//     // Speed multiplier for leveling up.
//     public float speedMultiplier = 1.1f;

//     [Space(10)]
//     // Pickup range for weapons.
//     public float pickupRange = 1.5f;
//     // Pickup range multiplier for leveling up.
//     public float pickupRangeMultiplier = 1.1f;

//     [Space(10)]
//     // Distance traveled by the player.
//     public float playerDistance;

//     [Space(10)]
//     // Maximum number of weapons.
//     public int maxWeapons = 3;
//     // List of unassigned weapons.
//     public List<Weapon> unassignedWeapons;
//     // List of assigned weapons.
//     public List<Weapon> assignedWeapons;
//     // List of all weapons.
//     public List<Weapon> listWeapons;

//     [HideInInspector]
//     // Fully levelled weapons.
//     public List<Weapon> fullyLevelledWeapons = new List<Weapon>();

//     [HideInInspector]
//     // Flag for chest state.
//     public bool isChestClosed = true;
//     [HideInInspector]
//     // Flag for spawned chest.
//     public bool isChestSpawned = false;

//     [HideInInspector]
//     // List of dialogue triggers.
//     public List<DialogueTrigger> dialogueTriggers = new List<DialogueTrigger>();

//     // Movement vector.
//     Vector3 movement;

//     private void Awake()
//     {
//         // Set instance as this.
//         instance = this;
//     }

//     void Start()
//     {
//         // Add a weapon if no weapons are assigned.
//         if (assignedWeapons.Count == 0)
//         {
//             AddWeapon(Random.Range(0, unassignedWeapons.Count));
//         }
//     }

//     // Movement input handler.
//     private void OnMovement(InputValue value)
//     {
//         Vector2 inputMovement = value.Get<Vector2>();
//         movement = new Vector3(inputMovement.x, inputMovement.y, 0);
//     }

//     // Open dialogue handler.
//     public void OnOpenDialogue()
//     {
//         foreach (DialogueTrigger trigger in dialogueTriggers)
//         {
//             if (trigger.isInRange && !DialogueManager.instance.isDialogueOpen)
//             {
//                 trigger.TriggerDialogue();
//             }
//         }
//     }

//     // Next sentence handler.
//     public void OnNextSentence()
//     {
//         DialogueManager.instance.DisplayNextSentence();
//     }

//     // Fixed update for movement.
//     private void FixedUpdate()
//     {
//         transform.position += movement * speed * Time.fixedDeltaTime;

//         float distanceThisFrame = movement.magnitude * speed * Time.fixedDeltaTime;
//         playerDistance += distanceThisFrame;

//         // Flip player sprite.
//         Flip();
//         // Handle running animation and particles.
//         Running();
//     }

//     // Flip player sprite based on movement direction.
//     private void Flip()
//     {
//         if (movement.x > 0)
//         {
//             spriteRenderer.flipX = false;
//         }
//         else if (movement.x < 0)
//         {
//             spriteRenderer.flipX = true;
//         }
//     }

//     // Handle running animation and particles.
//     private void Running()
//     {
//         foreach (ParticleSystem particles in footParticles)
//         {
//             if (movement.magnitude > 0)
//             {
//                 animator.SetBool("isRunning", true);

//                 if (!particles.isPlaying)
//                 {
//                     particles.Play();
//                     SFXManager.instance.PlaySFX(0);
//                 }
//             }
//             else
//             {
//                 animator.SetBool("isRunning", false);

//                 if (particles.isPlaying)
//                 {
//                     particles.Stop();
//                     SFXManager.instance.StopSFX(0);
//                 }
//             }
//         }
//     }

//     // Add a weapon to the player's inventory by index.
//     public void AddWeapon(int weaponNumber)
//     {
//         if (weaponNumber < unassignedWeapons.Count)
//         {
//             if (unassignedWeapons[weaponNumber].tag == "PlayerUpdate")
//             {
//                 for (int i = 0; i < unassignedWeapons.Count; i++)
//                 {
//                     if (i != weaponNumber && unassignedWeapons[i].tag != "PlayerUpdate")
//                     {
//                         weaponNumber = i;
//                         break;
//                     }
//                 }
//             }

//             assignedWeapons.Add(unassignedWeapons[weaponNumber]);
//             listWeapons.Add(unassignedWeapons[weaponNumber]);

//             unassignedWeapons[weaponNumber].gameObject.SetActive(true);

//             unassignedWeapons.RemoveAt(weaponNumber);
//         }
//     }

//     // Add a weapon to the player's inventory directly.
//     public void AddWeapon(Weapon weaponToAdd)
//     {
//         weaponToAdd.gameObject.SetActive(true);

//         assignedWeapons.Add(weaponToAdd);

//         if (weaponToAdd.tag != "PlayerUpdate")
//         {
//             listWeapons.Add(weaponToAdd);
//         }

//         unassignedWeapons.Remove(weaponToAdd);
//     }

//     // Level up speed.
//     public void SpeedLevelUp()
//     {
//         speed *= speedMultiplier;
//     }
    

//     // Level up pickup range.
//     public void PickupRangeLevelUp()
//     {
//         pickupRange *= pickupRangeMultiplier;
//     }
// }
