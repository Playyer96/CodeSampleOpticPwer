using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;

public class TargetCanvasFP : MonoBehaviour
{
   public Vector3 v_OffsetPos;
   public Transform tr_PlayerRef;
   public float fl_OffsetZ;

   private void Update()
   {
       v_OffsetPos.z = tr_PlayerRef.position.z + fl_OffsetZ;
       transform.position = v_OffsetPos;
   }
}
