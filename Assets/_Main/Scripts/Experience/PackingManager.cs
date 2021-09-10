using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace DreamHouseStudios.SofasaLogistica
{
    public class PackingManager : MonoBehaviour
    {
        #region Components

        public int totalItemsToTrack = 0;

        [SerializeField] private SpectraUISettings settings;
        [SerializeField] private TestPocketPacking pocketPacking;

        [Space(10f), Header("Packing Items")] [SerializeField]
        private List<ReferenceState> r_states = null;

        public List<ReferenceState> r_CustomAcctions = null;

        [Space(10f), Header("Items to Scan")] public List<ProductInvoice> productInvoices = null;

        public BoxProductsHolder boxProductsHolder;

        public UnityEvent e_OnReferenceStateComplete;

        private float progress = 0f;
        private float totalProgress = 0f;
        private string sectionTime;
        private bool alreadyVisited = false;
        private bool productsScanned = false;
        private bool packingDone = false;
        private bool rs_Done = false;

        public bool visited => alreadyVisited;
        public bool ProductsScanned => productsScanned;
        public bool PackingDone => packingDone;
        public float TotalProgress => totalProgress;

        private IEnumerator packingChecker;

        private WaitForEndOfFrame waitForEndOfFrame;
        private WaitForSeconds quarterSecond;
        private WaitForSeconds twoSeconds;


        private bool boxFilled = false;
        #endregion

        #region Unity Functions

        private void Start()
        {
            Init();

            StartPackingChecker();
            ExperienceUI.instance.packingProgress = 0;
        }

        #endregion

        #region Functions

        public void Init()
        {
            quarterSecond = new WaitForSeconds(0.25f);
            twoSeconds = new WaitForSeconds(2f);
            waitForEndOfFrame = new WaitForEndOfFrame();
            alreadyVisited = true;
            packingDone = false;

            if (e_OnReferenceStateComplete == null)
            {
                e_OnReferenceStateComplete = new UnityEvent();
            }
            totalItemsToTrack += r_states.Count;
            totalItemsToTrack += productInvoices.Count;
            totalItemsToTrack += r_CustomAcctions.Count;
            totalItemsToTrack ++;
        }

        public void StartPackingChecker()
        {
            if (packingChecker != null)
                StopCoroutine(packingChecker);

            packingChecker = PackingChecker();
            StartCoroutine(packingChecker);
        }

        public void StopPackingChecker()
        {
            if (packingChecker != null)
                StopCoroutine(packingChecker);
        }

        private void ProgressCounter(bool sum)
        {
            if (sum)
                progress += (100f / totalItemsToTrack);
            else
                progress -= (100f / totalItemsToTrack);
        }

        private void ReferenceStateHandler(ReferenceState referenceState, bool dynamic)
        {
            if (referenceState && referenceState.IsInOrder && referenceState.progressSet == false)
            {
                referenceState.progressSet = true;
                ProgressCounter(true);
            }
            else if (dynamic && referenceState && !referenceState.IsInOrder && referenceState.progressSet == true)
            {
                referenceState.progressSet = false;
                ProgressCounter(false);
            }
        }

        IEnumerator PackingChecker()
        {
            yield return waitForEndOfFrame;
            bool killWhenComplete = true;
            while (killWhenComplete)
            {
                yield return quarterSecond;

                if (progress >= 99)
                    totalProgress = Mathf.Round(progress);
                else
                    totalProgress = progress;

                ExperienceUI.instance.packingProgress = totalProgress;

                for (int i = 0; i < r_states.Count; i++)
                {
                    if (r_states[i])
                    {
                        ReferenceStateHandler(r_states[i], true);
                    }
                }

                for (int i = 0; i < r_CustomAcctions.Count; i++)
                {
                    ReferenceStateHandler(r_CustomAcctions[i], false);
                }

                if (settings.experienMode == ExperienMode.Entrenamiento)
                {
                    for (int i = 0; i < r_states.Count; i++)
                    {
                        //if (r_states[i].isInOrder && !rs_Done)
                        if (r_states.All(refState => refState.isInOrder) && !rs_Done)
                        {
                            // Enviar evento de estados de referencia en orden
                            e_OnReferenceStateComplete.Invoke();
                            rs_Done = true;
                        }
                    }
                }

                if (boxProductsHolder.allProductsOnBox && !boxFilled)
                {
                    //int productCounter = 0;
                    //if (boxProductsHolder.productsOnBox != productCounter)
                    //{
                    //    productCounter++;
                        ProgressCounter(true);
                        boxFilled = true;
                        //}
                }

                if (totalProgress >= 99.5f)
                {
                    totalProgress = Mathf.Round(progress);

                    /*switch (settings.experienMode)
                    {
                        case ExperienMode.Evaluacion:
                            if (settings.experienMode == ExperienMode.Evaluacion)
                                ScenesManager.instance.StartLoadScene(ScenesManager.instance.menuScene);
                            break;
                        case ExperienMode.Entrenamiento:
                            if (settings.experienMode == ExperienMode.Evaluacion)
                            {
                                yield return new WaitForSeconds(10f);
                                ScenesManager.instance.StartLoadScene(ScenesManager.instance.menuScene);
                            }

                            break;
                    }

                    packingDone = true;*/
                }

                if (packingDone) killWhenComplete = false;
            }
        }

        #endregion
    }
}