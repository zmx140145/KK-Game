using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGG.Move;
using UGG.Combat;
public class RollSkill : StateMachineBehaviour
{
    [SerializeField]private GameObject Eff;
    [SerializeField]private float RollTime;
        [SerializeField,Header("Eff开始时间")]private float effBeginTime;
          [SerializeField,Header("特效校准距离")]private float EffDistance;

          
          [SerializeField,Header("特效安全距离")]private float EffSaveDistance;
    [SerializeField,Header("延迟开始时间")]private float relayBeginTime;
    [SerializeField,Header("瞬移的距离")]private float RollDistance;
    [SerializeField,Header("瞬移预留距离")]private float RollSaveDistance;
     private CharacterMovementBase _characterMovement;
     private CharacterCombatSystemBase _characterCombat;
    private bool isEff;
    private bool isRoll;
    private bool NoRelay;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isEff=false;
         isRoll=false;
         NoRelay=false;
       if(_characterMovement==null)
       {
        _characterMovement=animator.gameObject.GetComponentInParent<CharacterMovementBase>();
       }
       if(_characterCombat==null)
       {
        _characterCombat=animator.GetComponent<CharacterCombatSystemBase>();
       }
       
    }
   
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_characterCombat==null)
        {
            return;
        }
        if(_characterMovement==null)
        {
            return;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime>relayBeginTime)
        {
          if(!NoRelay)
          {
            GameObjectPoolSystem.Instance.RelayTime=new GameObjectPoolSystem.Relay(0.5f,0.1f);
            GameObjectPoolSystem.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(0.1f,()=>{GameObjectPoolSystem.Instance.RemoveRelayTime();},false);
             NoRelay=true;
          }
         
        }
           if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime>effBeginTime)
           {
           if(!isEff)
               {
                 float enemyDistnace=float.MaxValue;
                if(_characterCombat.GetCurrentTarget()!=null)
                {
                enemyDistnace=_characterCombat.GetCurrentTargetDistance();
                 }
                 if(enemyDistnace>EffSaveDistance)
                 {
                Vector3 v3=_characterMovement.transform.root.position+_characterMovement.transform.forward*EffDistance;
                 v3.y=0f;
                 Instantiate(Eff,v3,animator.gameObject.transform.rotation);
                 }
               
                 isEff=true;
               }
           }
       if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime>RollTime)
       {
          
    if(!isRoll)
    {
     Roll(animator);
     isRoll=true;
    }
        
       }
       
    }
   private void Roll(Animator animator)
   {
    
    
    float enemyDistnace=float.MaxValue;
    if(_characterCombat.GetCurrentTarget()!=null)
    {
      enemyDistnace=_characterCombat.GetCurrentTargetDistance();
    }
   
    if(enemyDistnace-RollSaveDistance<RollDistance)
    {
        Debug.Log("向前1");
        _characterMovement.control.Move(_characterMovement.transform.forward*(Mathf.Clamp(enemyDistnace-RollSaveDistance,0f,float.MaxValue)));
    
    }
    else
    {
        Debug.Log("向前2");
      _characterMovement.control.Move(_characterMovement.transform.forward*RollDistance);
    }
    
   }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
