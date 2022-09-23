using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponAnimationEvent : MonoBehaviour
{
   [SerializeField] private Transform hipGS;
   [SerializeField] private Transform handGS;
   [SerializeField] private Transform handKatana;
 [SerializeField] private Transform hipKatana;

   private void ShowGS()
   {
    if(!handGS.gameObject.activeSelf)
    {
        handGS.gameObject.SetActive(true);
        hipGS.gameObject.SetActive(false);
        handKatana.gameObject.SetActive(false);
        hipKatana.gameObject.SetActive(true);
    }
   }
   private void HideGS()
   {
    if(handGS.gameObject.activeSelf)
    {
        handGS.gameObject.SetActive(false);
        hipGS.gameObject.SetActive(true);
        handKatana.gameObject.SetActive(true);
        hipKatana.gameObject.SetActive(false);
    }
   }
}
