using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AttackStateA : StateMachineBehaviour
{
    private Transform playerPos;

    private float distance;

    private const string _MOVE = "Moving";
    private const string _ATTACK = "Attack";
    private const string _SPECIALATTACK = "Special Attack";
    private const string _JUMPBACK = "Run Back";

    private float activateSuperAttack;

    private bool enemyIsAttacking=false;

    private float timer =0f;
 

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_SPECIALATTACK, false);

        timer = 0;

        playerPos = GameObject.Find("Player").transform;
        
        activateSuperAttack = Random.Range(2, 7f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        distance = Vector3.Distance(animator.transform.position, playerPos.position);   

        enemyIsAttacking = true;

        animator.transform.LookAt(playerPos);

        timer += Time.deltaTime;

        if (timer > activateSuperAttack)
        {
            animator.SetBool(_SPECIALATTACK, true);
        }

        if (distance < 2f )
        {
            animator.SetBool(_JUMPBACK, true);
        }

        Debug.Log(distance);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_ATTACK, false);
        enemyIsAttacking = false;

        //animator.SetBool(_MOVE, true);

        // Delete this if it's not working 
        if (distance< 1f)
        {
            animator.SetBool(_JUMPBACK, true);
        }
        
       
    }

    public bool EnemyAttacking()
    {
        return enemyIsAttacking;
    }
}
