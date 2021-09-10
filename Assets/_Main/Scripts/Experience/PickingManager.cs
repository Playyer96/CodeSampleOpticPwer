using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DreamHouseStudios.SofasaLogistica
{
    public class PickingManager : MonoBehaviour
    {
        #region Components
        [SerializeField] List<GameObject> items = new List<GameObject>();
        [SerializeField] List<GameObject> invoices = new List<GameObject>();
        [SerializeField] ProductInvoice[] productInvoices;

        [SerializeField] GameObject teleports;

        float progress = 0f;
        float totalProgress = 0f;
        bool alreadyVisited = false;
        bool productsScanned = false;
        bool pickingDone = false;

        string sectionTime;
        public bool visited { get { return alreadyVisited; } }
        public bool ProductsScaneed { get { return productsScanned; } }
        public bool PickingDone { get { return pickingDone; } }
        public float TotalProgress { get { return totalProgress; } }

        IEnumerator checkIfCompleted;
        IEnumerator pickingChecker;
        WaitForEndOfFrame waitForEndOfFrame;
        WaitForSeconds twoSeconds;
        #endregion

        #region Unity Functions
        private void OnEnable()
        {
            Init();
        }
        #endregion

        #region Functions
        public void Init()
        {
            teleports.SetActive(true);
            waitForEndOfFrame = new WaitForEndOfFrame();
            twoSeconds = new WaitForSeconds(2f);
            alreadyVisited = true;
            pickingDone = false;
            ShowHide(false);
        }
        public void ShowHide(bool value)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetActive(value);
            }
            for (int i = 0; i < invoices.Count; i++)
            {
                invoices[i].SetActive(value); 
            }
        }

        public void StartPickingChecker()
        {
            if (pickingChecker != null)
                StopCoroutine(pickingChecker);

            pickingChecker = PickingChecker();
            StartCoroutine(pickingChecker);

            if (checkIfCompleted != null)
                StopCoroutine(checkIfCompleted);

            checkIfCompleted = CheckIfCompleted();
            StartCoroutine(checkIfCompleted);
        }

        public void StopPickingChecker()
        {
            teleports.SetActive(false);
            if (pickingChecker != null)
                StopCoroutine(pickingChecker);

            if (checkIfCompleted != null)
                StopCoroutine(checkIfCompleted);
        }

        private void ProgressCounter()
        {
            progress += 11.11f;
        }

        IEnumerator CheckIfCompleted()
        {
            yield return waitForEndOfFrame;
            bool killOnCompleted = true;
            while (killOnCompleted)
            {
                yield return twoSeconds;
                if (productInvoices.All(parts => parts.Scanned))
                {
                    productsScanned = true;
                    ProgressCounter();
                    if (productsScanned) killOnCompleted = false;
                }
            }
        }

        IEnumerator PickingChecker()
        {
            yield return waitForEndOfFrame;
            bool killWhenComplete = true;
            while (killWhenComplete)
            {
                yield return waitForEndOfFrame;
                killWhenComplete = false;
            }
        }
        #endregion
    }
}