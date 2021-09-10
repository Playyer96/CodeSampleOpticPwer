using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using static RandomProductsAndsShelfsSlots;

public class ReportBackendPicking : MonoBehaviour
{
   public List<PickingPocketProductInfo> requestedList;
   public List<PickingPocketProductInfo> pickedList;
   public ReportBackend[] rb;
   
   public void CreateReport()
   {
      requestedList = FindObjectOfType<PickingManager>().requestedList;
      pickedList = FindObjectOfType<PickingManager>().pickedList;

      for (int i = 0; i < rb.Length; i++)
      {
         if (i < requestedList.Count)
         {
            if (requestedList[i].quantity == pickedList[i].quantity)
            {
               rb[i].isReported = true;
               rb[i].report = "Del producto: " + requestedList[i].productCode + " tomó la cantidad correcta";
            }
            else if(pickedList[i].quantity > requestedList[i].quantity)
            {
               rb[i].report = "Del producto: " + requestedList[i].productCode + " tomó más de la cantidad solicitada";
            }
            else
            {
               rb[i].report = "Del producto: " + requestedList[i].productCode + " tomó menos de la cantidad solicitada";
            }
         }
         else
         {
            rb[i].gameObject.SetActive(false);
         }
      }
      
      SendReportBackEnd.Instance.CreateList();
   }
}
