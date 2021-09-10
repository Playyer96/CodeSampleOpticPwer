
using System.Collections.Generic;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;
using UnityEngine.Events;

public class ShelfPacakage : MonoBehaviour
{
    public bool allBagsOnShelf;
    public bool progressIsSet;
    public BoxManager b_BM;
    public List<Bag_Shelf> b_Products;
    public UnityEvent e_OnAllProducts;
    public UnityEvent e_OnDeferredProduct;
    private bool b_call = false;
    public GameObject g_Residue;
    public TutorialReception rm;

    private void Start()
    {
        rm = FindObjectOfType<TutorialReception>();
        if (e_OnAllProducts == null)
        {
            e_OnAllProducts = new UnityEvent();
        }

        if (e_OnDeferredProduct == null)
        {
            e_OnDeferredProduct = new UnityEvent();
        }
        b_Products = new List<Bag_Shelf>();
        b_BM = FindObjectOfType<BoxManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Bag_Shelf>())
        {
            Bag_Shelf bs = other.GetComponent<Bag_Shelf>();
            if (bs.b_Ready && !bs.b_IsDeferred)
            {
                if (b_Products.Contains(bs))
                {
                    return;
                }
                else
                {
                    b_Products.Add(bs);
                    allBagsOnShelf = CheckProducts();
                }
            }
        }

        if (allBagsOnShelf)
        {
            LaunchEvent();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Bag_Shelf>())
        {
            Bag_Shelf bs = other.GetComponent<Bag_Shelf>();
            if (b_Products.Contains(bs))
            {
                bs.b_Ready = false;
                b_Products.Remove(bs);
            }
        }
    }

    public void AddDeferred(Bag_Shelf bs)
    {
        if (bs.b_Ready && bs.b_IsDeferred)
        {
            if (b_Products.Contains(bs))
            {
                return;
            }
            else
            {
                b_Products.Add(bs);
                allBagsOnShelf = CheckProducts();
                e_OnDeferredProduct.Invoke();
            }
        }
        if (allBagsOnShelf)
        {
            LaunchEvent();
        }
    }

    public bool CheckProducts()
    {
        if (b_Products.Count == b_BM.i_Products)
        {
            for (int i = 0; i < b_Products.Count; i++)
            {
                if (!b_Products[i].b_Ready)
                {
                    return false;
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public void LaunchEvent()
    {
        if (!b_call)
        {
            b_call = true;
            e_OnAllProducts.Invoke();
            if (rm.s_Settings.experienMode == ExperienMode.Evaluacion)
            {
                g_Residue.GetComponent<DreamHouseStudios.VR.Interactable>().enabled = true;
            }
        }
    }
}