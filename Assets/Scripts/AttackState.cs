using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackState : StateMachineBehaviour
{
    private Transform playerPos;

    private const string _MOVE = "Moving";
    private const string _ATTACK = "Attack";
    private const string _BLOCK = "Block";

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.Find("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(playerPos);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_ATTACK, false);   
        animator.SetBool(_MOVE, true);
    }
}
