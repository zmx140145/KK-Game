using System;
using System.Collections;
using System.Collections.Generic;
using UGG.Move;
using UnityEngine;

namespace UGG.Combat
{
    public abstract class CharacterCombatSystemBase : MonoBehaviour
    {

          [SerializeField] protected Transform currentTarget;
        private bool canExecute;
        //设置是否可以被处决
        public bool CanExecute
        {
      get
      {
        return canExecute;
      }
      set
      {
        canExecute=value;
      }
        }
        public UI_Health ui_Health;
        protected Animator _animator;
        protected CharacterInputSystem _characterInputSystem;
        protected CharacterMovementBase _characterMovementBase;
        protected AudioSource _audioSource;
        
        
        //aniamtionID
        protected int executeID=Animator.StringToHash("Execute");
                protected int lockOnID = Animator.StringToHash("LockOn");
        protected int lAtkID = Animator.StringToHash("LAtk");
        protected int rAtkID = Animator.StringToHash("RAtk");
        protected int defenID = Animator.StringToHash("Parry");
        protected int animationMoveID = Animator.StringToHash("AnimationMove");
        protected int sWeaponID=Animator.StringToHash("SWeapon");
        //攻击检测
        [SerializeField, Header("攻击检测")] protected Transform attackDetectionCenter;
        [SerializeField] protected float attackDetectionRang;
        [SerializeField] protected LayerMask enemyLayer;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _characterInputSystem = GetComponentInParent<CharacterInputSystem>();
            _characterMovementBase = GetComponentInParent<CharacterMovementBase>();
            _audioSource = _characterMovementBase.GetComponentInChildren<AudioSource>();
        }
public Transform GetCurrentTarget()
{
   if(currentTarget==null)
   {
      return null;
   }
   return currentTarget;
}

public float GetCurrentTargetDistance()
{
    if(currentTarget)
    {
    return Vector3.Distance(currentTarget.position,transform.root.position);
    }
    else{
        Debug.LogWarning("空对象");
        return 0f;
        
    }
}
public Vector3 GetDirectionForTarget()
{
    if(currentTarget)
    {
    return(currentTarget.position-transform.root.position).normalized;
    }
    else
    {
         Debug.LogWarning("空对象");
        return Vector3.zero;
    }
   
}
        protected virtual void OnAnimationAttackParticleEvent(int angle)
        {
          
        }
       


        /// <summary>
        /// 攻击动画攻击检测事件
        /// </summary>
        /// <param name="hitName">传递受伤动画名</param>
        protected virtual void OnAnimationAttackEvent(string hitName)
        {
            //gameObject.Find("Player").SendMessage(playKnifeLight);
            // if(hitName == "Hit_Up_Left"  )
            //     GameObject.Find("Player").GetComponent<Script_ATK>().playKnifeLight();
            // else if(hitName == "Hit_H_Right")
            //     GameObject.Find("Player").GetComponent<Script_ATK2>().playKnifeLight();
            // else if(hitName == "Hit_D_Top")
            //     GameObject.Find("Player").GetComponent<Script_ATK3>().playKnifeLight();

            
            Collider[] attackDetectionTargets = new Collider[4];

            int counts = Physics.OverlapSphereNonAlloc(attackDetectionCenter.position, attackDetectionRang,
                attackDetectionTargets, enemyLayer);
                
            if (counts > 0)
            {
                for (int i = 0; i < counts; i++)
                {
                    if (attackDetectionTargets[i].TryGetComponent(out IDamagar damagar))
                    {
                        damagar.TakeDamager(5f,hitName,transform.root.transform);
                        
                    }
                }
            }
           PlayWeaponEffect();
        }
        //播放武器音效
        private void PlayWeaponEffect()
        {
            if(_animator.CheckAnimationTag("Attack"))
            {
                GameAssets.Instance.PlaySoundEffect(_audioSource,SoundAssetsType.swordWave);
            }
            if(_animator.CheckAnimationTag("GSAttack"))
            {
                GameAssets.Instance.PlaySoundEffect(_audioSource,SoundAssetsType.hSwordWave);
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(attackDetectionCenter.position, attackDetectionRang);
        }
    }
}
