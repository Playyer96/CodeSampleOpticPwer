using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica
{
    public class LocationMobileShelf : MonoBehaviour
    {
        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<VR.Interactable>()) return;
            
            if (other.GetComponent<VR.Interactable>().beingGrabbed)
            {
                if (other.transform.parent != null)
                    other.transform.SetParent(_transform);
            }
            else
                other.transform.SetParent(_transform);
        }
    }
}