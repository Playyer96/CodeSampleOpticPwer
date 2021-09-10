using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;

namespace DreamHouseStudios.SofasaLogistica
{
    public class ShelvesManager : MonoBehaviour
    {
        #region Components

        [SerializeField] private LocationManager locationManager;
        [SerializeField] private ShelfAreaDetector[] shelfAreaDetectors = null;
        [SerializeField] private Shelf[] shelves = null;

        private bool shelvesFilled = false;
        private IEnumerator _checkShelves;
        private WaitForEndOfFrame _waitForEndOfFrame;
        private WaitForSeconds _quarterSecond;

        [Serializable]
        public class Shelf
        {
            public string shelfId = null;
            public List<ReceptionInvoice> products = null;
            public int totalProductsOnShelf = 0;
            public bool allBagsOnShelf = false;
        }

        #endregion

        #region Unity Functions

        private void Awake()
        {
            // locationManager = FindObjectOfType<LocationManager>();

            _waitForEndOfFrame = new WaitForEndOfFrame();
            _quarterSecond = new WaitForSeconds(0.25f);
            if (shelfAreaDetectors.Length <= 0) return;
            shelves = new Shelf[shelfAreaDetectors.Length];
            for (var i = 0; i < shelfAreaDetectors.Length; i++)
            {
                shelves[i] = new Shelf {shelfId = shelfAreaDetectors[i].shelfInvoice.Data.shelfId};
            }
        }

        private void Start()
        {

            Invoke("AfterStart", 2f);
        }

        #endregion

        #region Functions

        private void StartCheckShelves()
        {
            if (_checkShelves != null)
                StopCoroutine(_checkShelves);

            _checkShelves = CheckShelves();
            StartCoroutine(_checkShelves);
        }

        private void AfterStart()
        {
            for (int j = 0; j < shelves.Length; j++)
            {
                for (int i = 0; i < locationManager.receptionInvoices.Count; i++)
                {
                    print(locationManager.receptionInvoices[i].Product.shelfId);
                    if (shelves[j].shelfId == locationManager.receptionInvoices[i].Product.shelfId)
                    {
                        shelves[j].totalProductsOnShelf++;
                    }
                }
            }

            if (shelves.Length == shelfAreaDetectors.Length)
                StartCheckShelves();
        }

        IEnumerator CheckShelves()
        {
            yield return _waitForEndOfFrame;
            bool killWhenComplete = true;
            while (killWhenComplete)
            {
                yield return _quarterSecond;
                if (shelves != null)
                {
                    for (int i = 0; i < shelves.Length; i++)
                    {
                        if (shelves[i].products.Count == shelves[i].totalProductsOnShelf)
                            shelves[i].allBagsOnShelf = true;
                        else if (shelves.Any() == false)
                            shelves[i].allBagsOnShelf = true;
                    }
                }

                yield return _waitForEndOfFrame;
                if (shelvesFilled) killWhenComplete = false;
            }
        }

        #endregion
    }
}