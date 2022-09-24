using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraForKK : MonoBehaviour
{
   [SerializeField] private Transform Player;
   [SerializeField] private Transform Target;
   [SerializeField,Header("最近响应距离")] private float MinDistance;
   private Vector3 lastV3;
   [SerializeField] private Camera m_camera;
   public Vector3 right;
   [SerializeField] private Transform rightItem;
    [SerializeField] private float distance;
    [SerializeField] private float height;
     [SerializeField,Header("检测层")] protected LayerMask ObstacleLayer;
       [SerializeField,Header("遮蔽层")] protected int CullLayer;
         [SerializeField,Header("显示层")] protected int ViewLayer;
     [SerializeField] private HashSet<GameObject> hitInfos=new HashSet<GameObject>();
    [SerializeField] private HashSet<GameObject> hitObjs=new HashSet<GameObject>();
     [SerializeField] private float UpdateTime;
     private float time=0f;
   void Start()
   {
 
   }
   private void Update() {
    ChangeCameraPos();
   Vector3 v3=rightItem.position-transform.position;
   v3.y=0;
   right=v3.normalized;
   if(time>UpdateTime)
   {
CheckViewObstacle();
time=0f;
   }
   time+=Time.deltaTime;
   }
   private void CheckViewObstacle()
   {
      //获得指向角色的向量
      Vector3 toCenter=(Target.position+Player.position)/2f-transform.root.position;
      Vector3 toUL=m_camera.ViewportToWorldPoint(new Vector3(0,0,toCenter.magnitude));
       Vector3 toDR=m_camera.ViewportToWorldPoint(new Vector3(1 ,1,toCenter.magnitude));
      Vector3 dir=toCenter.normalized;
      hitInfos.Clear();
      for(float i=0f;i<=1f;i+=0.1f)
      {
         for(float j=0f;j<=1f;j+=0.1f)
         {
        Ray ray=m_camera.ViewportPointToRay(new Vector3(i,j,1f));
        Debug.DrawRay(ray.origin,ray.direction,Color.blue,0.5f);
        RaycastHit[] hits=  Physics.RaycastAll(ray,toCenter.magnitude,ObstacleLayer);
        foreach(var it in hits)
        {
        hitInfos.Add(it.collider.gameObject);
        }
         }
      }
    


if(hitInfos.Count>0)
{
      foreach(GameObject obj in hitObjs)
      {
         bool flag=false;
         foreach(GameObject hit in hitInfos)
         {
       if(hit==obj)
       {
         flag=true;
       }
         }
         if(!flag)
         {
 obj.gameObject.layer=ViewLayer;
         }
        
      }
      hitObjs.Clear();
      foreach(GameObject hit in hitInfos)
      {
         hit.gameObject.layer=CullLayer;
         hitObjs.Add(hit);
      }
}   
else
{
    foreach(GameObject obj in hitObjs)
    {
       obj.gameObject.layer=ViewLayer;
    }
}

  }
   private void ChangeCameraPos()
   {
   Vector3 ToTarget=(Target.position-Player.position);
   //获得两个人之间的距离
   float distanceOfThem=ToTarget.magnitude;
   ToTarget.y=0;
   //获得玩家到目标的方向向量
   ToTarget=ToTarget.normalized;
   //如果上一次的记录不是空的
   if(lastV3!=null)
   {
      
     if(Vector3.Dot(ToTarget,lastV3)<-0.5)
         {
            //点积计算角度 如果大角度相反方向的镜头旋转
            //那么让结果反向
          ToTarget=-ToTarget;
         }
         
                  
              lastV3=Vector3.Lerp(lastV3,ToTarget,Mathf.Max(distanceOfThem*distanceOfThem-0.2f/MinDistance));
                   
      
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




   private void OnDrawGizmos() {
       Vector3 toCenter=(Target.position+Player.position)/2f-transform.position;
      Vector3 toUL=m_camera.ViewportToWorldPoint(new Vector3(0,0,toCenter.magnitude));
       Vector3 toDR=m_camera.ViewportToWorldPoint(new Vector3(1 ,1,toCenter.magnitude));
       Gizmos.color=Color.blue;
      Gizmos.DrawLine(transform.root.position,toUL);
       Gizmos.DrawLine(transform.root.position,toDR);

   }
}
