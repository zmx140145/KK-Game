using System;
using System.Collections;
using System.Collections.Generic;
using UGG.Move;
using UnityEngine;

namespace UGG.Combat
{
    public abstract class CharacterCombatSystemBase : MonoBehaviour
    {
        protected Animator _animator;
        protected CharacterInputSystem _characterInputSystem;
        protected CharacterMovementBase _characterMovementBase;
        protected AudioSource _audioSource;
        
        
        //aniamtionID
        protected int lAtkID = Animator.StringToHash("LAtk");
        protected int rAtkID = Animator.StringToHash("RAtk");
        protected int defenID = Animator.StringToHash("Defen");
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

        protected virtual void OnAnimationAttackParticleEvent(int angle)
        {
            Script_ATK obj= GameObject.Find("Player").GetComponent<Script_ATK>();
            obj.SetRotationAndPlay(angle);
                
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
                        damagar.TakeDamager(0f,hitName,transform.root.transform);
                        
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
