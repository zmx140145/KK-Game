using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollEffect : MonoBehaviour
{
   [SerializeField]private ParticleSystem _particle;
   [SerializeField]private float EndTime;
   [SerializeField]private float StartTime;
   private void Start() {
  
      
       _particle.time=StartTime;
   }
   private void Update() {
    Debug.LogWarning( _particle.time);
    if(_particle.time>EndTime)
    {
        Destroy(gameObject);
    }
   }
}
