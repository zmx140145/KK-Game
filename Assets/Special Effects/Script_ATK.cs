using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ATK : MonoBehaviour
{
    // Start is called before the first frame update
   public ParticleSystem Particle_System;
public void SetRotationAndPlay(int angle)
{
    Particle_System.transform.localRotation = Quaternion.Euler(0, 0, angle );
    playKnifeLight();
}
   public void playKnifeLight(){
        Particle_System.Play();
   }
}
