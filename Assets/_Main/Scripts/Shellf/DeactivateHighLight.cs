using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateHighLight : MonoBehaviour
{
    public GameObject g_HighLight;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hand")
        {
            if (g_HighLight.activeInHierarchy)
            {
                g_HighLight.SetActive(false);
            }
        }
    }
}
