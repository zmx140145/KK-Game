using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="AI_Idle",menuName ="StateMachine/State/AI_Idle")]
public class AI_Idle : StateActionSO
{
    public override void OnUpdate()
    {
        Debug.Log("等待");
    }
}
