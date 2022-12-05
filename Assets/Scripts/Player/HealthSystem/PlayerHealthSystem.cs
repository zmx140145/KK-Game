using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace UGG.Health
{
    public class PlayerHealthSystem : CharacterHealthSystemBase
    {
         protected CharacterInputSystem _inputSystem;
         private void Start() {
            _inputSystem=GetComponent<CharacterInputSystem>();
         }
         protected override void Update() {
         
        }
         private void LateUpdate() {
        OnHitLockTarget();
       }
       #region Hit
        public override void TakeDamager(float damagar, string hitAnimationName, Transform attacker)
        {
            //设置攻击者
               SetAttacker(attacker);
             Debug.Log(String.Format("攻击的名字{0}",hitAnimationName));
             if(CanParry())
             {
               
             _combatSystem.ui_Health.Health-=damagar/10f;
            Parry(hitAnimationName);

             }
             else
             {
    attacker.gameObject.GetComponentInChildren<UGG.Combat.CharacterCombatSystemBase>().ui_Health.Blue+=damagar*2f;
    _combatSystem.ui_Health.Health-=damagar;
  _animator.Play(hitAnimationName,0,0f);
            GameAssets.Instance.PlaySoundEffect(_audioSource,SoundAssetsType.hit);
             
             }
          
        }



        private bool CanHitLockAttacker()
        {
            return true;
        }
        private void OnHitLockTarget()
        {
            if(_animator.CheckAnimationTag("Hit")||_animator.CheckAnimationTag("ParryHit"))
            {

                //要转向攻击者
             transform.rotation=transform.LockOnTarget(currentAttacker,transform,50f);
            }
        }
        #endregion
        #region  Parry

        private bool CanParry()
        {
            
            if(_animator.GetBool(parryID)&&(_animator.CheckCurrentTagAnimationIsOverTime("Hit",0.1f)||_animator.CheckAnimationTag("Parry")||_animator.CheckAnimationTag("ParryHit")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Parry(string hitName)
        {
            switch(hitName)
            {
                case "Hit_D_Up":
                  _animator.Play("Parry_F",0,0f);
                 break; 
                case "Hit_H_Right":
                  _animator.Play("Parry_HR",0,0f);
                  break;
                  case "Hit_H_Left":
                    _animator.Play("Parry_HL",0,0f);
                  break;
                  case "Hit_Up_Right":
                   _animator.Play("Parry_UR",0,0f);
                  break;
                  default:
                 _animator.Play("Parry_F",0,0f);
                break;
            }
             GameAssets.
             Instance.PlaySoundEffect(_audioSource,SoundAssetsType.defence);
        }

        #endregion

        
    }
    
}

