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

    [SerializeField] private TextMeshProUGUI ammoIndicator;

    [SerializeField] private float depletion = 0.1f;
    [SerializeField] private float recuperation = 0.05f;

    private GameInput gameInput;
    private Player player;

    private PlayerAnimation playerAnimation;

    private bool depleteStamina=false;
    private bool recuperateStamina = false;
    private bool isPlayerAiming;
    private bool isPlayerRunning = false;

    private int totalAmmunition = 5;
    private int firedAmmunition = 1;

    private void Awake()
    {
        gameInput= inputManager.GetComponent<GameInput>();  
        playerAnimation = playerPrefab.GetComponent<PlayerAnimation>();
        player = playerObject.GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameInput.OnLightAttackPerformed += DepleteStamina;
        gameInput.OnLightAttackCanceled += RecuperateStamina;
        gameInput.OnRifleFirePerformed += AmmoManager;
        gameInput.OnPlayerRunningPeformed += PlayerRunning;
        gameInput.OnPlayerRunningCanceled += PlayerWalking;
    }

    // Update is called once per frame
    void Update()
    {
        ManageStaminaBar();
        ManagePlayerHealthBar();

        isPlayerAiming = playerAnimation.IsPlayerAiming();

        ammoIndicator.text = "Ammo: " + totalAmmunition.ToString();

        if (totalAmmunition <= 0)
        {
            ammoIndicator.text = "Ammo: " + 0;
        }

        bool enemySighted = player.IsEnemySighted();
        bool playershooting = playerAnimation.IsRifleShot();

        if(enemySighted == true && playershooting == true)
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
        if(staminaBar.GetComponent<Slider>().value != 0)
        {
           depleteStamina = true;
           recuperateStamina= false;
        }
    }

    private void RecuperateStamina(object receiver, EventArgs e)
    {
        if(staminaBar.GetComponent<Slider>().value != 100)
        {
            recuperateStamina = true;
            depleteStamina = false;
        }
    }

    private void ManageStaminaBar()
    {
        if (depleteStamina == true || isPlayerRunning ==true)
        {
            staminaBar.GetComponent<Slider>().value -= depletion;
        }

        if (recuperateStamina == true || isPlayerRunning ==false)
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
        if(isPlayerAiming ==true)
        {
            totalAmmunition -= firedAmmunition;
        }
    }

    public int GetAmmunition()
    {
        return totalAmmunition;
    }
}
