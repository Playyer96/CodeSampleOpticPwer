using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica
{
    public class ReceptionInvoice : MonoBehaviour
    {
        #region Components

        public ProductData Product = null;

        [Space(10f), Header("Invoice Components")]
        [SerializeField]
        private TMPro.TextMeshProUGUI referenceText = null;

        [SerializeField] private TMPro.TextMeshProUGUI descriptionText = null;
        [SerializeField] private TMPro.TextMeshProUGUI locationText = null;
        [SerializeField] private TMPro.TextMeshProUGUI boxNumberText = null;
        [SerializeField] private TMPro.TextMeshProUGUI quantityText = null;
        [SerializeField] private TMPro.TextMeshProUGUI umText = null;

        private bool _invoiceScanned = false;
        private bool _shelfScanned = false;

        [HideInInspector] public bool progressIsSet = false;

        public bool InvoiceScanned => _invoiceScanned;

        public bool ShelfScanned => _shelfScanned;

        public bool StoredInUbication = false;

        #endregion Components

        #region Unity Functions

        private void OnEnable()
        {
            if (referenceText)
                referenceText.text = this.Product.productId;
            if (descriptionText)
                descriptionText.text = this.Product.description;
            if (locationText)
                locationText.text = this.Product.location;
            if (boxNumberText)
                boxNumberText.text = this.Product.shelfId;
            if (quantityText)
                quantityText.text = "";
            if (umText)
                umText.text = "";
        }

        #endregion Unity Functions

        #region Functions

        public void SetInvoiceScanned(bool value)
        {
            _invoiceScanned = value;
        }

        public void SetShelfScanned(bool value)
        {
            _shelfScanned = value;
        }

        #endregion Functions
    }
}