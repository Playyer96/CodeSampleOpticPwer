//using System;
//using DreamHouseStudios.SofasaLogistica;
//using DreamHouseStudios.VR;
//using System.Collections;
//using UnityEngine;

//public class ProductTrigger : MonoBehaviour
//{
//    public bool inMobileShelf, counted = false, released; //counted = ya se contó del inventario
//    public Vector3 diff;
//    public Rigidbody rb;
//    [NonSerialized] public static bool firstGrabbed, firstReleased;

//    public Interactable i_Interactable;
//    //public float lastXApplied;

//    public ProductInvoice productInvoice;

//    private IEnumerator Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        i_Interactable = GetComponent<Interactable>();
//        while (productInvoice == null)
//        {
//            productInvoice = GetComponentsInChildren<ProductInvoice>()[0];
//            yield return new WaitForSeconds(.25f);
//        }
//    }

//    /*private void OnTriggerEnter(Collider other)
//    {
//        if (other.tag == "EstanteriaMovil" && !inMobileShelf)
//        {
//            inMobileShelf = true;
//            //Debug.LogError("Comentar las siguientes dos líneas.");
//            //PickingManager.CountDiscountProduct(transform.GetComponentsInChildren<ProductInvoice>()[0]);//BORRARRRRRRR!!!!!
//            //counted = true;//BORRARRRRRRR!!!!!
//        }
//    }*/

//    private void OnTriggerStay(Collider other)
//    {
//        if (inMobileShelf) return;
//        if (other.tag == "EstanteriaMovil" && !inMobileShelf)
//        {
//            //if (GetComponent<DreamHouseStudios.VR.Interactable>() != null)
//            //{
//            //    DreamHouseStudios.VR.Interactable i_Interactable = GetComponent<DreamHouseStudios.VR.Interactable>();
//            //    if (!i_Interactable.beingGrabbed)
//            //    {
//            //        transform.parent = other.transform;
//            //        i_Interactable.enabled = false;
//            //    }
//            //}
//            if (!i_Interactable.beingGrabbed)
//            {
//                transform.parent = other.transform;
//                inMobileShelf = true;

//                if (!PickingManager.instance) return;
//                if (!counted)
//                {
//                    counted = true; //contar en la lista de picked
//                    released = true;
//                    Debug.Log("<color=blue>Release + count</color>");
//                    PickingManager.CountDiscountProduct(productInvoice);
//                }
//            }

//            //Debug.LogError("Comentar esta y las siguientes dos líneas.");
//            //PickingManager.CountDiscountProduct(transform.GetComponentsInChildren<ProductInvoice>()[0]);//BORRARRRRRRR!!!!!
//            //counted = true;//BORRARRRRRRR!!!!!
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        Debug.Log("Exit");

//        if (!PickingManager.instance) return;
//        if (other.tag == "EstanteriaMovil" && inMobileShelf)
//        {
//            Debug.Log(name + " IsCounted: " + counted.ToString());
//            if (counted)
//                PickingManager.CountDiscountProduct(transform.GetComponentsInChildren<ProductInvoice>()[0],
//                    -1); //descontar de la lista de picked
//            inMobileShelf = false;
//            counted = false;
//        }
//    }
//}