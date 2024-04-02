using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemySword : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerHealthBar;
    [SerializeField] private GameObject inputManager;
    [SerializeField] private Animator animator;

    // Don't delete
    //[SerializeField] private ParticleSystem sparkle;
    //[SerializeField] private ParticleSystem blood;

    private GameInput gameInput;

    private float blockTimer = 0;
    private float enemySwordTimer = 0;

    private bool startBlockTimer =false;
    private bool startShieldTimer =false;
    private bool isEnemyAttacking;

    private PlayerAnimation playerAnimation;

    private void Awake()
    {
        playerAnimation= playerPrefab.GetComponent<PlayerAnimation>();
        gameInput = inputManager.GetComponent<GameInput>();
    }

    private void Start()
    {
        gameInput.OnShieldPerformed += StartTimer;
        gameInput.OnShieldCanceled += CancelTimer;

        //You will need this line for your other class!!!
        animator.GetBehaviour<AttackStateA>().EnemyAttacking();
    }

    private void Update()
    {

       isEnemyAttacking = animator.GetBehaviour<AttackStateA>().EnemyAttacking();
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

            // play hit animation 
            // blood.Play();

        }

        if (other.gameObject.CompareTag("Shield") && shieldActive ==true)
        {
            startShieldTimer = true;
            
            // Don't delete
            // sparkle.Play();

            //play block animation
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
        if(startShieldTimer == true && startBlockTimer ==true && isEnemyAttacking ==true)
        {
            if (Mathf.Abs(blockTimer - enemySwordTimer) < 0.9f)
            {
                Debug.Log("Parry");
                startShieldTimer=false;
            }
        }
    }
}
