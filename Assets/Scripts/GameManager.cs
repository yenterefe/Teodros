using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject staminaBar;
    [SerializeField] private GameObject playerBar;
    [SerializeField] private GameObject inputManager;
    [SerializeField] private float depletion = 0.1f;
    [SerializeField] private float recuperation = 0.05f;
    private GameInput gameInput;
    private bool depleteStamina=false;
    private bool recuperateStamina = false;
    


    private void Awake()
    {
        gameInput= inputManager.GetComponent<GameInput>();  
    }

    // Start is called before the first frame update
    void Start()
    {
        gameInput.OnLightAttackPerformed += DepleteStamina;
        gameInput.OnLightAttackCanceled += RecuperateStamina;
    }

    // Update is called once per frame
    void Update()
    {
        ManageStaminaBar();
        ManagePlayerHealthBar();
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
        if (depleteStamina == true)
        {
            staminaBar.GetComponent<Slider>().value -= depletion;
        }

        if (recuperateStamina == true)
        {
            staminaBar.GetComponent<Slider>().value += recuperation;
        }
    }

    private void ManagePlayerHealthBar()
    {
        if(playerBar.GetComponent<Slider>().value <=0)
        {
            Debug.Log("Game Over");
        }
    }
}
