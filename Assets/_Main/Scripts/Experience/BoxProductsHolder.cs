using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DreamHouseStudios.SofasaLogistica
{
    public class BoxProductsHolder : MonoBehaviour
    {
        #region Components

        public List<ProductInvoice> productsObjs = new List<ProductInvoice>();
        public int productsOnBox = 0;
        public TestPocketPacking testPocketPacking = null;

        public UnityEvent e_ProductsOnBox;
        public UnityEvent e_OneProdutStored;

        private Transform _transform;
        public bool allProductsOnBox = false;

        #endregion Components

        #region Unity Functions

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Start()
        {
            testPocketPacking = FindObjectOfType<TestPocketPacking>();

            if (e_ProductsOnBox == null)
            {
                e_ProductsOnBox = new UnityEvent();
            }

            if (e_OneProdutStored == null)
            {
                e_OneProdutStored = new UnityEvent();
            }
        }

        public void OnClosedBox()
        {
            /*
            for (int i = 0; i < productsObjs.Count; i++)
            {
                productsObjs[i].gameObject.SetActive(false);
            }
            */
            StartCoroutine(CloseBox());
        }

        private IEnumerator CloseBox()
        {
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < productsObjs.Count; i++)
            {
                productsObjs[i].gameObject.transform.parent.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(1f);
        }

        private void Update()
        {
            if (testPocketPacking && productsOnBox == testPocketPacking.productsOnScene.Count && !allProductsOnBox)
            {
                e_ProductsOnBox.Invoke();
                allProductsOnBox = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            //if (testPocketPacking && productsObjs.Count == testPocketPacking.g_Products.Length) return;

            if (other.CompareTag("StickPoint"))
            {
                if (other.GetComponent<VR.Interactable>() != null &&
                    !other.GetComponent<VR.Interactable>().beingGrabbed)
                {
                    other.transform.SetParent(_transform);

                    if (other.GetComponent<Collider>())
                        other.GetComponent<Collider>().enabled = false;

                    if (other.GetComponent<Rigidbody>())
                    {
                        other.GetComponent<Rigidbody>().useGravity = false;
                        other.GetComponent<Rigidbody>().isKinematic = true;
                    }

                    if (other.GetComponent<VR.Interactable>())
                        other.GetComponent<VR.Interactable>().enabled = false;
                }

                if (productsObjs.Contains(other.gameObject.GetComponentInChildren<ProductInvoice>()))
                {
                    return;
                }
                other.gameObject.GetComponentInChildren<ProductInvoice>().storedInBox = true;
                e_OneProdutStored.Invoke();
                productsObjs.Add(other.gameObject.GetComponentInChildren<ProductInvoice>());
                productsOnBox++;
            }
        }

        #endregion Unity Functions
    }
}