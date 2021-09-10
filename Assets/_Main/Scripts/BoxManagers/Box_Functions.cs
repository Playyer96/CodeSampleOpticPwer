using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box_Functions : MonoBehaviour
{
    public BoxManager b_BoxManager;
        void Start()
    {
        b_BoxManager = FindObjectOfType<BoxManager>();    
    }

    public void SetBoxes()
    {
        b_BoxManager.HideBoxes();
        b_BoxManager.ActiveBoxInPlace();
        b_BoxManager.InstantiateProducts();
    }
}
