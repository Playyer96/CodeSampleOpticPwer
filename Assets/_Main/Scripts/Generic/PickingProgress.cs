using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingProgress : MonoBehaviour
{
    public PocketPickingProducts ppp;

    private void Start()
    {
        ppp = FindObjectOfType<PocketPickingProducts>();
    }
    
    
}
