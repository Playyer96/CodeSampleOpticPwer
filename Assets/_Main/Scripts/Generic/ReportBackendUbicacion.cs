using System.Collections;
using System.Collections.Generic;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;

public class ReportBackendUbicacion : MonoBehaviour
{
   public LocationManager.Shelf[] shelf;
   public ReportBackend[] rb;

   public void CreateReport()
   {
      shelf = FindObjectOfType<LocationManager>().shelves;
      for (int i = 0; i < rb.Length; i++)
      {
         if (shelf[i].totalProductsOnShelf > 0)
         {
            if (shelf[i].allBagsOnShelf)
            {
               rb[i].isReported = shelf[i].allBagsOnShelf;
               rb[i].report = "Todos los productos ubicados correctamente en la estantería: " + shelf[i].shelfId;
            }
            else
            {
               rb[i].report = "Falta ubicar productos en la estantería: " + shelf[i].shelfId;
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
