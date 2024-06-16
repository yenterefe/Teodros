using System.Collections;
using System.Collections.Generic;
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
  

    [SerializeField] private TextMeshProUGUI ammoIndicator;

    [SerializeField] private float depletion = 0.1f;
    [SerializeField] private float recuperation = 0.05f;

    private GameInput gameInput;
    private Player player;
    private EnemySpawner enemySpawner;
    private Collector collector;
    private Bullet bullet;
    private Health health;

    private PlayerAnimation playerAnimation;

    private bool isStaminaDepleted=false;
    private bool recuperateStamina = false;
    private bool isPlayerAiming;
    private bool isPlayerRunning = false;
    private bool isRifleOn;
    private bool isEnemyTriggerEntered;
    private bool isAmmoCollected = false;

    private int totalAmmunition = 5;
    private int firedAmmunition = 1;
    private int leftBullet = 5;

    private int pauseCounter = 0;

    private void Awake()
    {
        gameInput= inputManager.GetComponent<GameInput>();  
        playerAnimation = playerPrefab.GetComponent<PlayerAnimation>();
        player = playerObject.GetComponent<Player>();
        enemySpawner = enemyTriggerBox.GetComponent<EnemySpawner>();
        collector= playerObject.GetComponent<Collector>();
        bullet = ammo.GetComponent<Bullet>();  
        health = healthObject.GetComponent<Health>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameInput.OnLightAttackPerformed += DepleteStamina;
        gameInput.OnLightAttackCanceled += RecuperateStamina;
        gameInput.OnRifleFirePerformed += AmmoManager;
        gameInput.OnPlayerRunningPeformed += PlayerRunning;
        gameInput.OnPlayerRunningCanceled += PlayerWalking;
        gameInput.OnPausePerformed += PauseApplication;

        // Get Collector from the OnTrigger method for the ICollector interface for any collectable objects.
        bullet.OnBulletCollected += BulletManager;
        health.OnHealthCollected += HealthManager;



        // sets framerate
        Application.targetFrameRate = 100;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"total ammunition: {totalAmmunition} " + $"left ammo: {leftBullet}");

        Debug.Log($"Time scale value is: {Time.timeScale}");

        //to determine if enemy spawner is triggered
        isEnemyTriggerEntered = enemySpawner.IsEnemyTriggerEntered();

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

        if(isEnemySighted && isPlayershooting)
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
        if(staminaBar.GetComponent<Slider>().value != 0 && !isPlayerAiming && !isRifleOn)
        {
           isStaminaDepleted = true;
           recuperateStamina= false;
        }
    }

    private void RecuperateStamina(object source, EventArgs e)
    {
        if(staminaBar.GetComponent<Slider>().value != 100)
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

        if (recuperateStamina|| !isPlayerRunning)
        {
            staminaBar.GetComponent<Slider>().value += recuperation;
        }
    }

    private void ManagePlayerHealthBar()
    {
        if(playerHealthBar.GetComponent<Slider>().value <=0)
        {
            Debug.Log("Game Over");
        }
    }

    private void AmmoManager(object source, EventArgs e)
    {
        if(isPlayerAiming)
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
        if(isEnemyTriggerEntered && enemyTriggerBox != null)
        {
            if(!enemy.activeInHierarchy)
            {
                enemy.SetActive(true);
                Vector3 offset = new Vector3(0, 0, 10);
                enemy.transform.position = enemyTriggerBox.transform.position;
            }
        }   
    }

    private void BulletManager(object source, EventArgs e)
    {
       if(totalAmmunition < 5 && leftBullet>0)
        {
            // The rifle can only take 5 ammos and if player has more than 5, there will be some ammo left for next time. But, if the ammo is completely used it will disappear 

            leftBullet-= (5 - totalAmmunition);
            totalAmmunition = 5;
        }

       if(leftBullet ==0)
        {
            ammo.SetActive(false);
        }

    }

    private void HealthManager(object source, EventArgs e)
    {
        if(playerHealthBar.GetComponent<Slider>().value <100)
        {
            playerHealthBar.GetComponent<Slider>().value = 100;
            healthObject.SetActive(false);
        }
        
    }

    private void PauseApplication(object source, EventArgs e)
    {

        pauseCounter++;
        
        if(pauseCounter %2 !=0)
        {
            Time.timeScale = 0;
           playerPrefab.GetComponent<Animator>().enabled = false;
            // Do the same for enemy if active or other NPCs

        }

        else
        {
            Time.timeScale = 1f;
            playerPrefab.GetComponent<Animator>().enabled = true;
            // Do the same for enemy if active or other NPCs
        }
    }
}
