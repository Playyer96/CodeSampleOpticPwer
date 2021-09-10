using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosIco : MonoBehaviour
{
   public Transform t_Target;
   public Vector3 v_Offset;

   private void Update()
   {
      transform.position = t_Target.position + v_Offset;
   }
}
