using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace UGG.Health
{
    public class AIHealthSystem : CharacterHealthSystemBase
    {
        [SerializeField]private bool isInExecuteTime=false;
   [SerializeField,Header("统计累积被连招击中")] private int hurtCount;
   [SerializeField,Header("AI防守行为调整")] private int maxParryCount;
   [SerializeField] private int backAttackParryCount;//当格挡次数大于自己设置的值会自动触发一次反击动作
   [SerializeField] private int maxHitCount;
   [SerializeField] private int hitCount;//不能让AI一直被打，如果受伤次数超过了自己设置的那个值就脱身

   private void Start() {
    hitCount=0;
   }
       private void LateUpdate() {
        OnHitLockTarget();
       }
        public override void TakeDamager(float damagar, string hitAnimationName, Transform attacker)
        {
            //设置攻击者
               SetAttacker(attacker);
        switch(CanParry())
        {
            case 0:
           
             OnHit(damagar,hitAnimationName,attacker);
             break;
             
            case 1://格挡
            
               hurtCount=0;
            if(backAttackParryCount==2)
            {
                //触发一次反击
                _animator.Play("BackAttack",0,0f);
                GameAssets.Instance.PlaySoundEffect(_audioSource,SoundAssetsType.defence);
                backAttackParryCount=0;
            }
            else
            {
             OnParry(hitAnimationName);
             backAttackParryCount++;
            }
              attacker.gameObject.GetComponentInChildren<UGG.Combat.CharacterCombatSystemBase>().ui_Health.Blue+=damagar*2f;
             _combatSystem.ui_Health.Health-=damagar/10f;
            break; 

            case 2:
            //脱身
            hurtCount=0;
             
            _animator.Play("Avoid_B",0,0);
          
            break;
        }
      
             
        }
        private void OnHit(float damagar,string hitAnimationName,Transform attacker)
        {
            //正常受伤
            if(!hitAnimationName.Contains("Executed"))
            {

                GameObjectPoolSystem.Instance.RelayTime=new GameObjectPoolSystem.Relay(0.5f,0.05f);
           GameObjectPoolSystem.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(0.05f,()=>{GameObjectPoolSystem.Instance.RemoveRelayTime();},false);

             hurtCount++;
             _animator.Play(hitAnimationName,0,0f);
             GameAssets.Instance.PlaySoundEffect(_audioSource,SoundAssetsType.hit);
             attacker.gameObject.GetComponentInChildren<UGG.Combat.CharacterCombatSystemBase>().ui_Health.Blue+=damagar*2f;
             _combatSystem.ui_Health.Health-=damagar;
           
             if(hurtCount>4)
             {
                      hurtCount=0;
                  hitCount=0;
                 OnExecute(attacker);
           
             }
            }
            else
            {
                //被处决
                   _animator.Play("Executed_0",0,0f);
                   _combatSystem.ui_Health.Health=0f;
                   isInExecuteTime=false;
                   Time.timeScale=1f;
            }
             
             
        }
        public override void OnExecute(Transform attacker)
        {
           isInExecuteTime=true;
            attacker.gameObject.GetComponentInChildren<UGG.Combat.CharacterCombatSystemBase>().CanExecute=true;
           
           GameObjectPoolSystem.Instance.RelayTime=new GameObjectPoolSystem.Relay(0.25f,0.25f);
           GameObjectPoolSystem.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(0.25f,()=>{isInExecuteTime=false; attacker.gameObject.GetComponentInChildren<UGG.Combat.CharacterCombatSystemBase>().CanExecute=false;GameObjectPoolSystem.Instance.RemoveRelayTime();},false);
           
        }
        private void OnHitLockTarget()
        {
            if(_animator.CheckAnimationTag("Hit"))
            {

                //要转向攻击者
             transform.rotation=transform.LockOnTarget(currentAttacker,transform,50f);
            }
        }
        
        private int CanParry()//0无防守 //1防守 //2脱身
        {
            if(maxParryCount>0)
            {
                maxParryCount--;
               return 1;
            
            }
            else
            {
         if(hitCount==maxHitCount)
              {
                //触发脱身
                hitCount=0;
                maxParryCount+=UnityEngine.Random.Range(1,4);
                return 2;
              }
            else
            {
                hitCount++;
                return 0;
            }
            }
            
        }

        private void OnParry(string hitName)
        {
       switch(hitName)
       {
        default:
            _animator.Play("Parry_F",0,0f);
        break;
           
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

           


       }
      GameAssets.Instance.PlaySoundEffect(_audioSource,SoundAssetsType.defence);


        }

    }

}