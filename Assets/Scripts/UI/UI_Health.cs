using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Health : MonoBehaviour
{
   
   protected int blueID=Animator.StringToHash("Blue");
   [SerializeField] private Image[] BlueImgs;
   [SerializeField] private Image[] HealthImgs;
   [SerializeField] private Image HealthEffImg;
   [SerializeField] private float lostMult;
   private Animator _animator;
   [SerializeField]private float blue;
   [SerializeField]private float health;
   public float Health
   {
    get{
        return health;
    }
    set
    {
        
        health=value<0f?0f:value;
      
        foreach(var it in HealthImgs)
        {
            it.fillAmount=value<0f?0f:value/100f;
        }
        
    }
   }
   public float Blue
   {
    get{
        return blue;
    }
    set
    {
          blue=value<0f?0f:(value>100f)?100f:value;
           foreach(var it in BlueImgs)
        {
            it.fillAmount=blue/100f;
        }
    }
   }
   private void Start() {
    _animator=GetComponent<Animator>();
    Health=100f;
    Blue=0f;
   }
   private void Update() {
    if(health/100f<HealthEffImg.fillAmount)
    {
     StartCoroutine("HealthLost");
    }
    else
    {
    if(health/100f>HealthEffImg.fillAmount)
    {
    HealthEffImg.fillAmount= health/100f;
    }
    }
    if(blue>=100f)
    {
        _animator.SetBool(blueID,true);
    }
    else
    {
        _animator.SetBool(blueID,false);
    }
   }
   public void UseBlue()
   {
    _animator.Play("BlueUse",0,0f);
    StartCoroutine("BlueUse");
   }
   private IEnumerator BlueUse()
   {
    yield return new WaitForFixedUpdate();
    while(_animator.CheckAnimationTag("Blue"))
    {
        Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
     Blue=(1f-_animator.GetCurrentAnimatorStateInfo(0).normalizedTime)*100f;
     yield return null;
    }
    Blue=0f;
    yield break;
   }
   private IEnumerator HealthLost()
   {
    while(health/100f<HealthEffImg.fillAmount)
    {
        HealthEffImg.fillAmount-=Time.fixedDeltaTime*lostMult;
        yield return new WaitForFixedUpdate();
    }
    HealthEffImg.fillAmount=health/100f;
    yield break;
   }
}
