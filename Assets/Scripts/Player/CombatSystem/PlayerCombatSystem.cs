using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UGG.Combat
{
    public class PlayerCombatSystem : CharacterCombatSystemBase
    {
        //记录跟踪的目标
        [SerializeField] private Transform currentTarget;
        //检测是否进入暴走模式
        bool isStrong=false;
        //Speed
        [SerializeField, Header("攻击移动速度倍率"), Range(.1f, 10f)]
        private float attackMoveMult;
        
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
            UpdateCurrentTarget();
        }
      private void LateUpdate() {
OnAttackActionAutoLockON();
        
      }
        private void PlayerAttackAction()
        {
            if(_characterInputSystem.playerRAtk)
            {
isStrong=(isStrong)?false:true;
_animator.SetBool(sWeaponID,isStrong);
            }
           
            if (_characterInputSystem.playerLAtk)
            {
               
               _animator.SetTrigger(lAtkID);
             
            
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
            if(_animator.CheckAnimationTag("Motion"))
            {
                if(_characterInputSystem.playerMovement.sqrMagnitude>0)
                {
                    currentTarget=null;
                }
            }
        }
        private void SetState()
        {

        }
        #endregion
    }
}

