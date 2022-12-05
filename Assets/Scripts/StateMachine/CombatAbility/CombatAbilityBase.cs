using System.Collections;
using System.Collections.Generic;
using UGG.Move;
using UnityEngine;

public abstract class CombatAbilityBase : ScriptableObject
{
   [SerializeField] protected  string abilityName;
  [SerializeField] protected int abilityID;
  [SerializeField]protected float abilityCDTime;
   
    [SerializeField] protected float abilityUseDistance;
    [SerializeField]  protected bool abilityIsDone;


protected Animator _animator;
protected AICombatSystem _combatSystem;
protected CharacterMovementBase _movement;

//AnimatorID
protected int runID=Animator.StringToHash("Run");
    protected int horizontalID = Animator.StringToHash("Horizontal");
    protected int verticalID = Animator.StringToHash("Vertical");

/// <summary>
/// 调用技能
/// </summary>
public abstract void InvokeAbility();
protected void UseAbility()
{
    _animator.Play(abilityName,0,0f);
    abilityIsDone=false;
   ResetAbility();
}

public void ResetAbility()
{
     //技能CD
    //在对象池拿计时器 计时器在到时间时会调用定时时输入进入的委托
    GameObjectPoolSystem.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(abilityCDTime,()=>abilityIsDone=true,false);
}


    #region 调用接口


public void InitAbility(Animator _animator,AICombatSystem _combat,CharacterMovementBase _moment)
{
    this._animator=_animator;
    this._combatSystem=_combat;
    this._movement=_moment;
}
    public string GetAbilityName()=>abilityName;
    public int GetAbilityID()=>abilityID;
    public bool GetAbilityIsDone()=>abilityIsDone;
    public void SetAbilityDone(bool done)=>abilityIsDone=done;

    #endregion
}
