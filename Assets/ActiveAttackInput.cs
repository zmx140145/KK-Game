using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGG.Combat;
public class ActiveAttackInput : StateMachineBehaviour
{
    private PlayerCombatSystem combatSystem;
[SerializeField] private float maxAllowAttackTime;
private float currentAllowTime;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //获得玩家攻击系统
       if(combatSystem==null)
       {
            combatSystem=animator.GetComponent<PlayerCombatSystem>();
           
       }
 currentAllowTime=maxAllowAttackTime;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      

        
    
        if(!combatSystem.GetAllowAttackInput())
        {
       if(currentAllowTime>0)
       {
        if(currentAllowTime<animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
currentAllowTime=0;
        }
        
       }
       else
       {
        combatSystem.SetAllowAttackInput(true);
       }
        }
      
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
