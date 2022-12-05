using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGG.Combat;
public class ChangeCombo : StateMachineBehaviour
{
    private AICombatSystem _combatSystem;
    [SerializeField]private string ChangeName;
    [SerializeField]private float ChangeTime;
    [SerializeField]private bool CanChange;
    [SerializeField]private bool ComboChangeActive;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      CanChange=false;
      ComboChangeActive=false;
      if(_combatSystem==null)
      _combatSystem=animator.GetComponentInChildren<AICombatSystem>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    
        if(_combatSystem==null)
        {
       
            return;
        }
        if(_combatSystem.GetCurrentTarget()==null)
        {
  
            return;
        }
       CheckChangeTime(animator);
       ChangeComboAction(animator);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    // override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
      
    // }
   private void CheckChangeTime(Animator animator)
   {

 if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime<ChangeTime)
     {
       CanChange=true;
     }  
     else
     {
      CanChange=false;
     } 
   }
   private void ChangeComboAction(Animator animator)
   {
    if(CanChange)
    {
        if(_combatSystem.GetCurrentTargetDistance()<1.8f)
        {
          ComboChangeActive=true;
        }
    }
    if(!CanChange&&ComboChangeActive)
    {
  
        animator.CrossFade(ChangeName,0f,0,0f);
    }
   }
    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
