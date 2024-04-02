using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemySword : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerHealthBar;
    [SerializeField] GameObject inputManager;
    private GameInput gameInput;

    private float blockTimer = 0;
    private float enemySwordTimer = 0;

    bool startBlockTimer =false;
    bool startShieldTimer =false;

    private PlayerAnimation playerAnimation;

    private void Awake()
    {
        playerAnimation= playerPrefab.GetComponent<PlayerAnimation>();
        gameInput= inputManager.GetComponent<GameInput>();
    }

    private void Start()
    {
        gameInput.OnShieldPerformed += StartTimer;
        gameInput.OnShieldCanceled += CancelTimer;
    }

    private void Update()
    {
       ManageShieldTimer();
       ManageEnemyAttackTimer();
       Parry();
    }

    private void OnTriggerEnter(Collider other)
    {
        bool shieldActive = playerAnimation.ShieldActive();

        if (other.gameObject.CompareTag("Player") && shieldActive == false)
        {
            playerHealthBar.GetComponent<Slider>().value -=20;        
        }

        if(other.gameObject.CompareTag("Shield") && shieldActive ==true)
        {
            //enemySwordTimer += Time.deltaTime;
            startShieldTimer = true;
        }

        if(shieldActive == false)
        {
            enemySwordTimer= 0;
            startShieldTimer= false;
        }
    }

    private void StartTimer(object receiver, EventArgs e)
    {
        startBlockTimer = true;
    }

    private void CancelTimer(object receiver, EventArgs e)
    {
        startBlockTimer= false;
    }

    private void ManageShieldTimer()
    {
        if (startBlockTimer == true)
        {
            blockTimer += Time.deltaTime;
        }

        else
        {
            blockTimer = 0;
        }
    }

    private void ManageEnemyAttackTimer()
    {
        if (startShieldTimer == true)
        {
            enemySwordTimer += Time.deltaTime;
        }
    }

    private void Parry()
    {
        if(startShieldTimer == true && startBlockTimer ==true)
        {
            if (Mathf.Abs(blockTimer - enemySwordTimer) < 0.9f)
            {
                Debug.Log("Parry");
                startShieldTimer=false;
            }
        }
    }
}
