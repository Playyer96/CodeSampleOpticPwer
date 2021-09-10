using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica
{
    public class ShelfInvoice : MonoBehaviour
    {
        #region Components

        public ShelfData Data;

        bool _scanned = false;

        [HideInInspector] public bool progressIsSet = false;

        public bool Scanned => _scanned;

        #endregion

        #region Functions

        public void SetBool(bool value)
        {
            _scanned = value;
        }

        #endregion
    }
}