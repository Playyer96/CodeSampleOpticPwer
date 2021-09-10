using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica {
	public class BoxInvoice : MonoBehaviour {
		#region Components
		public BoxData Data;

        [HideInInspector] public bool progressIsSet = false;
		bool scanned = false;

		public bool Scanned { get { return scanned; } }
		#endregion

		#region Functions
		public void SetBool (bool value) {
			scanned = value;
		}
		#endregion
	}
}