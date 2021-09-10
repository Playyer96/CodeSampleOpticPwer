using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsVisualize : MonoBehaviour
{
  
   void OnDrawGizmos()
   {
      for (int i = 0; i < transform.childCount; i++)
      {
          Gizmos.color = Color.green;
          Gizmos.DrawWireSphere(transform.GetChild(i).position, .1f);
      }
   }
}
