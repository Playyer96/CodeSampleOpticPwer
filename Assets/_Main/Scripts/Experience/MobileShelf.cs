using System;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace DreamHouseStudios.SofasaLogistica
{
	public class MobileShelf : MonoBehaviour
	{
		public event Action OnProductInCart;

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.GetComponent<DreamHouseStudios.VR.Interactable>() != null)
			{
				if (OnProductInCart != null)
					OnProductInCart();

				collision.transform.SetParent(transform);
                collision.gameObject.GetComponent<DreamHouseStudios.VR.Interactable>().enabled = false;

                if (collision.gameObject.GetComponent<Rigidbody>())
                {
                    collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                }
			}
		}
	}
}