using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DreamHouseStudios.SofasaLogistica {
	public class UIPocket : MonoBehaviour {
		#region Components
		public event Action OnBoxScanned;

		public event Action OnProductScanned;

		public event Action OnShelfScanned;

		[SerializeField] private Text pocketText;

		[SerializeField] private LineRenderer scannerPoint;

		private BoxInvoice boxInvoice;

		private ShelfInvoice shelfInvoice;

		private Dictionary<string, int> products;
		[SerializeField] PocketData pocketData;

		private bool isScanning;
		#endregion

		#region Unity Functions
		private void Awake () {
			products = new Dictionary<string, int> ();

			if (pocketData) {
				pocketData.boxes.Clear ();
				pocketData.products.Clear ();
			}
		}
		#endregion

		#region Functions
		public void Show () {
			pocketText.text = "Escanea un elemento";
			gameObject.SetActive (true);
		}

		public void TryScanInvoice () {
			RaycastHit[] hits = Physics.SphereCastAll (scannerPoint.transform.position, 0.15f, scannerPoint.transform.forward, 3f);

			isScanning = true;
			StartCoroutine (ScanRay ());

			if (hits.Length == 0) {
				pocketText.text = "No has escaneado ningun elemento";
				return;
			}

			ProductInvoice invoice = null;
			foreach (RaycastHit hit in hits) {
				if (boxInvoice == null && hit.collider.transform.GetComponent<BoxInvoice> ()) {
					boxInvoice = hit.collider.transform.GetComponent<BoxInvoice> ();
					if (OnBoxScanned != null)
						OnBoxScanned ();

					if (pocketData)
						pocketData.boxes.Add (boxInvoice.Data);
					pocketText.text = string.Format ("Caja escaneada:\nId: {0}\nCantidad:{1}", boxInvoice.Data.boxId, boxInvoice.Data.TotalQuantity);
				} else if (hit.collider.transform.TryGetComponent (out invoice))
					RegisterProductInvoice (invoice.Product);
				else if (hit.collider.transform.GetComponent<ShelfInvoice> () != null) {
					shelfInvoice = hit.collider.transform.GetComponent<ShelfInvoice> ();
					RegisterShelf (shelfInvoice.Data);
				}
			}
		}

		private IEnumerator ScanRay () {
			while (isScanning) {
				scannerPoint.SetPosition (0, scannerPoint.transform.position);
				scannerPoint.SetPosition (1, scannerPoint.transform.position + (scannerPoint.transform.forward * 3));

				yield return null;
			}
		}

		public void StopScan () {
			scannerPoint.SetPosition (0, Vector3.zero);
			scannerPoint.SetPosition (1, Vector3.zero);

			isScanning = false;
		}

		private void RegisterShelf (ShelfData shelf) {
			if (OnShelfScanned != null)
				OnShelfScanned ();

			pocketText.text = "Estante escaneado, código: " + shelf.shelfId;
		}

		private void RegisterProductInvoice (ProductData product) {
			if (boxInvoice == null) {
				pocketText.text = "No hay una caja registrada sobre la cual hacer inventario";
				return;
			}
			if (shelfInvoice != null) {
				if (shelfInvoice.Data.shelfId == product.shelfId) {
					if (products.ContainsKey (product.productId)) {
						if (products[product.productId] > 0) {
							products[product.productId]--;

							if (pocketData)
								pocketData.products.Add (product);

							pocketText.text = string.Format ("Producto: {0}\nEstante: {1}\nCantidad pendiente: {2}", product.productId, product.shelfId, products[product.productId]);
						} else
							pocketText.text = string.Format ("Se han descargado todas las unidades del producto '{0}'", product.productId);
					} else
						pocketText.text = string.Format ("Producto: {0} no esta registrado en el pocket", product.productId);
				} else
					pocketText.text = string.Format ("El producto '{0}' no pertenece a ese estante", product.productId);

				return;
			}
			if (!products.ContainsKey (product.productId))
				products.Add (product.productId, 1);
			else
				products[product.productId]++;

			pocketText.text = string.Format ("Producto: {0}\nEstante: {1}\nCantidad registrada actualmente: {2}", product.productId, product.shelfId, products[product.productId]);

			if (OnProductScanned != null)
				OnProductScanned ();
		}

		public void Hide () {
			StopScan ();
			gameObject.SetActive (false);
		}
		#endregion
	}
}