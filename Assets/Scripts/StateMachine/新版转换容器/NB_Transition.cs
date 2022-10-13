using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NB_Transition", menuName = "StateMachine/Transition/New NB_Transition")]
public class NB_Transition : ScriptableObject
{
    [Serializable]
    private class StateAcitionConfig
    {
        public StateActionSO fromState;
        public StateActionSO toState;
        public List<ConditionSO> conditions;

        public void Init(StateMachineSystem stateMachineSystem)
        {
            fromState.InitState(stateMachineSystem);
            toState.InitState(stateMachineSystem);

            foreach (var item in conditions)
            {
                item.InitCondition(stateMachineSystem);
            }
        }
    }


    //�洢����״̬ת����Ϣ������

    private Dictionary<StateActionSO, List<StateAcitionConfig>> states = new Dictionary<StateActionSO, List<StateAcitionConfig>>();

    //��ȡ״̬���ã����ⲿ�����ֶ�������Ϣ
    [SerializeField] private List<StateAcitionConfig> configStateData = new List<StateAcitionConfig>();

    private StateMachineSystem stateMachineSystem;


    public void InitTransition(StateMachineSystem stateMachineSystem)
    {
        this.stateMachineSystem = stateMachineSystem;
        SaveAllStateTransitionInfo();
    }


    /// <summary>
    /// ��������״̬������Ϣ
    /// </summary>
    private void SaveAllStateTransitionInfo()
    {
        foreach (var item in configStateData)
        {
            item.Init(stateMachineSystem);

            //���ʱ����������Ѿ����ú���Ϣ�ˡ�������Ҫ�����ǵ�ת����ϵ��������
            if (!states.ContainsKey(item.fromState))
            {
                //������ڴ洢�ֵ��Ƿ��д��ڵ�Key,���û��������Ҫ����һ�������ҳ�ʼ�����������洢����
                states.Add(item.fromState, new List<StateAcitionConfig>());
                states[item.fromState].Add(item);
            }
            else
            {
                states[item.fromState].Add(item);
            }
        }
    }


    /// <summary>
    /// ����ȥ��ȡ������������״̬
    /// </summary>
    public void TryGetApplyCondition()
    {
        int conditionPriority = 0;
        int statePriority = 0;
        List<StateActionSO> toStates = new List<StateActionSO>();
        StateActionSO toState = null;

        //������ǰ״̬��ת��״̬�Ƿ�����������
        if (states.ContainsKey(stateMachineSystem.currentState))
        {
            foreach (var stateItem in states[stateMachineSystem.currentState])
            {
                foreach (var conditionItem in stateItem.conditions)
                {
                    if (conditionItem.ConditionSetUp())
                    {
                        if (conditionItem.GetConditionPriority() >= conditionPriority)
                        {
                            //��ô�ͽ�ת����ϵ����һ��״̬���������������ж��������ˣ������һ��Ψһ���ʵ�״̬
                            conditionPriority = conditionItem.GetConditionPriority();
                            toStates.Add(stateItem.toState);
                        }
                    }
                }
            }
        }
        else
        {
            return;
        }

        if (toStates.Count != 0 || toStates != null)
        {
            //�����������������ȼ����������ȼ���ߵ�����һ������
            foreach (var item in toStates)
            {
                if (item.GetStatePriority() >= statePriority)
                {
                    //����һ��״̬����Ϊ���ȼ���ߵ���һ��
                    statePriority = item.GetStatePriority();
                    toState = item;
                }
            }
        }

        if (toState != null)
        {
            stateMachineSystem.currentState.OnExit();
            stateMachineSystem.currentState = toState;
            stateMachineSystem.currentState.OnEnter();
            toStates.Clear();
            conditionPriority = 0;
            statePriority = 0;
            toState = null;
        }
    }



}
