using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceRelease : MonoBehaviour
{
    public DreamHouseStudios.VR.Interactable i_Interactable;

    private void Start()
    {
        i_Interactable = GetComponent<DreamHouseStudios.VR.Interactable>();
    }

    public void V_ForceRelease()
    {
        i_Interactable.onRelease.Invoke(i_Interactable);
        gameObject.SetActive(false);
    }
}
