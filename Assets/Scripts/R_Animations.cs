using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_Animations : StateMachineBehaviour {

    int idleTimer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTimer = animator.GetInteger("idleTimer");

        animator.SetInteger("idleTimer", idleTimer++);

        //reset timer
        if (idleTimer > 3)
        {
            idleTimer = 0;
        }

        Debug.Log(idleTimer);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("IdleAnimID", idleTimer);
        animator.SetInteger("idleTimer", idleTimer++);

		if ((animator.GetBool("isAttacking")== true) || (animator.GetBool("isWalking")==true))
		{
				animator.SetInteger("idleTimer", 0);
		}

        //Debug.Log(idleTimer);
        //Debug.Log("OnStateMachineExit");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void onstatemove(animator animator, animatorstateinfo stateinfo, int layerindex)
    //{

    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}


}
