using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DreamHouseStudios.SofasaLogistica
{
    public class ShelfAreaDetector : MonoBehaviour
    {
        public ShelfInvoice shelfInvoice;
        public List<ReceptionInvoice> receptionInvoices = new List<ReceptionInvoice>();
        public UnityEvent e_ProductOnShelf;
        public UnityEvent e_CustomProducInShelf;
        public SpectraUISettings settings;

        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            shelfInvoice = GetComponentInParent<ShelfInvoice>();
        }

        private void Start()
        {
            if (e_ProductOnShelf == null)
            {
                e_ProductOnShelf = new UnityEvent();
            }

            if (e_CustomProducInShelf == null)
            {
                e_CustomProducInShelf = new UnityEvent();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.GetComponentInChildren<ReceptionInvoice>()) return;
            if (!other.gameObject.GetComponentInChildren<ProductInvoice>()) return;

            if (other.transform.parent != _transform)
            {
                if (other.GetComponent<DreamHouseStudios.VR.Interactable>() != null &&
                    !other.GetComponent<DreamHouseStudios.VR.Interactable>().beingGrabbed)
                {
                    other.transform.SetParent(_transform);
                    if (settings.experienMode == ExperienMode.Entrenamiento && other.GetComponentInChildren<ReceptionInvoice>().StoredInUbication)
                    {
                        //other.GetComponent<DreamHouseStudios.VR.Interactable>().enabled = false;
                    }
                }
            }

            if (receptionInvoices.Contains(other.GetComponentInChildren<ReceptionInvoice>()))
            {
                return;
            }

            if (other.GetComponentInChildren<ReceptionInvoice>().Product.shelfId == shelfInvoice.Data.shelfId)
            {
                /*if (other.GetComponentInChildren<ReceptionInvoice>().InvoiceScanned &&
                    other.GetComponentInChildren<ProductInvoice>().Scanned)
                {*/
                //if (Checklist.Get("Check_12", "Accion_1"))
                e_ProductOnShelf.Invoke();
                receptionInvoices.Add(other.GetComponentInChildren<ReceptionInvoice>());
                if (settings.experienMode == ExperienMode.Entrenamiento)
                {
                    other.GetComponentInChildren<ReceptionInvoice>().StoredInUbication = true;
                    //Debug.Break();
                    e_CustomProducInShelf.Invoke();
                }
                //}
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (receptionInvoices.Contains(other.GetComponentInChildren<ReceptionInvoice>()))
            {
                receptionInvoices.Remove(other.GetComponentInChildren<ReceptionInvoice>());
            }
        }

        public void OnShelf()
        {
        }
    }
}