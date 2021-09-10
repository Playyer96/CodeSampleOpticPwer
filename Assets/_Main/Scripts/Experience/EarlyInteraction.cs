using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarlyInteraction : MonoBehaviour
{
    [SerializeField] Color normalColor = Color.black;
    [SerializeField] Color colColor = Color.white;

    new Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hand")
        {
            renderer.material.SetColor("Color_5A35FE0D", colColor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Hand")
        {
            renderer.material.SetColor("Color_5A35FE0D", normalColor);
        }
    }
}
