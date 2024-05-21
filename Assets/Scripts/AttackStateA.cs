using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class AttackStateA : StateMachineBehaviour
{
    private Transform playerPos;

    private GameObject enemy;

    private float distance;

    private float counter = 0;

    private const string MOVE = "Moving";
    private const string IDLE = "idle";
    private const string ATTACK = "Attack";
    private const string SPECIAL_ATTACK = "Special Attack";
    private const string ATTACK_TWO = "Attack_2";
  

    private NavMeshAgent agent;

    private float activateSuperAttack;

    float attackDistance = 1.25f; 

    private bool isEnemyAttacking=false;

    private float timer =0f;
 

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.SetBool(SPECIAL_ATTACK, false);

        animator.SetBool(IDLE, false);

        timer = 0;

        playerPos = GameObject.Find("Player").transform;

        enemy = GameObject.Find("Enemy A");

        agent = enemy.GetComponent<NavMeshAgent>(); 
        
        activateSuperAttack = Random.Range(2, 7f);

        counter++;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        distance = Vector3.Distance(animator.transform.position, playerPos.position);   

        animator.transform.LookAt(playerPos);

        timer += Time.deltaTime;

        isEnemyAttacking = true;

        /*if (timer > activateSuperAttack)
        {
            animator.SetBool(SPECIAL_ATTACK, true);
        }*/

        if(distance> 2f)
        {
            animator.SetBool(MOVE, true);
        }

        if(timer > 0.25f && counter %3 != 0)
        {
            animator.SetBool(ATTACK_TWO,true);
        }

        if(counter%3 == 0)
        {
            animator.SetBool(SPECIAL_ATTACK, true);
            counter = 0;
        }

       Debug.Log($"distance is equal to {distance}, counter is equal to {counter}, and counter divided by three is {counter/3}");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(ATTACK, false);
        isEnemyAttacking = false;
    }

    public bool EnemyAttacking()
    {
        return isEnemyAttacking;
    }
}
