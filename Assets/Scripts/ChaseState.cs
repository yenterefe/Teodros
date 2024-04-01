using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{ 
    private NavMeshAgent agent;

    private GameObject enemy;
    private GameObject player;

    private Transform playerPos;
    private Transform enemyPos;

    private Enemy enemyScript;

    private float distance;

    private bool playerIsAttacking;

    private const string _MOVE = "Moving";
    private const string _ATTACK = "Attack";
    private const string _BLOCK = "Block";
 
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

       

        enemy = GameObject.Find("Enemy A");

        player = GameObject.Find("Player");

        enemyPos = GameObject.Find("Enemy A").transform ;

        playerPos = GameObject.Find("Player").transform;

        enemyScript = enemy.GetComponent<Enemy>();

        agent = enemy.GetComponent<NavMeshAgent>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerIsAttacking = enemyScript.PlayerAttacking();

        distance = Vector3.Distance(animator.transform.position, player.transform.position);

       

        if (distance < 2)
            {
                agent.SetDestination(agent.transform.position);

                animator.SetBool(_MOVE, false);

                animator.SetBool(_ATTACK, true);

                if(playerIsAttacking == true)
                {
                    animator.SetBool(_BLOCK, true);

                }
            }

        else
        {

            agent.SetDestination(player.transform.position);
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        
    }
}
