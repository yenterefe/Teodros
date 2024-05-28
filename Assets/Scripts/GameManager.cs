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

    [SerializeField] private TextMeshProUGUI ammoIndicator;

    [SerializeField] private float depletion = 0.1f;
    [SerializeField] private float recuperation = 0.05f;

    private GameInput gameInput;
    private Player player;
    private EnemySpawner enemySpawner;

    private PlayerAnimation playerAnimation;

    private bool isStaminaDepleted=false;
    private bool recuperateStamina = false;
    private bool isPlayerAiming;
    private bool isPlayerRunning = false;
    private bool isRifleOn;
    private bool isEnemyTriggerEntered;

    private int totalAmmunition = 5;
    private int firedAmmunition = 1;

    private void Awake()
    {
        gameInput= inputManager.GetComponent<GameInput>();  
        playerAnimation = playerPrefab.GetComponent<PlayerAnimation>();
        player = playerObject.GetComponent<Player>();
        enemySpawner = enemyTriggerBox.GetComponent<EnemySpawner>();

    }

    // Start is called before the first frame update
    void Start()
    {
        gameInput.OnLightAttackPerformed += DepleteStamina;
        gameInput.OnLightAttackCanceled += RecuperateStamina;
        gameInput.OnRifleFirePerformed += AmmoManager;
        gameInput.OnPlayerRunningPeformed += PlayerRunning;
        gameInput.OnPlayerRunningCanceled += PlayerWalking;

        // sets framerate
        Application.targetFrameRate = 100;

    }

    // Update is called once per frame
    void Update()
    {
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

    private void PlayerRunning(object receiver, EventArgs e)
    {
        isPlayerRunning = true;
    }

    private void PlayerWalking(object receiver, EventArgs e)
    {
        isPlayerRunning = false;
    }

    private void DepleteStamina(object receiver, EventArgs e)
    {
        if(staminaBar.GetComponent<Slider>().value != 0 && !isPlayerAiming && !isRifleOn)
        {
           isStaminaDepleted = true;
           recuperateStamina= false;
        }
    }

    private void RecuperateStamina(object receiver, EventArgs e)
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

    private void AmmoManager(object receiver, EventArgs e)
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
}
