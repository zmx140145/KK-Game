using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UGG.Combat
{
    public class PlayerCombatSystem : CharacterCombatSystemBase
    {
 
        //记录跟踪的目标
      
      
        //检测是否进入暴走模式
        bool isStrong=false;
        //允许攻击输入
        private bool AllowAttackInput;
        //Speed
        [SerializeField, Header("攻击移动速度倍率"), Range(.1f, 10f)]
        private float attackMoveMult;
        [SerializeField]private float hurtMoveMult;
        //检测
        [SerializeField, Header("检测敌人")] private Transform detectionCenter;
        [SerializeField] private float detectionRang;

        //缓存
        private Collider[] detectionedTarget = new Collider[1];
        
        private void Update()
        {
           
            PlayerAttackAction();
            DetectionTarget();
            ActionMotion();
            HurtMotion();
            UpdateCurrentTarget();
            PlayerDefenceAction();
        }
      private void LateUpdate() {
OnAttackActionAutoLockON();
        
      }

/// <summary>
/// 玩家防守输入
/// </summary>
    private void PlayerDefenceAction()
    {
if(AllowDefence())
{
   
  _animator.SetBool(defenID,_characterInputSystem.playerDefen);

}
else
{
     _animator.SetBool(defenID,false);
}
    }
    private bool AllowDefence()
    {
        if(_animator.CheckAnimationTag("Motion")||_animator.CheckAnimationTag("Parry")||_animator.CheckAnimationTag("ParryHit"))
        {
          return true;
        }
        else
        {
            return false;
        }
    }


      /// <summary>
      /// 玩家攻击输入
      /// </summary>
     
        private void PlayerAttackAction()
        {

        
  //当玩家在Motion状态时默认允许输入
if(!_animator.CheckAnimationTag("Attack"))
{
    SetAllowAttackInput(true);
}
if(CanExecute&&_characterInputSystem.playerLAtk)
{
    
         CanExecute=false;
         _animator.SetTrigger(executeID);
         return;
}
         if(AllowAttackInput&&!CanExecute)
         {
       
            if(_characterInputSystem.playerRAtk&&ui_Health.Blue>=100f)
            {
        ui_Health.UseBlue();
        isStrong=true;
        GameObjectPoolSystem.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(15f,()=>{isStrong=false; _animator.SetBool(sWeaponID,false);},false);
        _animator.SetBool(sWeaponID,isStrong);
            }
           
            if (_characterInputSystem.playerLAtk)
            {
               _animator.SetTrigger(lAtkID);
               SetAllowAttackInput(false);
            }
         }
       
            
           
            
        }


private void OnAttackActionAutoLockON()
{
    //如果允许锁定
    if(CanAttackLockOn())
    {
 //如果在播放攻击动画
    if(_animator.CheckAnimationTag("Attack")||_animator.CheckAnimationTag("GSAttack"))
    {
        transform.root.rotation=transform.LockOnTarget(currentTarget,transform.root.transform,50f);
    }
    }
   
}



        private void ActionMotion()
        {
            if (_animator.CheckAnimationTag("Attack")||_animator.CheckAnimationTag("GSAttack"))
            {
                _characterMovementBase.CharacterMoveInterface(transform.forward,_animator.GetFloat(animationMoveID) * attackMoveMult,true);
            }
        }
        private void HurtMotion()
        {
            if(_animator.CheckAnimationTag("Hit"))
            {
                 _characterMovementBase.CharacterMoveInterface(transform.forward,_animator.GetFloat(animationMoveID) * hurtMoveMult,true);
            }
        }
        #region 动作检测
        
        /// <summary>
        /// 攻击状态是否允许自动锁定敌人
        /// </summary>
        /// <returns></returns>
        private bool CanAttackLockOn()
        {
            if (_animator.CheckAnimationTag("Attack")||_animator.CheckAnimationTag("GSAttack"))
            {
                if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.75f)
                {
                    return true;
                }
            }
            return false;
        }


        private void DetectionTarget()
        {
            int targetCount = Physics.OverlapSphereNonAlloc(detectionCenter.position, detectionRang, detectionedTarget,
                enemyLayer);
            
            //后续功能补充
            if(targetCount>0)
            {
                SetCurrentTarget(detectionedTarget[0].transform);
            }
        }
        private void SetCurrentTarget(Transform target)
        {
            if(currentTarget==null||currentTarget!=target)
            {
                currentTarget=target;
            }
        }
        private void UpdateCurrentTarget()
        {
            if(currentTarget!=null)
            {
               _animator.SetFloat(lockOnID,1f,0.25f,Time.deltaTime);
            }
            else
            {
                _animator.SetFloat(lockOnID,0f,0.01f,Time.deltaTime);
                return;
            }
            if(_animator.CheckAnimationTag("Motion"))
            {
                if(GetCurrentTargetDistance()>detectionRang+0.1f)
                {
                    currentTarget=null;
                }
            }
        }
        //特效重写 因为只有player攻击时有特效
        protected override void OnAnimationAttackParticleEvent(int angle)
        {
              Script_ATK obj= GameObject.Find("Player").GetComponent<Script_ATK>();
            obj.SetRotationAndPlay("Attack",angle,0f);
            base.OnAnimationAttackParticleEvent(angle);
        }
        private void SetState()
        {

        }

        /// <summary>
        /// 获取当前是否允许输入攻击信号
        /// </summary>
        /// <returns></returns>
        public bool GetAllowAttackInput()=>AllowAttackInput;
        /// <summary>
        /// 设置当前是否允许输入攻击信号
        /// </summary>
        /// <param name="allow"></param>
        public void SetAllowAttackInput(bool allow)
        {
AllowAttackInput=allow;
        }
        #endregion
    }
}

