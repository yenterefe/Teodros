using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class IdleState : StateMachineBehaviour
{
    private float timer;
    private float distance;

    private GameObject enemy;
    private GameObject player;

    private NavMeshAgent agent;

    private bool playerSeen=false;

    private const string _MOVE = "Moving";
    
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");

        agent= animator.GetComponent<NavMeshAgent>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Ray ray = new Ray(enemy.transform.position, enemy.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.name =="Player")
            {
                playerSeen = true;
            }

            else
            {
                playerSeen = false;

            }
        }

        if (playerSeen == true)
        {
            animator.SetBool(_MOVE, true);
        }

        else
        {
            timer += Time.deltaTime;
            if (timer > 5f)
            {
                animator.SetBool(_MOVE, true);
            }
        }

        //Debug.Log("Is player seen: " + playerSeen);
    }
}
