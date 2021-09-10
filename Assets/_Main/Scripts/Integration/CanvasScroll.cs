using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScroll : MonoBehaviour
{
    CanvasInteractor ci;
    float f_deltaX;

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CanvasInteractor>() != null)
       {
           ci = other.GetComponent<CanvasInteractor>();
           
       }
    }
   void OnTriggerStay(Collider other)
   {
       if(ci != null)
       {
           if(ci.canInterac)
           {
               
           }
       }
   }

   void OnTriggerExit(Collider other)
   {
       if(ci != null)
       {
           ci = null;
       }
   }
}
