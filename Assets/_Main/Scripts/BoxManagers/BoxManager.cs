using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DreamHouseStudios.SofasaLogistica;
using Random = UnityEngine.Random;
using UnityEngine.Events;
using Valve.VR;

public class BoxManager : MonoBehaviour
{
    public bool instanceProductsOnStart = false;
    public GameObject[] g_Box;
    public GameObject[] g_boxInPlace;
    public List<GameObject> g_products;
    public Transform[] t_SpawnPoints;

    public GameObject g_CollidersFotos;

    public int i_Min, i_Max;
    int r;
    public int i_Products;

    [Space(10f), Header("Scene Managers")] public ReceptionManager R_Rm;
    public LocationManager L_Lm;
    public SpectraUISettings s_Settings;
    public PrinterList pl;

    public UnityEvent OnCreateProducts;

    void Start()
    {
        if (OnCreateProducts == null)
        {
            OnCreateProducts = new UnityEvent();
        }
        
        if (instanceProductsOnStart)
        {
            StartCoroutine(StartInstantiateProducts());
        }
        else
        {
            SetBox();
        }


        i_Products = Random.Range(i_Min, i_Max);
        if (pl != null)
        {
            pl.productAmmount = i_Products;
        }
        R_Rm = FindObjectOfType<ReceptionManager>();

        if (R_Rm != null)
        {
            R_Rm.totalItemsToTrack += i_Products;
        }

        if (L_Lm != null)
        {
            L_Lm.totalProducts += i_Products;
            //L_Lm.totalItemsToTrack += i_Products;
        }

        if (g_CollidersFotos != null)
            g_CollidersFotos.SetActive(false);
    }

    void SetBox()
    {
        HideBoxes();
        if (s_Settings.boxType == 0)
        {
            s_Settings.boxType = 1;
        }
        else
        {
            s_Settings.boxType = 0;
        }
        g_Box[s_Settings.boxType].SetActive(true);
    }

    public void HideBoxes()
    {
        for (int i = 0; i < g_Box.Length; i++)
        {
            g_Box[i].SetActive(false);
        }
    }

    public void ActiveBoxInPlace()
    {
        g_boxInPlace[s_Settings.boxType].SetActive(true);
        R_Rm.boxInvoices = g_boxInPlace[s_Settings.boxType].GetComponentInChildren<BoxInvoice>();
        g_CollidersFotos.SetActive(true);
    }

    IEnumerator StartInstantiateProducts()
    {
        yield return new WaitForSeconds(0.5f);
        InstantiateProducts();
    }

    public void InstantiateProducts()
    {
        int randomp = Random.Range(1, i_Products);
        for (int i = 0; i < i_Products; i++)
        {
            int i_R = Random.Range(0, g_products.Count);
            GameObject g_Go = Instantiate(g_products[i_R], t_SpawnPoints[i].position, Quaternion.identity);
            g_Go.GetComponent<ObjectReset>().resetPos = t_SpawnPoints[i];
            g_Go.AddComponent<RandomProductsAndsShelfsSlots.ProductTrigger>();
            if (s_Settings.experienMode == ExperienMode.Entrenamiento)
            {
                g_Go.GetComponent<DreamHouseStudios.VR.Interactable>().enabled = false;
            }


            if (R_Rm != null)
                R_Rm.productInvoices.Add(g_Go.GetComponentInChildren<ProductInvoice>());

            if (L_Lm != null)
            {
                L_Lm.receptionInvoices.Add(g_Go.GetComponentInChildren<ReceptionInvoice>());
                L_Lm.productInvoices.Add(g_Go.GetComponentInChildren<ProductInvoice>());
            }

            if (R_Rm != null)
            {
                if (i == randomp)
                {
                    if (g_Go.GetComponent<Bag_Shelf>() != null)
                    {
                        g_Go.GetComponent<Bag_Shelf>().b_IsDeferred = true;
                        g_Go.GetComponent<Bag_Shelf>().Get_Component();
                    }
                }
            }

            g_products.RemoveAt(i_R);
        }
        OnCreateProducts.Invoke();

        /*if (s_Settings.experienMode == ExperienMode.Entrenamiento)
        {
            if (R_Rm != null)
            {
                for (int i = 0; i < R_Rm.productInvoices.Count; i++)
                {
                    Debug.Log("<color=red>Indice Error " + i+"</color>");
                    R_Rm.productInvoices[i].GetComponent<DreamHouseStudios.VR.Interactable>().enabled = false;
                }
            }
        }*/
    }
}