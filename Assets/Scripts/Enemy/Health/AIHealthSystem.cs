using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace UGG.Health
{
    public class AIHealthSystem : CharacterHealthSystemBase
    {

       private void LateUpdate() {
        OnHitLockTarget();
       }
        public override void TakeDamager(float damagar, string hitAnimationName, Transform attacker)
        {
            //设置攻击者
               SetAttacker(attacker);
             Debug.Log(String.Format("攻击的名字{0}",hitAnimationName));
            _animator.Play(hitAnimationName,0,0f);
            GameAssets.Instance.PlaySoundEffect(_audioSource,SoundAssetsType.hit);
             
        }
        private void OnHitLockTarget()
        {
            if(_animator.CheckAnimationTag("Hit"))
            {

                //要转向攻击者
             transform.rotation=transform.LockOnTarget(currentAttacker,transform,50f);
            }
        }
    }

}