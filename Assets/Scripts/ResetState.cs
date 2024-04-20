using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetState : StateMachineBehaviour
{
    private Transform player;
    private float timer;
    private const float STOP_TIMER = 1.5f;
    private const string IDLE = "Idle";
   
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      timer += Time.deltaTime;
      
      if (timer > STOP_TIMER)
        {
            animator.SetBool(IDLE, true);
        }


        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        //animator.SetBool(_IDLE, true);
    }
}
