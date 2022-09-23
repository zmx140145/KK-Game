using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraForKK : MonoBehaviour
{
   [SerializeField] private Transform Player;
   [SerializeField] private Transform Target;
   [SerializeField,Header("最近响应距离")] private float MinDistance;
   private Vector3 lastV3;
   private Camera m_camera;
   public Vector3 right;
   [SerializeField] private Transform rightItem;
    [SerializeField] private float distance;
    [SerializeField] private float height;
   void Start()
   {
    m_camera=GetComponentsInChildren<Camera>()[0];
   }
   private void Update() {
    ChangeCameraPos();
   Vector3 v3=rightItem.position-transform.position;
   v3.y=0;
   right=v3.normalized;
   }
   private void ChangeCameraPos()
   {
   Vector3 ToTarget=(Target.position-Player.position);
   float distanceOfThem=ToTarget.magnitude;
   ToTarget.y=0;
   ToTarget=ToTarget.normalized;
   if(lastV3!=null)
   {
     if(Vector3.Dot(ToTarget,lastV3)<-0.5)
         {
          ToTarget=-ToTarget;
         }
                   if(distanceOfThem>MinDistance)
                   {
                     lastV3=Vector3.Lerp(lastV3,ToTarget,distanceOfThem/MinDistance-1f);
                   }
      
   }
   else
   {
      lastV3=ToTarget;
   }
   Vector3 ToPerpendCenter=Vector3.Cross(Vector3.up,lastV3).normalized;
   Vector3 center=(Target.position+Player.position)/2f;
   center.y=height;
   Vector3 pos=center+ToPerpendCenter*distance;
   pos.y=height;
   transform.position=pos;
   transform.LookAt(center);
   }
}
