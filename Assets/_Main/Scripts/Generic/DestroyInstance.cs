using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInstance : MonoBehaviour
{
   
    void Start()
    {
        if(CalibrateManager.Instance != null)
            Destroy(CalibrateManager.Instance.gameObject);
    }
}
