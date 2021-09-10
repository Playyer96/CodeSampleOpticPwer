using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

namespace DreamHouseStudios.SofasaLogistica
{
    public class ReceptionManager : MonoBehaviour
    {
        #region Components

        public int totalItemsToTrack = 0;
        [SerializeField] private SpectraUISettings settings;

        [Space(10f), Header("Boxes")] [SerializeField]
        private List<UnpackBox> boxes = null;

        [Space(10f), Header("Reception Items")] [SerializeField]
        private List<ReferenceState> r_states = null;

        [SerializeField] private ReferenceState r_HookBox = null;

        public List<ItemID> tapes = null;
        [SerializeField] List<ShelfDetection> shelves = null;

        [Space(10f), Header("Items to Scan")] public BoxInvoice boxInvoices = null;
        public List<ProductInvoice> productInvoices = null;

        [Space(10f), Header("Photos")] public CameraPointer cameraPointer;

        public ShelfPacakage shelfPacakage;

        public CheckRightCan invoiceResidue = null;

        private float _progress = 0f;
        private float _totalProgress = 0f;

        private string _sectionTime;

        private bool b_tookPhotoIn = false;
        private bool b_tookPhotoOut = false;

        private bool _alreadyVisited = false;
        private bool _productsScanned = false;
        private bool _receptionDone = false;

        IEnumerator _receptionChecker;
        IEnumerator _checkIfCompleted;
        WaitForEndOfFrame _waitForEndOfFrame;
        WaitForSeconds _quarterSecond;

        public bool visited => _alreadyVisited;

        public bool ReceptionDone => _receptionDone;

        public float TotalProgress => _totalProgress;
        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            Init();

            //StartReceptionChecker();
        }

        private void Start()
        {
            StartReceptionChecker();
            ExperienceUI.instance.receptionProgress = 0;
        }

        #endregion

        #region Funcitons

        public void Init()
        {
            _alreadyVisited = true;
            _receptionDone = false;
            _waitForEndOfFrame = new WaitForEndOfFrame();
            _quarterSecond = new WaitForSeconds(0.25f);

            if (cameraPointer)
                totalItemsToTrack += 2;
            
            if (shelfPacakage)
                totalItemsToTrack++;

            if (boxes.Count > 0)
                totalItemsToTrack++;

            if (invoiceResidue)
                totalItemsToTrack++;

            totalItemsToTrack += tapes.Count;
            totalItemsToTrack += r_states.Count;
            totalItemsToTrack += shelves.Count;
            totalItemsToTrack += productInvoices.Count;
        }

        private void ProgressCounter(bool sum)
        {
            if (sum)
                _progress += (100f / totalItemsToTrack);
            else
                _progress -= (100f / totalItemsToTrack);
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

        private void TapeChecker(ItemID tape)
        {
            if (tape && tape.isSet && !tape.progressSet && tape.GetComponent<CheckRightCan>().trashDestroy)
            {
                tape.progressSet = true;
                ProgressCounter(true);
            }
        }

        public void StartReceptionChecker()
        {
            if (_receptionChecker != null)
                StopCoroutine(_receptionChecker);

            _receptionChecker = ReceptionChecker();
            StartCoroutine(_receptionChecker);

            if (_checkIfCompleted != null)
                StopCoroutine(_checkIfCompleted);

            _checkIfCompleted = CheckIfCompleted();
            StartCoroutine(_checkIfCompleted);
        }

        public void StopReceptionChecker()
        {
            if (_receptionChecker != null)
                StopCoroutine(_receptionChecker);

            if (_checkIfCompleted != null)
                StopCoroutine(_checkIfCompleted);
        }

        IEnumerator CheckIfCompleted()
        {
            yield return _waitForEndOfFrame;
            bool killOnCompleted = true;
            while (killOnCompleted)
            {
                yield return _quarterSecond;
                if (productInvoices.All(parts => parts.Scanned))
                {
                    _productsScanned = true;
                    if (_productsScanned) killOnCompleted = false;
                }
            }
        }

        IEnumerator ReceptionChecker()
        {
            yield return _waitForEndOfFrame;
            bool killOnCompleted = true;
            while (killOnCompleted)
            {
                yield return _quarterSecond;

                if (_progress >= 99)
                    _totalProgress = Mathf.Round(_progress);
                else
                    _totalProgress = _progress;

                ExperienceUI.instance.receptionProgress = _totalProgress;

                if (cameraPointer.b_In && !b_tookPhotoIn)
                {
                    b_tookPhotoIn = true;
                    ProgressCounter(true);
                }

                if (cameraPointer.b_Out && !b_tookPhotoOut)
                {
                    b_tookPhotoOut = true;
                    ProgressCounter(true);
                }

                foreach (var t in boxes)
                {
                    if (t && t.gameObject.activeInHierarchy)
                    {
                        if (t.IsBoxOpen && t.progressSet == false)
                        {
                            t.progressSet = true;
                            ProgressCounter(true);
                        }
                    }
                }

                foreach (var t in productInvoices)
                {
                    if (t)
                    {
                        if (!t.progressIsSet)
                        {
                            if (t.Scanned)
                            {
                                ProgressCounter(true);
                                t.progressIsSet = true;
                            }
                           
                        }
                    }
                }

                if (boxInvoices)
                {
                    if (boxInvoices.Scanned && !boxInvoices.progressIsSet)
                    {
                        boxInvoices.progressIsSet = true;
                        ProgressCounter(true);
                    }
                }

                if (shelfPacakage)
                {
                    if (shelfPacakage.allBagsOnShelf && !shelfPacakage.progressIsSet)
                    {
                        shelfPacakage.progressIsSet = true;
                        ProgressCounter(true);
                    }
                }

                for (int i = 0; i < r_states.Count; i++)
                {
                    if (r_states[i])
                    {
                        ReferenceStateHandler(r_states[i], true);
                    }
                }

                if (r_HookBox)
                {
                    if (boxes[1].gameObject.activeInHierarchy)
                    {
                        ReferenceStateHandler(r_HookBox, true);
                    }
                }

                for (int i = 0; i < tapes.Count; i++)
                {
                    if (tapes[i])
                        TapeChecker(tapes[i]);
                }

                if (!invoiceResidue.progressSet && invoiceResidue.trashDestroy)
                {
                    invoiceResidue.progressSet = true;
                    ProgressCounter(true);
                }

                for (int i = 0; i < shelves.Count; i++)
                {
                    if (shelves[i] && shelves[i].g_Colliders.Count == 4 && !shelves[i].progressSet)
                    {
                        ProgressCounter(true);
                        shelves[i].progressSet = true;
                    }
                }

                if (_totalProgress >= 99.5f && !ScenesManager.instance.locationScene.visited)
                {
                    _totalProgress = Mathf.Round(_progress);

                    /*switch (settings.experienMode)
                    {
                        case ExperienMode.Evaluacion:
                            //ScenesManager.instance.StartLoadScene(ScenesManager.instance.locationScene.sceneName);
                            break;
                        case ExperienMode.Entrenamiento:
                            yield return new WaitForSeconds(10f);
                            ScenesManager.instance.StartLoadScene(ScenesManager.instance.locationScene.sceneName);

                            break;
                    }*/

                    _receptionDone = true;
                }

                if (_receptionDone) killOnCompleted = false;
            }
        }

        #endregion
    }
}