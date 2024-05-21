using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2State : StateMachineBehaviour
{

    private const string MOVE = "Moving";

    private const string ATTACK = "Attack";

    private float attackTime = 1f;

    private float timer;

    private bool isEnemyAttacking = false;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;

        isEnemyAttacking = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if(timer > attackTime)
        {
            animator.SetBool(ATTACK, true);
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isEnemyAttacking = false;
    }

    public bool IsEnemyAttacking()
    {
        return isEnemyAttacking;
    }
}
