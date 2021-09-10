
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;

public class ScanChecker : MonoBehaviour
{
   public ProductInvoice[] pi;

   public void OnFillProducts()
   {
      pi = FindObjectsOfType<ProductInvoice>();
   }

   public bool CheckAllProducts()
   {
      bool scanned = true;
      for (int i = 0; i < pi.Length; i++)
      {
         if (!pi[i].Scanned)
         {
            scanned = false;
            return scanned;
         }
      }
      return scanned;
   }

   public void SetReportBackend()
   {
      GetComponent<ReportBackend>().isReported = CheckAllProducts();
   }
}
