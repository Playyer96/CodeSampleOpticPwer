using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica
{
    public class ProductInvoice : MonoBehaviour
    {
        #region Components

        public ProductData Product = null;

        private bool scanned = false;

        [HideInInspector] public bool progressIsSet = false;
        public bool Scanned { get { return scanned; } }
        public bool storedInBox = false;

        #endregion Components

        #region Functions

        public void SetBool(bool value)
        {
            scanned = value;
        }

        #endregion Functions
    }
}