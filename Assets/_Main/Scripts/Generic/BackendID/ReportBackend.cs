using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;

public class ReportBackend : MonoBehaviour
{
    public bool isReported = false;
    public string report;
    
    

    public int actionsNeeded;
    public int actualActions;
    
    public void SetBool(bool value)
    {
        isReported = value;
    }

    public void CounterActions(int val)
    {
        actualActions += val;
        if (actionsNeeded == actualActions)
        {
            isReported = true;
        }
    }

    public void GetBool()
    {
        isReported = GetComponent<ReferenceState>().isInOrder;
    }
}
