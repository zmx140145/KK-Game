using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGG.Move;
[CreateAssetMenu(fileName ="AI_Combat",menuName ="StateMachine/State/AI_Combat")]
public class AI_Combat : StateActionSO
{
    private int randomHorizontal;
    private float UpdateTime=0.5f;
    private float time=5f;
    public override void OnUpdate()
    {
        if(time>UpdateTime)
        {
            randomHorizontal=GetRandomHorizontal();
            time=0f;
        }
        time+=Time.deltaTime;
      AICombatAction();
    }
   
   [SerializeField]private CombatAbilityBase currentAbility;
   private void AICombatAction()
   {
    if(currentAbility==null)
    {

       //如果当前没有技能则防守
        NoCombat();
        GetAbility();
    }
    else
    {
         //如果有技能就使用技能
           currentAbility.InvokeAbility();
        if(!currentAbility.GetAbilityIsDone())
        {

        currentAbility=null;
        }
        
    }
   }


   private void GetAbility()
   {
    if(currentAbility==null)
    {
        currentAbility=_combatSystem.GetVaildAbility();
    }
   }
    private void NoCombat()
    { 
         //不能攻击就逃跑
if(_animator.CheckAnimationTag("Motion"))
{

    if(_combatSystem.GetCurrentTargetDistance()<4.0f-0.1f)
    {
          if(_combatSystem.GetCurrentTargetDistance()<1.5f+0.1f)
         {
           bool? isRun= _combatSystem.GetCurrentTarget()?.GetComponent<PlayerMovementController>()?.IsRun();
           if(isRun==true)
           {
            _animator.Play("Roll_B",0,0f);
           }
           else
           {
            _animator.Play("Attack_1",0,0f);
           }
           
            
         }
        //往后退，不退就挨打
      
       _animator.SetFloat(runID,0f,0.23f,Time.deltaTime);
        _movement.CharacterMoveInterface(-_combatSystem.GetDirectionForTarget(),1.5f,true);
        _animator.SetFloat(verticalID,-1f,0.23f,Time.deltaTime);
         _animator.SetFloat(horizontalID,0f,0.1f,Time.deltaTime);
     
    }
   else
   {
 if(_combatSystem.GetCurrentTargetDistance()>4.3f+0.1f)
    {
     
        if(_combatSystem.GetCurrentTargetDistance()>8f+0.1f)
        {
              //太远就加速跑
_movement.CharacterMoveInterface(_combatSystem.GetDirectionForTarget(),5f,true);
_animator.SetFloat(runID,1f,0.23f,Time.deltaTime);
        _animator.SetFloat(verticalID,2f,0.23f,Time.deltaTime);
         _animator.SetFloat(horizontalID,0f,0.1f,Time.deltaTime);
        }
        else
        {
            //差不多距离就慢慢走过去
 _movement.CharacterMoveInterface(_combatSystem.GetDirectionForTarget(),1.5f,true);
        _animator.SetFloat(verticalID,1f,0.23f,Time.deltaTime);
         _animator.SetFloat(horizontalID,0f,0.1f,Time.deltaTime);
         _animator.SetFloat(runID,0f,0.23f,Time.deltaTime);
        }
         
    }
    else
    {
        //这是在中间距离时
        _movement.CharacterMoveInterface(_movement.transform.right*(randomHorizontal==0?1f:randomHorizontal),randomHorizontal==0?0f:1.5f,true);
        _animator.SetFloat(verticalID,0,0.1f,Time.deltaTime);
         _animator.SetFloat(horizontalID,randomHorizontal,0.23f,Time.deltaTime);
         _animator.SetFloat(runID,0f,0.23f,Time.deltaTime);
    }
   }
   
 
    
}
    }
    private int GetRandomHorizontal()=>Random.Range(-1,2);
}
