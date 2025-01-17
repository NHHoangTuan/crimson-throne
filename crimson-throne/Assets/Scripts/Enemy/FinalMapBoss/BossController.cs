using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BossController : EnemyController
{
    #region Variables
    [Header("Boss Attributes")]
    [SerializeField] private int maxHealth = 1000;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private string dialogText = "Save the princess? I decide her fate.";
    [SerializeField] private GameObject dialogBox; 
    private bool eventTriggered = false;
    #endregion

    #region Initialization
    protected override void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        health = maxHealth;
        killAll = true;
        isDefeated = true;
        dialogBox?.SetActive(false);
    }

    protected override void Update()
    {
        if (!eventTriggered) 
        {
            CheckPlayerInRange();
        }
        base.Update();
    }
    #endregion

    #region Set Up Boss
    private void CheckPlayerInRange()
    {
        if (PlayerController.instance == null) return;
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerController.instance.transform.position);
        if (distanceToPlayer <= detectionRadius && !eventTriggered)
        {
            StartCoroutine(TriggerBossEvent());
        }
    }
    
    private IEnumerator TriggerBossEvent()
    {
        eventTriggered = true;
        SetUpAttributes();
        PlayerController.instance.GetComponent<Rigidbody2D>().simulated = false;

        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        PlayCinemaToBoss(mainCamera);
        yield return new WaitForSeconds(3f);
        PlayCinemaToPrincess(mainCamera);
        yield return new WaitForSeconds(1.5f);
        PlayCinemaToPlayer(mainCamera);

        PlayerController.instance.GetComponent<Rigidbody2D>().simulated = true;
        isDefeated = false;
        StartAbility();
        AudioManager.instance.PlayMusic(AudioManager.instance.daVinciBackground);
        WaveManager.instance.canStart = true;
    }

    private void SetUpAttributes()
    {
        spawnItemAction = pos => ItemSpawner.instance?.SpawnKey(pos);
        target = PlayerController.instance.transform;
    }

    private void StartAbility()
    {
        BossKatanaSpawner.instance?.StartAbility();
        BossDash.instance?.StartAbility();
        BossFireSpawner.instance?.StartAbility();
    }

    public override void TakeDamage(float damageTaken, float knockbackForce)
    {
        base.TakeDamage(damageTaken, knockbackForce);
        BossHealthBar.instance.SetValue(health / (float)maxHealth);
    }
    #endregion

    #region Cinema
    private void PlayCinemaToBoss(GameObject mainCamera)
    {
        if (mainCamera != null)
        {
            CinemachineCamera cinemachineCamera = mainCamera.GetComponent<CinemachineCamera>();
            if (cinemachineCamera != null)
            {
                cinemachineCamera.Follow = transform; 
                cinemachineCamera.Lens.OrthographicSize -= 2; 
            }
        }
        if (dialogBox != null)
        {
            dialogBox.SetActive(true);
            dialogBox.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = dialogText;
        }
    }

    private void PlayCinemaToPrincess(GameObject mainCamera)
    {
        if (mainCamera != null)
        {
            CinemachineCamera cinemachineCamera = mainCamera.GetComponent<CinemachineCamera>();
            GameObject princess = GameObject.FindGameObjectWithTag("Princess");
            if (cinemachineCamera != null && princess != null)
            {
                cinemachineCamera.Follow = princess.transform; 
            }
        }
    }
        
    private void PlayCinemaToPlayer(GameObject mainCamera)
    {
        if (dialogBox != null)
        {
            dialogBox.SetActive(false);
        }
        if (mainCamera != null)
        {
            CinemachineCamera cinemachineCamera = mainCamera.GetComponent<CinemachineCamera>();
            if (cinemachineCamera != null)
            {
                cinemachineCamera.Follow = PlayerController.instance.transform;
                cinemachineCamera.Lens.OrthographicSize += 2;
            }
        }
    }
    #endregion
}