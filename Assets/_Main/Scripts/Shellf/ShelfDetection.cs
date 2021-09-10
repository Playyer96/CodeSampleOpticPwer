using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfDetection : MonoBehaviour
{

    [HideInInspector] public bool progressSet = false;
    public List<GameObject> g_Colliders;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Whells")
        {
            if(!g_Colliders.Contains(other.gameObject))
            {
                g_Colliders.Add(other.gameObject);
            }
            CounterShelf.Instance.GetInts();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Whells")
        {
            if(g_Colliders.Contains(other.gameObject))
            {
                g_Colliders.Remove(other.gameObject);
            }
            CounterShelf.Instance.GetInts();
        }
    }
}
