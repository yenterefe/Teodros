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

    private bool isEnemyHit = false;

    private PlayerSword playerSword;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject sword = GameObject.Find("Sword");

        if (sword == null)
        {
            Debug.Log("Sword is NOT active");
            isEnemyHit = false;
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

        timer += Time.deltaTime;

        if(timer > attackTime && !isEnemyHit)
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
