using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterShelf : MonoBehaviour
{
   public static CounterShelf Instance;
   public ShelfDetection[] shelf;
   private ReportBackend rb;

   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
      }
      rb = GetComponent<ReportBackend>();
   }

   public void GetInts()
   {
      int whells = 0;
      foreach (ShelfDetection s in shelf)
      {
         whells += s.g_Colliders.Count;
      }
      rb.actualActions = whells;
      rb.CounterActions(0);
   }

}
