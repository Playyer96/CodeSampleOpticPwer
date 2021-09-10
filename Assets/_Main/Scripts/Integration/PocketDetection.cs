using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketDetection : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // if (!other.GetComponent<CanvasInteractor>()) return;
        Debug.Log("<color=green>Enter</color>" + other.name);
        CanvasInteractor ci = other.GetComponent<CanvasInteractor>();
        ci.canInterac = true;
    }
    private void OnTriggerExit(Collider other)
    {
        // if (!other.GetComponent<CanvasInteractor>()) return;
        CanvasInteractor ci = other.GetComponent<CanvasInteractor>();
        ci.canInterac = false;
        Debug.Log("<color=red>Exit</color>");
    }
}
