using System;
using System.Collections;
using System.Collections.Generic;
using DreamHouseStudios.SofasaLogistica;
using UnityEditor;
using UnityEngine;

public class HookStatus : MonoBehaviour
{
   public DreamHouseStudios.VR.Interactable i_Interactable;
   public string s_Name;
   public Rigidbody r_RB;
   public bool b_IsTrigger;

   private void Start()
   {
      i_Interactable = GetComponent<DreamHouseStudios.VR.Interactable>();
   }

   private void Update()
   {
      if (b_IsTrigger || !r_RB.isKinematic)
      {
         return;
      }
      else
      {
         if (!i_Interactable.beingGrabbed)
         {
            r_RB.isKinematic = false;
         }
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.transform.name == s_Name)
      {
         b_IsTrigger = true;
      }
   }

   private void OnTriggerStay(Collider other)
   {
      if (other.transform.name == s_Name)
      {
         if (!i_Interactable.beingGrabbed)
         {
            r_RB.isKinematic = true;
            transform.localPosition = other.GetComponent<ReferenceState>().v_Position;
            transform.localEulerAngles = other.GetComponent<ReferenceState>().v_Rotation;
            if (other.GetComponent<ReferenceState>().b_Event)
            {
               other.GetComponent<ReferenceState>().LaunchEvent();
            }
         }
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.transform.name == s_Name)
      {
         b_IsTrigger = false;
      }
   }
}
