using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica {
    public class ShelfController : MonoBehaviour {
        #region Components
        [SerializeField] string shelfId;

        [Space (10f), Header ("Product Positions")]
        [SerializeField] string[] productsIds;
        [SerializeField] GameObject[] productsPos;
        private Dictionary<string, GameObject> products = new Dictionary<string, GameObject> ();

        #endregion

        #region Unity Functions
        private void Start () {
            Init ();
        }

        private void OnTriggerEnter (Collider other) {
            if (other.gameObject.tag == "Product") {
                ProductInvoice productInvoice = other.gameObject.GetComponentInChildren<ProductInvoice> ();
                if (shelfId == productInvoice.Product.shelfId) {
                    Debug.LogWarningFormat ("The Product is not in the correct shelf");
                    return;
                } else {
                    for (int i = 0; i < products.Count; i++) {
                        if (productInvoice.Product.productId == products.Keys.ToString ()) {
                            if (other.bounds.Contains (productsPos[i].transform.position)) {
                                Debug.Log ("Product place in the correct position");
                                productInvoice.transform.SetParent (productsPos[i].transform);
                                productInvoice = null;
                            } else
                                Debug.LogWarning ("Product place incorrectly");
                            Debug.Log ("The product is in the correct shelf");
                        } else
                            Debug.LogWarning ("Product place incorrectly");
                    }
                }
                if (productInvoice != null)
                    productInvoice = null;
            }
        }

        private void OnTriggerExit (Collider other) {
            if (other.gameObject.tag == "Product") {
                ProductInvoice productInvoice = other.gameObject.GetComponentInChildren<ProductInvoice> ();

                productInvoice.transform.SetParent (null);

                if (productInvoice != null)
                    productInvoice = null;
                else
                    return;
            }
        }
        #endregion

        #region Functions
        private void Init () {
            if (productsIds.Length != productsPos.Length) return;
            for (int i = 0; i < productsIds.Length; i++) {
                products.Add (productsIds[i], productsPos[i]);
            }
        }

        private void Destroy () {
            for (int i = 0; i < productsIds.Length; i++) {
                products.Remove (productsIds[i]);
            }
        }

        #endregion
    }
}