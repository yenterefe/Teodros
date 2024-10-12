using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject staminaBar;
    [SerializeField] private GameObject playerHealthBar;
    [SerializeField] private GameObject enemyHealthBar;
    [SerializeField] private GameObject inputManager;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject enemyTriggerBox;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject ammo;
    [SerializeField] private GameObject healthObject;
    [SerializeField] private GameObject inventory;

    [SerializeField] private ItemData bulletData;

    [SerializeField] private TextMeshProUGUI ammoIndicator;

    [SerializeField] private float depletion = 0.1f;
    [SerializeField] private float recuperation = 0.05f;

    private GameInput gameInput;
    private Player player;
    private EnemySpawner enemySpawner;
    private Collector collector;
    private Bullet bullet;
    private Health health;

    public EventHandler<ItemData> OnBulletUIPressed;

    private PlayerAnimation playerAnimation;

    private bool isStaminaDepleted = false;
    private bool recuperateStamina = false;
    private bool isPlayerAiming;
    private bool isPlayerRunning = false;
    private bool isRifleOn;
    private bool isEnemyTriggerEntered;
    private bool isAmmoCollected = false;
    private bool isAmmoUIPressed = false;

    private int totalAmmunition = 5;
    private int firedAmmunition = 1;
  

    private void Awake()
    {
        gameInput = inputManager.GetComponent<GameInput>();
        playerAnimation = playerPrefab.GetComponent<PlayerAnimation>();
        player = playerObject.GetComponent<Player>();
        enemySpawner = enemyTriggerBox.GetComponent<EnemySpawner>();
        collector = playerObject.GetComponent<Collector>();
        bullet = ammo.GetComponent<Bullet>();
        health = healthObject.GetComponent<Health>();
    }

    private void OnEnable()
    {
        gameInput.OnLightAttackPerformed += DepleteStamina;
        gameInput.OnLightAttackCanceled += RecuperateStamina;
        gameInput.OnRifleFirePerformed += AmmoManager;
        gameInput.OnPlayerRunningPeformed += PlayerRunning;
        gameInput.OnPlayerRunningCanceled += PlayerWalking;
        gameInput.OnPausePerformed += PauseApplication;
        gameInput.OnUnPausePerformed += ResumeApplication;
        gameInput.OnUIActionPerformed += BulletCollected;

        // Get Collector from the OnTrigger method for the ICollector interface for any collectable objects.
        Bullet.OnBulletCollected += BulletManager;
        Health.OnHealthCollected += HealthManager;

    }

    private void OnDisable()
    {
        gameInput.OnLightAttackPerformed -= DepleteStamina;
        gameInput.OnLightAttackCanceled -= RecuperateStamina;
        gameInput.OnRifleFirePerformed -= AmmoManager;
        gameInput.OnPlayerRunningPeformed -= PlayerRunning;
        gameInput.OnPlayerRunningCanceled -= PlayerWalking;
        gameInput.OnPausePerformed -= PauseApplication;
        gameInput.OnUnPausePerformed -= ResumeApplication;

        // Get Collector from the OnTrigger method for the ICollector interface for any collectable objects.
        Bullet.OnBulletCollected -= BulletManager;
        Health.OnHealthCollected -= HealthManager;
    }

    private void Start()
    {
        // sets framerate
        Application.targetFrameRate = 100;
    }

    void Update()
    {
        //Debug.Log($"total ammunition: {totalAmmunition} " + $"left ammo: {leftBullet}");

        //to determine if enemy spawner is triggered
        isEnemyTriggerEntered = enemySpawner.IsEnemyTriggerEntered();

        RemoveAmmoFromStack();

        SpawnEnemy();

        ManageStaminaBar();
        ManagePlayerHealthBar();
        isPlayerAiming = playerAnimation.IsPlayerAiming();

        isRifleOn = playerAnimation.IsRifleModeActivated();

        ammoIndicator.text = "Ammo: " + totalAmmunition.ToString();

        if (totalAmmunition <= 0)
        {
            ammoIndicator.text = "Ammo: " + 0;
        }

        bool isEnemySighted = player.IsEnemySighted();
        bool isPlayershooting = playerAnimation.IsRifleShot();

        if (isEnemySighted && isPlayershooting)
        {
            enemyHealthBar.GetComponent<Slider>().value = 0;
        }
    }

    private void PlayerRunning(object source, EventArgs e)
    {
        isPlayerRunning = true;
    }

    private void PlayerWalking(object source, EventArgs e)
    {
        isPlayerRunning = false;
    }

    private void DepleteStamina(object source, EventArgs e)
    {
        if (staminaBar.GetComponent<Slider>().value != 0 && !isPlayerAiming && !isRifleOn)
        {
            isStaminaDepleted = true;
            recuperateStamina = false;
        }
    }

    private void RecuperateStamina(object source, EventArgs e)
    {
        if (staminaBar.GetComponent<Slider>().value != 100)
        {
            recuperateStamina = true;
            isStaminaDepleted = false;
        }
    }

    private void ManageStaminaBar()
    {
        if (isStaminaDepleted || isPlayerRunning)
        {
            staminaBar.GetComponent<Slider>().value -= depletion;
        }

        if (recuperateStamina || !isPlayerRunning)
        {
            staminaBar.GetComponent<Slider>().value += recuperation;
        }
    }

    private void ManagePlayerHealthBar()
    {
        if (playerHealthBar.GetComponent<Slider>().value <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    private void AmmoManager(object source, EventArgs e)
    {
        if (isPlayerAiming)
        {
            totalAmmunition -= firedAmmunition;
        }
    }

    public int GetAmmunition()
    {
        return totalAmmunition;
    }

    private void SpawnEnemy()
    {
        if (isEnemyTriggerEntered && enemyTriggerBox != null)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.SetActive(true);
                Vector3 offset = new Vector3(0, 0, 10);
                enemy.transform.position = enemyTriggerBox.transform.position;
            }
        }
    }

    public void BulletManager(object source, ItemData data)
    {
  
       
            isAmmoCollected = true;
    }

    //When pressing the slot containing ammo UI, will update totalBullet
    public void BulletCollected(object source, EventArgs e)
    {
        if(isAmmoCollected && totalAmmunition <5)
        {
            totalAmmunition += 1;
            isAmmoUIPressed = true;
            isAmmoCollected = false;
        }
    }

    // This will trigger an event to notify the inventory to reduce stack for the ammunition 
    public void RemoveAmmoFromStack()
    {
        if (isAmmoUIPressed)
        {
            if (OnBulletUIPressed != null)
            {
                OnBulletUIPressed(this, bulletData);
            }
        }
    }

    private void HealthManager(object source, ItemData data)
    {
        // Must refactor this code to work with the new UI inventory system

        /*if (playerHealthBar.GetComponent<Slider>().value < 100)
        {
            playerHealthBar.GetComponent<Slider>().value = 100;
        }*/
    }

    private void PauseApplication(object source, EventArgs e)
    {
        Time.timeScale = 0;
        playerPrefab.GetComponent<Animator>().enabled = false;
        // Do the same for enemy if active or other NPCs
        inventory.SetActive(true);
    }

    private void ResumeApplication(object source, EventArgs e)
    {
        Time.timeScale = 1f;
        playerPrefab.GetComponent<Animator>().enabled = true;
        // Do the same for enemy if active or other NPCs
        inventory.SetActive(false);
    }
}
