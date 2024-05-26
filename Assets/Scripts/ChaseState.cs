using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    [SerializeField] private float attackDistance = 1.5f;
    private NavMeshAgent agent;

    private GameObject enemy;
    private GameObject player;

    private Transform playerPos;
    private Transform enemyPos;

    private Enemy enemyScript;

    private PlayerSword playerSword;

    private float distance;

    private bool playerIsAttacking;

    private const string MOVE = "Moving";
    private const string ATTACK = "Attack";
    private const string BLOCK = "Block";
    private const string HIT = "Hit";
 
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.ResetTrigger(HIT);

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
        GameObject sword = GameObject.Find("Sword");

        if (sword == null)
        {
            Debug.Log("Sword is NOT active");
        }

        else
        {
            playerSword = sword.GetComponent<PlayerSword>();

            bool isEnemyHit = playerSword.IsEnemyHit();

            if (isEnemyHit)
            {
                animator.SetTrigger("Hit");
            }
        }

        playerIsAttacking = enemyScript.PlayerAttacking();

        distance = Vector3.Distance(animator.transform.position, player.transform.position);

        if (distance <= attackDistance)
            {
                agent.SetDestination(agent.transform.position);

                animator.SetBool(MOVE, false);

                animator.SetBool(ATTACK, true);

                if(playerIsAttacking == true)
                {
                    animator.SetBool(BLOCK, true);

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
