using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGG.Health;
using UGG.Move;


public abstract class ConditionSO : ScriptableObject
{
    //����
    protected AICombatSystem _combatSystem;
    protected AIMovement _movement;
    protected AIHealthSystem _healthSystem;
    protected Animator animator;

    [SerializeField] protected int priority;



    public void InitCondition(StateMachineSystem stateSystem)
    {
        _combatSystem = stateSystem.GetComponentInChildren<AICombatSystem>();

        _movement = stateSystem.GetComponent<AIMovement>();

        _healthSystem = stateSystem.GetComponent<AIHealthSystem>();

        animator = stateSystem.GetComponentInChildren<Animator>();
    }

    public abstract bool ConditionSetUp();

    
    public int GetConditionPriority() => priority;
}
