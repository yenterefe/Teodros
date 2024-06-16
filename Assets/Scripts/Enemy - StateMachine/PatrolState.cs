using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : StateMachineBehaviour
{ 
    private NavMeshAgent agent;

    private GameObject enemy;
    private GameObject player;

    private Transform playerPos;
    private Transform enemyPos;

    private Enemy enemyScript;

    private float distance;

    private bool isPlayerSeen = false;
    private bool isPlayerAttacking;

    private List<Transform> checkpoints = new List<Transform>();

    private const string _MOVE = "Moving";
    private const string _ATTACK = "Attack";
    private const string _BLOCK = "Block";
    private int checkpointNumber = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (checkpointNumber == 5)
        {
            checkpointNumber = 0;
        }

        //enemy = GameObject.Find("Enemy");

        enemy = GameObject.Find("Enemy A");

        player = GameObject.Find("Player");

        //enemyPos = GameObject.Find("Enemy").transform ;

        enemyPos = GameObject.Find("Enemy A").transform;

        playerPos = GameObject.Find("Player").transform;

        enemyScript = enemy.GetComponent<Enemy>();

        Transform checkPointsObject = GameObject.Find("Patrol Destinations").transform;
        
        foreach(Transform t in checkPointsObject)
        {
            checkpoints.Add(t);
        }
        agent = animator.GetComponent<NavMeshAgent>();

        agent.SetDestination(checkpoints[checkpointNumber].position);

        animator.SetBool(_MOVE, false);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isPlayerAttacking = enemyScript.PlayerAttacking();

        Ray ray = new Ray(enemy.transform.position, enemy.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.name=="Player")
            {
                isPlayerSeen = true;

                Debug.Log("Player seen");
            }

            // will delete in the final game
            else
            {
                isPlayerSeen = false;
            }
        }

        if (isPlayerSeen)
        {
            distance = Vector3.Distance(animator.transform.position, player.transform.position);

            //bool playerAimingGun = playerAnimation.IsPlayerAiming();

            //if (playerAimingGun == false)
            if (distance < 2)
            {
                agent.SetDestination(agent.transform.position);

                animator.SetBool(_ATTACK, true);

                if(isPlayerAttacking == true)
                {
                    animator.SetBool(_BLOCK, true);

                }
            }

            else
            {
                agent.SetDestination(player.transform.position);
            }
        }

        else 
        { 
            if((agent.remainingDistance <= agent.stoppingDistance))
            {
                agent.SetDestination(checkpoints[checkpointNumber].position);
                animator.SetBool(_MOVE, false);
            }
            
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        agent.SetDestination(agent.transform.position);
        checkpointNumber++;
    }
}
