using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCol : MonoBehaviour
{
    [SerializeField] private List<GameObject> products = new List<GameObject>();
    [SerializeField] private List<GameObject> OnCartProducts = new List<GameObject>();
    
    void Start()
    {
        for (int i = 0; i < products.Count; i++)
        {
            OnCartProducts[i].SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StickPoint"))
        {
            for (int i = 0; i < products.Count; i++)
            {
                if (other.gameObject == products[i])
                {
                    products[i].SetActive(false);
                    OnCartProducts[i].SetActive(true);
                }
            }
        }
    }
}