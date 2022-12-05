using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGG.Combat;
public class AICombatSystem : CharacterCombatSystemBase
{
  
   public float maxMoveValue;
   public float minMoveValue;
   public bool isLimitMoveValue;
   public float MoveValue
   {
      get
      {
     if(isLimitMoveValue)
      {
         return Mathf.Clamp(_animator.GetFloat(animationMoveID),minMoveValue,maxMoveValue);
      }
      else
      {
         return _animator.GetFloat(animationMoveID);
      }
      }
      
   }
   [SerializeField,Header("AI移动能力调整")] private float attackMoveMult;
   [SerializeField] private float rollMoveMult;
    [SerializeField] private float hurtMoveMult;
   [SerializeField,Header("检测范围中心")] private Transform detectionCenter;
   [SerializeField] private float detectionRang;
   [SerializeField] private LayerMask whatisEnemy;
   [SerializeField] private LayerMask whatisBos;
   Collider[] colliderTarget=new Collider[1];
   
   [SerializeField,Header("技能搭配")] private List<CombatAbilityBase> abilitys=new List<CombatAbilityBase>();


   //AnimationID
private void Start() {
   InitAllAbility();
}
   private void Update() {
    AIView();
    LockOnTarget();
    UpdateAnimationMove();
   }
private void AIView()
{
int targetCount=Physics.OverlapSphereNonAlloc(detectionCenter.position,detectionRang,colliderTarget,whatisEnemy);
if(targetCount>0)
{
    if(!Physics.Raycast(transform.root.position+transform.root.up*0.5f,(colliderTarget[0].transform.position-transform.root.position).normalized,out var hit,detectionRang,whatisBos))
    {
       if(Vector3.Dot((colliderTarget[0].transform.position-transform.root.position).normalized,transform.root.forward)>0.35f)
       {
        currentTarget=colliderTarget[0].transform;
       }
    }
}
}

//锁定LockOn
private void LockOnTarget()
{
   if(!GetCurrentTarget())
   {
       _animator.SetFloat(lockOnID,0f);
      return;
   }
   if(_animator.CheckAnimationTag("Motion")||_animator.CheckAnimationTag("Attack"))
   {
       //要转向攻击者
   transform.root.rotation=transform.LockOnTarget(currentTarget,transform.root.transform,50f);
      _animator.SetFloat(lockOnID,1f);
   }
   else
   {
      _animator.SetFloat(lockOnID,0f);
   }
}


private void UpdateAnimationMove()
{
   if(_animator.CheckAnimationTag("Hit"))
   {
      _characterMovementBase.CharacterMoveInterface(transform.root.forward,MoveValue*hurtMoveMult,true);
   }
   if(_animator.CheckAnimationTag("Roll"))
   {
      _characterMovementBase.CharacterMoveInterface(transform.root.forward,MoveValue*rollMoveMult,true);
   }
   if(_animator.CheckAnimationTag("Attack"))
   {
      
       _characterMovementBase.CharacterMoveInterface(transform.root.forward,MoveValue*attackMoveMult,true);
   }
}



#region 技能

private void InitAllAbility()
{
   if(abilitys.Count==0)return;
   for(int i=0;i<abilitys.Count;i++)
   {
      abilitys[i].InitAbility(_animator,this,_characterMovementBase);


      if(!abilitys[i].GetAbilityIsDone())
      {
         //如果当前技能不可用则重置
         abilitys[i].ResetAbility();
      }
   }
}

public CombatAbilityBase GetVaildAbility()
{
   for(int i=0;i<abilitys.Count;i++)
   {
      if(abilitys[i].GetAbilityIsDone())
      {
         return abilitys[i];
      }
      else
      {
         continue;
      }
      
   }
   return null;
}


public CombatAbilityBase GetAbilityByName(string name)
{
for(int i=0;i<abilitys.Count;i++)
   {
      if(abilitys[i].GetAbilityName().Equals(name))
      {
         return abilitys[i];
      }
      else
      {
         continue;
      }
      
   }
   return null;
}



public CombatAbilityBase GetAbilityByID(int id)
{
for(int i=0;i<abilitys.Count;i++)
   {
      if(abilitys[i].GetAbilityID()==id)
      {
         return abilitys[i];
      }
      else
      {
         continue;
      }
      
   }
   return null;
}
#endregion
}
