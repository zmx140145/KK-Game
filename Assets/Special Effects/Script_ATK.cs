using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ATK : MonoBehaviour
{
    private Dictionary<string,ParticleSystem> dic=new Dictionary<string, ParticleSystem>();
    public string[] str;
    // Start is called before the first frame update
   public ParticleSystem[] Particle_System;
   private void Start() {
    for(int i=0;i<str.Length;i++)
    {
        dic.Add(str[i],Particle_System[i]);
    }
    
   }
public void SetRotationAndPlay(string str,int? angle,float startTime)
{
    if(dic.TryGetValue(str,out var it))
    {
        if(angle!=null)
        {
it.transform.localRotation = Quaternion.Euler(0, 0, (int)angle );
        }
         
          playKnifeLight(it,startTime);
    }
 
}
   public void playKnifeLight(ParticleSystem it,float t){
    {
        it.time=t;
        it.Play();
    }
     
   }
   private void Update() {
    
   }
}
