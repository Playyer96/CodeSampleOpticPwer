using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ManageDinamicProducts : MonoBehaviour
{
    public ReceptionManager R_ReceptionManager;
    public LocationManager L_LocationManager;

    private void Start()
    {
        R_ReceptionManager = FindObjectOfType<ReceptionManager>();
        L_LocationManager = FindObjectOfType<LocationManager>();
    }

    /*
    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            DisabeProducts();
        }
    }
    */

    public void DisabeProducts()
    {
        if (R_ReceptionManager)
            for (int i = 0; i < R_ReceptionManager.productInvoices.Count; i++)
            {
                R_ReceptionManager.productInvoices[i].transform.parent.GetComponent<DreamHouseStudios.VR.Interactable>().enabled = false;
            }

        if (L_LocationManager)
            for (int i = 0; i < L_LocationManager.productInvoices.Count; i++)
            {
                L_LocationManager.productInvoices[i].transform.parent.GetComponent<DreamHouseStudios.VR.Interactable>().enabled = false;
            }
    }

    public void EnableAll()
    {
        if (R_ReceptionManager)
            for (int i = 1; i < R_ReceptionManager.productInvoices.Count; i++)
            {
                R_ReceptionManager.productInvoices[i].transform.parent.GetComponent<DreamHouseStudios.VR.Interactable>().enabled = true;
            }

        if (L_LocationManager)
            for (int i = 1; i < L_LocationManager.productInvoices.Count; i++)
            {
                L_LocationManager.productInvoices[i].transform.parent.GetComponent<DreamHouseStudios.VR.Interactable>().enabled = true;
            }
    }

    public void ManageProducts(int i_Index)
    {
        if (R_ReceptionManager)
            for (int i = 0; i < R_ReceptionManager.productInvoices.Count; i++)
            {
                if (i != i_Index)
                {
                    R_ReceptionManager.productInvoices[i].transform.parent.GetComponent<DreamHouseStudios.VR.Interactable>().enabled = false;
                }
                else
                {
                    R_ReceptionManager.productInvoices[i].transform.parent.GetComponentInParent<DreamHouseStudios.VR.Interactable>().enabled = true;
                }
            }

        if (L_LocationManager)
            for (int i = 0; i < L_LocationManager.productInvoices.Count; i++)
            {
                if (i != i_Index)
                {
                    L_LocationManager.productInvoices[i].transform.parent.GetComponent<DreamHouseStudios.VR.Interactable>().enabled = false;
                }
                else
                {
                    L_LocationManager.productInvoices[i].transform.parent.GetComponentInParent<DreamHouseStudios.VR.Interactable>().enabled = true;
                }
            }
    }
}
