using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;
using UnityEngine.Events;

namespace DreamHouseStudios.SofasaLogistica
{
    public class LocationManager : MonoBehaviour
    {
        #region Components

        public int totalItemsToTrack = 0;
        public int totalProducts = 0;

        [SerializeField] private SpectraUISettings settings;

        [Space(10f), Header("Items to Scan")] [SerializeField]
        public Shelf[] shelves = null;

        [SerializeField] public List<ReceptionInvoice> receptionInvoices = null;
        [SerializeField] public List<ProductInvoice> productInvoices = null;
        public ShelfAreaDetector[] shelfAreaDetectors = null;
        public UnityEvent e_AllProductsOnShelves;

        public bool shelvesFilled = false;

        private float _progress = 0f;
        private float _totalProgress = 0f;
        private string _sectionTime;
        private bool _alreadyVisited = false;
        private bool _productsScanned = false;
        private bool _locationDone = false;

        public bool Visited => _alreadyVisited;
        public bool ProductsScanned => _productsScanned;
        public bool LocationDone => _locationDone;
        public float TotalProgress => _totalProgress;

        private IEnumerator _checkShelves;
        private IEnumerator _locationChecker;
        private WaitForEndOfFrame _waitForEndOfFrame;
        private WaitForSeconds _quarterSecond;

        [Serializable]
        public class Shelf
        {
            public string shelfId = null;
            public int totalProductsOnShelf = 0;
            public bool allBagsOnShelf = false;
            public bool progressIsSet = false;
        }

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            Init();
        }

        private void Start()
        {
            if (e_AllProductsOnShelves == null)
            {
                e_AllProductsOnShelves = new UnityEvent();
            }

            StartCoroutine(SetupScene());
            ExperienceUI.instance.locationProgress = 0;
        }

        #endregion

        #region Functions

        public void Init()
        {
            _progress = 0f;
            _totalProgress = 0f;
            _waitForEndOfFrame = new WaitForEndOfFrame();
            _quarterSecond = new WaitForSeconds(0.25f);
            _alreadyVisited = true;
            _locationDone = false;
        }

        private void StartCheckShelves()
        {
            if (_checkShelves != null)
                StopCoroutine(_checkShelves);

            _checkShelves = CheckShelves();
            StartCoroutine(_checkShelves);
        }

        public void StartLocationChecker()
        {
            if (_locationChecker != null)
                StopCoroutine(_locationChecker);

            _locationChecker = LocationChecker();
            StartCoroutine(_locationChecker);
        }

        public void StopLocationChecker()
        {
            if (_locationChecker != null)
                StopCoroutine(_locationChecker);
        }

        [ContextMenu("Send Progress")]
        public void SendProgress()
        {
            ProgressCounter(true);
        }

        private void ProgressCounter(bool sum)
        {
            if (sum)
                _progress += (100f / totalItemsToTrack);
            else
                _progress -= (100f / totalItemsToTrack);
        }

        IEnumerator SetupScene()
        {
            yield return _waitForEndOfFrame;
            yield return new WaitForSeconds(1f);

            if (shelfAreaDetectors.Length <= 0) yield return null;
            shelves = new Shelf[shelfAreaDetectors.Length];
            for (var i = 0; i < shelfAreaDetectors.Length; i++)
            {
                shelves[i] = new Shelf {shelfId = shelfAreaDetectors[i].shelfInvoice.Data.shelfId};
            }

            yield return _quarterSecond;

            for (int i = 0; i < receptionInvoices.Count; i++)
            {
                for (int j = 0; j < shelves.Length; j++)
                {
                    //receptionInvoices[i].GetComponentInParent<Bag_Shelf>().b_HasReceptionInvoice = true;
                    //receptionInvoices[i].GetComponentInParent<Bag_Shelf>().b_IsInShlef = true;

                    if (receptionInvoices[i].Product.shelfId == shelves[j].shelfId)
                        shelves[j].totalProductsOnShelf++;
                }
            }

            yield return _quarterSecond;

            for (int i = 0; i < shelves.Length; i++)
            {
                if (shelves[i].totalProductsOnShelf > 0)
                    totalItemsToTrack++;
            }

            yield return _quarterSecond;
            StartCheckShelves();
            StartLocationChecker();
        }

        IEnumerator CheckShelves()
        {
            yield return _waitForEndOfFrame;
            bool killWhenComplete = true;
            while (killWhenComplete)
            {
                if (shelves.All(shelf => shelf.allBagsOnShelf))
                {
                    shelvesFilled = true;
                }

                for (int i = 0; i < shelfAreaDetectors.Length; i++)
                {
                    if (shelfAreaDetectors[i].receptionInvoices.Count == shelves[i].totalProductsOnShelf)
                    {
                        shelves[i].allBagsOnShelf = true;
                    }
                    else if (shelves.Any() == false)
                    {
                        shelves[i].allBagsOnShelf = true;
                    }
                    else
                    {
                        shelves[i].allBagsOnShelf = false;
                    }
                }

                yield return _waitForEndOfFrame;
                if (shelvesFilled)
                {
                    print("Done");
                    killWhenComplete = false;
                }
            }
        }

        IEnumerator LocationChecker()
        {
            yield return _waitForEndOfFrame;
            bool killWhenComplete = true;
            while (killWhenComplete)
            {
                yield return _quarterSecond;
                if (_totalProgress >= 99f)
                {
                    _totalProgress = Mathf.Round(_progress);
                    _locationDone = true;
                }
                else
                {
                    _totalProgress = _progress;
                }

                ExperienceUI.instance.locationProgress = _totalProgress;

                for (int i = 0; i < shelves.Length; i++)
                {
                    if (shelves[i].totalProductsOnShelf > 0 && shelves[i].allBagsOnShelf && !shelves[i].progressIsSet)
                    {
                        ProgressCounter(true);
                        shelves[i].progressIsSet = true;
                    }
                    else if (shelves[i].totalProductsOnShelf > 0 && !shelves[i].allBagsOnShelf &&
                             shelves[i].progressIsSet)
                    {
                        ProgressCounter(false);
                        shelves[i].progressIsSet = false;
                    }
                }

                if (shelvesFilled)
                {
                    e_AllProductsOnShelves.Invoke();
                }

                if (_totalProgress >= 99.5f && !ScenesManager.instance.pickingScene.visited)
                {
                    _totalProgress = Mathf.Round(_progress);

                    _locationDone = true;
                }

                yield return _waitForEndOfFrame;

                if (_locationDone)
                {
                    killWhenComplete = false;
                }
            }
        }

        #endregion
    }
}