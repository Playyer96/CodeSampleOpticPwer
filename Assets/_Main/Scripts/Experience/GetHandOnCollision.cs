using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHandOnCollision : MonoBehaviour
{
    public Transform t_DesirePosition;
    public DetectControllersScriptable detectControllersScriptable;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            switch (detectControllersScriptable.lastEnabledHandler)
            {
                case DetectControllers.HandsHandler.HI5:
                    t_DesirePosition = other.transform;
                    break;
                case DetectControllers.HandsHandler.Vive_Controller:
                    t_DesirePosition = other.transform;
                    break;
            }
        }
    }
}
