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

    private GameInput input;
    private Animator enemyAnimation;
    
    private bool playerAttacking= false;
    private bool enemyBlocking = false;

    private float distance;
    private float enemyAttackDistance = 2f;
    private float blockDistance = 3f;

    private const string _SHIELDBLOCK = "Block";
    private const string _ENEMYATTACK = "Attack";

    private void Awake()
    {
        input= inputManager.GetComponent<GameInput>();
        enemyAnimation=enemyPrefab.GetComponent<Animator>();
    }

    private void Start()
    {
        input.OnLightAttackPerformed += PlayerAttack;
        input.OnLightAttackPerformed += BlockAttack;
        input.OnLightAttackCanceled += ReturnToIdle;
    }

    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);
        Debug.Log(distance);
        AttackPlayer();
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
        if(enemyBlocking==false)
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
