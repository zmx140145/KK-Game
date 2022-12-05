using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGG.Combat;
public class AnimationFloatVarClamp : StateMachineBehaviour
{
  private AICombatSystem combatSystem;
    [SerializeField,Header("最大值")]private float max;
    [SerializeField,Header("最小值")]private float min;
    [SerializeField,Header("生效时间,两个Min,Max")]private float[] activeTime;

    [SerializeField]private float distance;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      if(combatSystem==null)
       combatSystem=animator.GetComponentInChildren<AICombatSystem>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    private bool CanLimit()
    {
     
     return combatSystem.GetCurrentTargetDistance()<distance;
    }
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(combatSystem==null)
      return;
       if(combatSystem.GetCurrentTarget()==null)
      return;
      if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime>activeTime[0]&&animator.GetCurrentAnimatorStateInfo(0).normalizedTime<activeTime[1]&&CanLimit())
      {
       combatSystem.minMoveValue=min;
       combatSystem.maxMoveValue=max;
       combatSystem.isLimitMoveValue=true;
      }
      else
      {
          combatSystem.isLimitMoveValue=false;
      }
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
