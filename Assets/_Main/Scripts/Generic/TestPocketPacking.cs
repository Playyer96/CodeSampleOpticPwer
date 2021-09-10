using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DreamHouseStudios.SofasaLogistica;

public class TestPocketPacking : MonoBehaviour
{
   public GameObject[] g_Products;
   [SerializeField]
   public List<DataProducts> l_Data;
   public Transform[] t_SpawnPoints;
   public List<GameObject> productsOnScene = null;

   private void Start()
   {
       SimulateProducts();
   }

   void SimulateProducts()
   {
       int i_Index = 0;
       int i_IndexList = 0;
       for (int i = 0; i <g_Products.Length; i++)
       {
           int r = Random.Range(1, 4);
           for (int j = 0; j < r; j++)
           {
               GameObject go = Instantiate(g_Products[i], Vector3.zero, Quaternion.identity);
               go.GetComponent<Bag_Shelf>().b_IsInShlef = true;
               go.GetComponent<Bag_Shelf>().b_HasReceptionInvoice = true;
               go.transform.position = t_SpawnPoints[i_Index].position;
               go.transform.parent = this.transform;
               go.GetComponent<ObjectReset>().resetPos = t_SpawnPoints[i_Index];

               productsOnScene.Add(go);

               i_Index++;
               if (j == 0)
               {
                   l_Data.Add(new DataProducts());
                   l_Data[i_IndexList].s_ID = go.GetComponentInChildren<ProductInvoice>().Product.productId;
                   l_Data[i_IndexList].s_Description = go.GetComponentInChildren<ProductInvoice>().Product.description;
                   l_Data[i_IndexList].i_Amount = r;
                   i_IndexList++;
               }
           }
       }
   }
}

[System.Serializable]
public class DataProducts
{
    public string s_ID;
    public string s_Description;
    public int i_Amount;
}
