using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackState : StateMachineBehaviour
{
    private PlayerSword playerSword; 

    private float timer = 0;
    private bool specialAttack = false;

    private const string IDLE = "Idle";
    private const string SPECIAL_ATTACK = "Special Attack";
    private const string ATTACK = "Attack";

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(SPECIAL_ATTACK, false);
        animator.SetBool(IDLE, true);
        specialAttack = true;
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
            //Debug.Log("Sword is active");
            playerSword = sword.GetComponent<PlayerSword>();

            bool isEnemyHit = playerSword.IsEnemyHit();

            if (isEnemyHit)
            {
                animator.SetTrigger("Hit");
            }
        }
        timer += Time.deltaTime;

        if(timer>0.25f)
        {
            animator.SetBool(ATTACK, true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        specialAttack = false;
    }

  
    public bool IsSpecialAttackActive()
    {
        return specialAttack;
    }
}
