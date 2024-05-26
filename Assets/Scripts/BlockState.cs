using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockState : StateMachineBehaviour
{
    private GameObject enemy;
    private GameObject player;

    private float distance;

    private Transform playerPos;
    private Enemy enemyScript;

    private PlayerSword playerSword;

    private const string MOVE = "Moving";
    private const string ATTACK = "Attack";
    private const string BLOCK = "Block";

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = GameObject.Find("Enemy");

        player = GameObject.Find("Player");

        playerPos = GameObject.Find("Player").transform;

        enemyScript = enemy.GetComponent<Enemy>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject sword = GameObject.Find("Sword");

        if (sword == null)
        {
            Debug.Log("Sword is NOT active");
        }

        else
        {
            playerSword = sword.GetComponent<PlayerSword>();

            bool isEnemyHit = playerSword.IsEnemyHit();

            if(isEnemyHit)
            {
                animator.SetTrigger("Hit");
            } 
        }

        distance = Vector3.Distance(animator.transform.position, playerPos.position);

        animator.transform.LookAt(playerPos);

        bool isPlayerIsAttacking = enemyScript.PlayerAttacking();

        animator.SetBool(MOVE, false);
        animator.SetBool(ATTACK, false);

        if (!isPlayerIsAttacking)
        {
            animator.SetBool(MOVE, true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(MOVE, true);
    }
}
