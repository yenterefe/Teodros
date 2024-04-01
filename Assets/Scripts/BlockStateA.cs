using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockStateA : StateMachineBehaviour
{
    private GameObject enemy;
    private GameObject player;

    private float distance;
    private float timer;

    private Transform playerPos;
    private Enemy enemyScript;

    private const string _MOVE = "Moving";
    private const string _ATTACK = "Attack";
    private const string _BLOCK = "Block";

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = GameObject.Find("Enemy");

        player = GameObject.Find("Player");

        playerPos = GameObject.Find("Player").transform;

        enemyScript = enemy.GetComponent<Enemy>();
        
        timer = 0;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Enemy looks at player while blocking 
        animator.transform.LookAt(playerPos);

        distance = Vector3.Distance(animator.transform.position, playerPos.position);

        bool playerIsAttacking = enemyScript.PlayerAttacking();

        animator.SetBool(_MOVE, false);
        animator.SetBool(_ATTACK, false);

        if (distance <2)
        {
            // Don't delete!!

            /*if(weaponType== shotel)
            {
                timer += Time.deltaTime;

                float blockTimer = 2f;

                if(timer < blockTimer)
                {
                    animator.SetBool(_BLOCK, false);
                }

            }*/
            
            if (playerIsAttacking == false)
            {
                animator.SetBool(_BLOCK, false);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_MOVE, true);
    }

}
