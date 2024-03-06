using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject inputManager;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerPrefab;

    private GameInput input;
    private Animator enemyAnimation;
    private PlayerAnimation playerAnimation;
    
    private bool playerAttacking= false;
    private bool enemyBlocking = false;
    private bool playerSeen = false;

    private float distance;
    private float enemyAttackDistance = 2f;
    private float blockDistance = 3f;

    private const string _SHIELDBLOCK = "Block";
    private const string _ENEMYATTACK = "Attack";

    private void Awake()
    {
        input= inputManager.GetComponent<GameInput>();
        enemyAnimation = enemyPrefab.GetComponent<Animator>();
        playerAnimation = playerPrefab.GetComponent<PlayerAnimation>();
    }

    private void Start()
    {
        input.OnLightAttackPerformed += PlayerAttack;
        input.OnLightAttackPerformed += BlockAttack;
        input.OnLightAttackCanceled += ReturnToIdle;
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        Debug.Log(playerSeen);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.name == "Player")
            {
                playerSeen = true;
            }

            else
            {
                playerSeen = false;
                enemyAnimation.SetBool("Moving", false);
            }
        }
       
        if(playerSeen== true)
        {
            AttackPlayer();

            bool playerAimingGun = playerAnimation.IsPlayerAiming();

            if (playerAimingGun == false)
            {
                distance = Vector3.Distance(transform.position, player.transform.position);

                bool stopMovement = false;

                if (distance < 2)
                {
                    stopMovement = true;
                    enemyAnimation.SetBool("Moving", false);
                }

                if (stopMovement == false)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 1 * Time.deltaTime);

                    enemyAnimation.SetBool("Moving", true);
                }
            }

            // If player has gun and enemy is not armed with a gun, it will run for cover 
            else
            {
                Vector3 runAway = transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -1 * Time.deltaTime);

                transform.Rotate(new Vector3(transform.position.x, 180, transform.position.z));
            }
        }

        else
        {
                //enemy stays idle
        }
      
        
    }
    
    // This is to see if player is attacking so enemy can shield
    private void PlayerAttack(object receiver, EventArgs e)
    {
        playerAttacking = true;
        Invoke("CancelPlayerAttack",.75f);
    }

    private void CancelPlayerAttack()
    {
        playerAttacking = false;
    }

    private void BlockAttack(object receiver, EventArgs e)
    {
        if(playerAttacking)
        {
            if (distance < blockDistance)
            {
                Debug.Log("Block");
                enemyAnimation.SetBool(_SHIELDBLOCK, true);
                enemyBlocking = true;
            }
        }
    }

    private void ReturnToIdle(object receiver, EventArgs e)
    {
        float blockAnimationTimer = .75f;
        Invoke("TurnOffBlock", blockAnimationTimer);
        enemyBlocking = false;
    }

    private void TurnOffBlock()
    {
        enemyAnimation.SetBool(_SHIELDBLOCK, false);
    }
       
    private void AttackPlayer()
    {
        if (enemyBlocking==false)
        {
            if (distance < enemyAttackDistance)
            {
                // player will take damage if they are not blocking

                //Play attack animation 
                enemyAnimation.SetBool(_ENEMYATTACK, true);
            }

            else
            {
                enemyAnimation.SetBool(_ENEMYATTACK, false);
            }
        }
    }
}
