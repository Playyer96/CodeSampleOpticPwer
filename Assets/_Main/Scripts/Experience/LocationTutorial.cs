using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DreamHouseStudios.SofasaLogistica
{
    public class LocationTutorial : Tutorials
    {
        #region Components

        [SerializeField] private AudioManager aAudio;

        public CanvasManager c_canvasManager;
        public ObjectsLogic o_ObjectsLogic;
        public bool b_Continue;
        public bool b_End;
        public PocketFlowControl p_PocketFlowControl;
        public ManageDinamicProducts m_DinamicProducs;
        public TR_Container container;
        public PocketManager p_Pocket;
        public BoxCollider[] b_Colliders;
        public int i_index = 0;
        public CheckListManager c_CM;
        public LocationManager locationManager;
        public ShelfAreaDetector[] shelfAreaDetectors;
        public Transform tutorialPos;

        private IEnumerator _setLocation;

        private WaitForEndOfFrame _waitForEndOfFrame;
        private WaitForSeconds _waitTenSeconds;
        private WaitForSeconds _waitFiveSeconds;
        private WaitForSeconds _waitFifteenSeconds;

        #endregion Components

        #region Unity Functions

        private void Start()
        {
            o_ObjectsLogic = GetComponentInChildren<ObjectsLogic>();
            c_CM = GetComponent<CheckListManager>();
            p_PocketFlowControl = GetComponent<PocketFlowControl>();
            m_DinamicProducs = GetComponent<ManageDinamicProducts>();
            p_Pocket = FindObjectOfType<PocketManager>();
            container = GetComponent<TR_Container>();

            shelfAreaDetectors = FindObjectsOfType<ShelfAreaDetector>();

            _waitForEndOfFrame = new WaitForEndOfFrame();
            _waitFiveSeconds = new WaitForSeconds(5f);
            _waitTenSeconds = new WaitForSeconds(10f);
            _waitFifteenSeconds = new WaitForSeconds(15f);

            switch (settings.experienMode)
            {
                case ExperienMode.Entrenamiento:
                    SetColliders(-1);

                    canvasManager.followPos = tutorialPos;

                    StartCoroutine(LocationTutorials());

                    for (int i = 0; i < shelfAreaDetectors.Length; i++)
                    {
                        shelfAreaDetectors[i].gameObject.SetActive(false);
                    }

                    break;

                case ExperienMode.Evaluacion:
                    p_PocketFlowControl.EnableAllPocketFUnctions();
                    break;
            }
            c_CM.SetCheckList(-1);
        }

        private void Update()
        {
            if (settings.experienMode == ExperienMode.Entrenamiento)
            {
                while (i_index == 15)
                {
                    p_Pocket.canScan = true;

                    for (int i = 0; i < shelfAreaDetectors.Length; i++)
                    {
                        shelfAreaDetectors[i].gameObject.SetActive(true);
                    }
                    break;
                }
            }
        }

        #endregion Unity Functions

        #region Functions

        public IEnumerator LocationTutorials()
        {
            while (!b_End)
            {
                Debug.Log("Index" + i_index);
                yield return StartCoroutine(SetLocation(i_index));
                yield return StartCoroutine(Wait());
                c_canvasManager.g_PopUpSoundFX.SetActive(false);
                b_Continue = false;
                i_index++;
            }
        }

        private IEnumerator SetLocation(int index)
        {
            p_PocketFlowControl.DisablePocketFunctions();
            c_canvasManager.isFollow = false;
            c_canvasManager.isAnim = false;
            StartCoroutine(c_canvasManager.SetIcono(-1, 0));
            StopCoroutine(c_canvasManager.SetAnimIco(0));

            switch (index)
            {
                case 0:
                    o_ObjectsLogic.SetObjects(-1, -1);
                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 0));
                    aAudio.SetAudio(0, 0);
                    yield return new WaitForSeconds(4);
                    b_Continue = true;
                    break;

                case 1:
                    c_canvasManager.isFollow = true;

                    c_CM.SetCheckList(0);

                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 1));
                    StartCoroutine(c_canvasManager.SetIcono(4, 0));
                    StartCoroutine(c_canvasManager.FollowTransform(0));

                    break;

                case 2:
                    c_canvasManager.isFollow = true;

                    for (int i = 0; i < locationManager.shelfAreaDetectors.Length; i++)
                    {
                        if (locationManager.receptionInvoices.Count > 0)
                        {
                            locationManager.shelfAreaDetectors[i].shelfInvoice.GetComponent<BoxCollider>().enabled = false;
                        }
                    }

                    c_CM.SetCheckList(1);

                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 2));
                    StartCoroutine(c_canvasManager.SetIcono(8, 0));
                    StartCoroutine(c_canvasManager.FollowTransform(0));

                    p_PocketFlowControl.ActivePocketFunctions(0, 0, 2);

                    break;

                case 3:
                    c_canvasManager.isFollow = true;

                    c_CM.SetCheckList(2);

                    StartCoroutine(c_canvasManager.SetIcono(8, 0));

                    StartCoroutine(c_canvasManager.FollowTransform(0));

                    p_PocketFlowControl.ActivePocketFunctions(1, 0, 0);
                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 2));
                    break;

                case 4:
                    c_canvasManager.isFollow = true;

                    c_CM.SetCheckList(3);

                    p_PocketFlowControl.ActivePocketFunctions(2, 1, 1);

                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 3));
                    StartCoroutine(c_canvasManager.SetIcono(8, 0));
                    StartCoroutine(c_canvasManager.FollowTransform(0));
                    break;

                case 5:
                    c_canvasManager.isFollow = true;

                    c_CM.SetCheckList(4);
                    p_Pocket.canScan = true;
                    m_DinamicProducs.ManageProducts(0);

                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 4));
                    StartCoroutine(c_canvasManager.SetIcono(9, 0));
                    StartCoroutine(c_canvasManager.FollowTransform(0));
                    p_PocketFlowControl.ActivePocketFunctions(1, 0, 0);
                    // Primer producto
                    StartCoroutine(c_canvasManager.FollowTransform(1));
                    break;

                case 6:
                    c_canvasManager.isFollow = true;

                    c_CM.SetCheckList(5);
                    p_PocketFlowControl.ActivePocketFunctions(8, 0, 0);

                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 5));
                    StartCoroutine(c_canvasManager.SetIcono(8, 0));
                    StartCoroutine(c_canvasManager.FollowTransform(0));
                    break;

                case 7:
                    c_canvasManager.isFollow = true;

                    c_CM.SetCheckList(6);
                    p_PocketFlowControl.ActivePocketFunctions(8, 3, 3);

                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 6));
                    StartCoroutine(c_canvasManager.SetIcono(8, 0));
                    StartCoroutine(c_canvasManager.FollowTransform(0));
                    break;

                case 8:
                    c_canvasManager.isFollow = true;

                    c_CM.SetCheckList(7);

                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 7));
                    yield return new WaitForSeconds(5f);
                    b_Continue = true;
                    break;

                case 9:
                    c_CM.SetCheckList(7);
                    p_Pocket.canScan = true;

                    //StartCoroutine(c_canvasManager.SetIcono(9, 0));

                    for (int i = 0; i < locationManager.shelfAreaDetectors.Length; i++)
                    {
                        if (locationManager.receptionInvoices.Count > 0)
                        {
                            if (locationManager.shelfAreaDetectors[i].shelfInvoice.Data.shelfId == locationManager.receptionInvoices[0].Product.shelfId)
                            {
                                //StartCoroutine(c_canvasManager.Follow_Transform(locationManager.shelfAreaDetectors[i].shelfInvoice.transform.GetChild(1)));
                                locationManager.shelfAreaDetectors[i].shelfInvoice.GetComponent<BoxCollider>().enabled = true;
                            }
                            else
                            {
                                locationManager.shelfAreaDetectors[i].shelfInvoice.GetComponent<BoxCollider>().enabled = false;
                            }
                        }
                    }

                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 8));
                    break;

                case 10:
                    c_CM.SetCheckList(8);
                    p_Pocket.canScan = true;
                    c_canvasManager.isFollow = true;

                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 9));
                    StartCoroutine(c_canvasManager.SetIcono(9, 0));
                    StartCoroutine(c_canvasManager.FollowTransform(1));
                    break;

                case 11:
                    c_canvasManager.isFollow = true;

                    c_CM.SetCheckList(9);
                    p_PocketFlowControl.ActivePocketFunctions(9, 3, 3);

                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 10));
                    StartCoroutine(c_canvasManager.SetIcono(8, 0));
                    StartCoroutine(c_canvasManager.FollowTransform(0));
                    break;

                case 12:
                    c_canvasManager.isFollow = true;

                    c_CM.SetCheckList(10);
                    p_PocketFlowControl.ActivePocketFunctions(9, 0, 0);

                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 11));
                    StartCoroutine(c_canvasManager.SetIcono(8, 0));
                    StartCoroutine(c_canvasManager.FollowTransform(0));
                    break;

                case 13:
                    c_canvasManager.isFollow = true;

                    m_DinamicProducs.DisabeProducts();

                    StartCoroutine(c_canvasManager.SetIcono(18, 0));
                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 12));

                    for (int i = 0; i < shelfAreaDetectors.Length; i++)
                    {
                        shelfAreaDetectors[i].gameObject.SetActive(true);
                    }

                    for (int i = 0; i < locationManager.shelfAreaDetectors.Length; i++)
                    {
                        if (locationManager.receptionInvoices.Count > 0)
                        {
                            locationManager.shelfAreaDetectors[i].shelfInvoice.GetComponent<BoxCollider>().enabled = true;
                        }
                        for (int j = 0; j < locationManager.receptionInvoices.Count; j++)
                        {
                            if (locationManager.receptionInvoices[j].InvoiceScanned)
                                if (locationManager.shelfAreaDetectors[i].shelfInvoice.Data.shelfId == locationManager.receptionInvoices[j].Product.shelfId)
                                {
                                    StartCoroutine(c_canvasManager.Follow_Transform(locationManager.shelfAreaDetectors[i].shelfInvoice.transform.GetChild(1)));
                                }
                        }
                    }

                    yield return new WaitForSeconds(5f);
                    b_Continue = true;

                    break;

                case 14:
                    c_canvasManager.isFollow = true;

                    c_CM.SetCheckList(11);
                    m_DinamicProducs.ManageProducts(0);
                    aAudio.SetAudio(0, 1);
                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 13));
                    StartCoroutine(c_canvasManager.SetIcono(4, 0));
                    StartCoroutine(c_canvasManager.FollowTransform(1));
                    break;

                case 15:

                    c_CM.SetCheckList(12);
                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 14));
                    //Evento se puede disparar desde aca!!!
                    m_DinamicProducs.EnableAll();
                    p_PocketFlowControl.EnableAllPocketFUnctions();
                    p_Pocket.canScan = true;

                    for (int i = 0; i < locationManager.shelfAreaDetectors.Length; i++)
                    {
                        locationManager.shelfAreaDetectors[i].shelfInvoice.GetComponent<BoxCollider>().enabled = true;
                    }
                    break;

                case 16:
                    aAudio.SetAudio(0, 2);
                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 15));
                    yield return new WaitForSeconds(5f);
                    b_Continue = true;
                    break;

                case 17:
                    aAudio.SetAudio(0, 3);
                    yield return StartCoroutine(c_canvasManager.SetPopUp(0, 16));
                    yield return new WaitForSeconds(5f);
                    ScenesManager.instance.StartLoadScene(ScenesManager.instance.pickingScene.sceneName);
                    break;
            }
        }

        private IEnumerator Wait()
        {
            while (!b_Continue)
            {
                yield return null;
            }
        }

        public void UnlockTutorial()
        {
            b_Continue = true;
        }

        private void SetColliders(int i_Index)
        {
            for (int i = 0; i < b_Colliders.Length; i++)
            {
                if (i > -1)
                {
                    if (i == i_Index ? b_Colliders[i].enabled = true : b_Colliders[i].enabled = false) ;
                }
            }
        }

        #endregion Functions
    }
}