using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateMachineSystem : MonoBehaviour
{

    public float CurStateRunTime=0f;
    public NB_Transition transition;

    
    public StateActionSO currentState;


    private void Awake()
    {
        transition?.InitTransition(this);
        currentState?.OnEnter();
    }


    private void Update()
    {
        StateMachineTick();
        CurStateRunTime+=Time.deltaTime;
    }

    private void StateMachineTick()
    {
        transition?.TryGetApplyCondition();
        currentState?.OnUpdate();

    }
}
