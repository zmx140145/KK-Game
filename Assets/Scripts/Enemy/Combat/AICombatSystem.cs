using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGG.Combat;
public class AICombatSystem : CharacterCombatSystemBase
{
   [SerializeField,Header("检测范围中心")] private Transform detectionCenter;
   [SerializeField] private float detectionRang;
   [SerializeField] private LayerMask whatisEnemy;
   [SerializeField] private LayerMask whatisBos;
   Collider[] colliderTarget=new Collider[1];
   [SerializeField,Header("目标")] private Transform currentTarget;
   private void Update() {
    AIView();
   }
private void AIView()
{
int targetCount=Physics.OverlapSphereNonAlloc(detectionCenter.position,detectionRang,colliderTarget,whatisEnemy);
if(targetCount>0)
{
    if(!Physics.Raycast(transform.root.position+transform.root.up*0.5f,(colliderTarget[0].transform.position-transform.root.position).normalized,out var hit,detectionRang,whatisBos))
    {
       if(Vector3.Dot((colliderTarget[0].transform.position-transform.root.position).normalized,transform.root.forward)>0.35f)
       {
        currentTarget=colliderTarget[0].transform;
       }
    }
}
}
}
