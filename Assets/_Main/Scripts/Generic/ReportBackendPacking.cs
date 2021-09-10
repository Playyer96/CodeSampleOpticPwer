using System.Collections;
using System.Collections.Generic;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;

public class ReportBackendPacking : MonoBehaviour
{
    public ReportBackend[] rb;
    public ReportBackend referencestate;
    public ReferenceState[] states;

    public BoxProductsHolder holder;
    public ReportBackend rb_products;

    public void SetBool()
    {
        for (int i = 0; i < rb.Length; i++)
        {
            rb[i].GetBool();
        }
    }

    public void SetReferenceState()
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (!states[i].isInOrder)
            {
                referencestate.isReported = false;
                return;
            }
        }

        referencestate.isReported = true;
    }

    public void SetBoolProducts()
    {
        rb_products.isReported = holder.allProductsOnBox;
    }

    public void SetReport()
    {
        SetBool();
        SetReferenceState();
        SetBoolProducts();
    }
}
