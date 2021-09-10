using System;
using System.Collections;
using System.Collections.Generic;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;

public class CheckRightCan : MonoBehaviour
{
    public TrashcanType trashcanType;

    [HideInInspector] public bool trashDestroy;
    [HideInInspector] public bool progressSet;
    public ReportBackend rb;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DestroyOnCollision>())
        {
            if (other.GetComponent<DestroyOnCollision>().trashcanType == trashcanType)
            {
                trashDestroy = true;
                rb.CounterActions(1);
            }
        }
    }
}
