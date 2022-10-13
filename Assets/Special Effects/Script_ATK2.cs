using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ATK2 : MonoBehaviour
{
    // Start is called before the first frame update
   public ParticleSystem Particle_System;

   public void playKnifeLight(){
        Particle_System.Play();
   }
}
