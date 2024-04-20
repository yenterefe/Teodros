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


    [SerializeField] private GameObject originalSword;
    [SerializeField] private GameObject warningSword;

    // Don't delete
    //[SerializeField] private ParticleSystem sparkle;
    //[SerializeField] private ParticleSystem blood;

    private GameInput gameInput;

    private float blockTimer = 0;
    private float enemySwordTimer = 0;

    private bool startBlockTimer =false;
    private bool startShieldTimer =false;
    private bool isEnemyAttacking;
    private bool shieldActive;
    private bool specialAttack;


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

        originalSword.SetActive(true);
    }

    private void Update()
    {
        shieldActive = animator.GetBehaviour<AttackStateA>().EnemyAttacking();
        specialAttack = animator.GetBehaviour<SpecialAttackState>().SpecialAttack();


        isEnemyAttacking = animator.GetBehaviour<AttackStateA>().EnemyAttacking();
        ManageShieldTimer();
        ManageEnemyAttackTimer();
        Parry();
        ChangeSwordMaterial();
    }
    
    private void ChangeSwordMaterial()
    {
        if (specialAttack == true)
        {
            originalSword.SetActive(false);
            warningSword.SetActive(true);
        }

       else
        {
            originalSword.SetActive(true);
            warningSword.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        bool shieldActive = playerAnimation.ShieldActive();

        //bool specialAttack = animator.GetBehaviour<SpecialAttackState>().SpecialAttack();

        if (other.gameObject.CompareTag("Player") && shieldActive == false)
        {
            playerHealthBar.GetComponent<Slider>().value -=20;

            // play hit animation 
            // blood.Play();

            // player loses half of their life against the special attack 
            if (specialAttack == true)
            {
                playerHealthBar.GetComponent<Slider>().value -= 50;
            }


        }

        if (other.gameObject.CompareTag("Shield") && shieldActive ==true)
        {
            startShieldTimer = true;

            // Don't delete
            // sparkle.Play();

            // Don't delete
            //play block animation

            // enemy cannot block special attack and must dodge 
            if (specialAttack == true)
            {
                playerHealthBar.GetComponent<Slider>().value -= 50;
            }
        }

        if(shieldActive == false)
        {
            enemySwordTimer= 0;
            startShieldTimer= false;
        }
    }


    // After this is to caculate time in order to determine if player parried enemy's attack

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
