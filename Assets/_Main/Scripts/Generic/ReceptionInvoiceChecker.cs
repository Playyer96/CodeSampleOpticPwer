using System.Collections;
using System.Collections.Generic;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;

public class ReceptionInvoiceChecker : MonoBehaviour
{
   public Bag_Shelf[] bs;

   public void SetReceptionInvoice()
   {
      bs = FindObjectsOfType<Bag_Shelf>();
   }

   public bool SetReceptionInvoiceChecker()
   {
      bool checker = true;
      for (int i = 0; i < bs.Length; i++)
      {
         if (!bs[i].b_HasReceptionInvoice)
         {
            checker = false;
            return checker;
         }
      }
      return checker;
   }

   public void SetReportBackend()
   {
      GetComponent<ReportBackend>().isReported = SetReceptionInvoiceChecker();
   }
}
