using System;
using UnityEngine.Events;
using UnityEngine;

public class TriggerPhoto : MonoBehaviour
{
    public UnityEvent e_OnExitProduct;

    private void Start()
    {
        if (e_OnExitProduct == null)
        {
            e_OnExitProduct = new UnityEvent();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<Bag_Shelf>() != null)
        {
            e_OnExitProduct.Invoke();
        }
    }
}
