using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class IdleStateA : StateMachineBehaviour
{
    private float timer;
    private float distance;
    private const float Move_DISTANCE = 2f;

    private GameObject enemy;
    private GameObject player;

    private Transform playerPos;

    private NavMeshAgent agent;

    private const string MOVE = "Moving";
    private const string ATTACK = "Attack";
    
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;

        enemy = GameObject.Find("Enemy A");

        player = GameObject.Find("Player");

        playerPos = GameObject.Find("Player").transform;

        agent = enemy.GetComponent<NavMeshAgent>();

        distance = Vector3.Distance(animator.transform.position, playerPos.position);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(distance);

        timer += Time.deltaTime;

        if (distance > Move_DISTANCE)
        {
            animator.SetBool(MOVE, true);
           
        }

        if(distance<Move_DISTANCE)
        {
            animator.SetBool(ATTACK, true);
        }
    }
}
