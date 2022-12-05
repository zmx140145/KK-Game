using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="NormalAbility",menuName ="Ability/NormalAbility")]
public class NormalAbility : CombatAbilityBase
{
   
    public override void InvokeAbility()
    {
        //技能逻辑
        if(_animator.CheckAnimationTag("Motion")&&abilityIsDone)
        {
            if(_combatSystem.GetCurrentTargetDistance()>abilityUseDistance+0.1f)
            {
                //当技能被激活时还没有进入范围 加速往前跑
               _movement.CharacterMoveInterface(_combatSystem.GetDirectionForTarget(),5f,true);
               _animator.SetFloat(runID,1f,0.23f,Time.deltaTime);
               _animator.SetFloat(verticalID,2f,0.23f,Time.deltaTime);
               _animator.SetFloat(horizontalID,0f,0.1f,Time.deltaTime);
            }
            else
            {
               //使用技能
               UseAbility();
            }
            
        }
    }
}
