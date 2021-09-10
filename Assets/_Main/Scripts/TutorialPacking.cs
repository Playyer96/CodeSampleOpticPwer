using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica
{
    public class TutorialPacking : Tutorials
    {
        #region Components

        public CanvasManager c_CanvasManager;
        public AudioManager a_Audio;
        public ObjectsLogic o_ObjectsLogic;
        public bool b_Continue;
        public bool b_End;
        public SpectraUISettings s_Settings;
        public PocketFlowControl p_PocketFlowControl;
        public ManageDinamicProducts m_DinamicProducs;
        public TR_Container container;
        public PocketManager p_Pocket;
        public BoxCollider[] b_Colliders;
        public int i_index = 0;
        public CheckListManager c_CM;

        public GameObject boxdisassembleHandles;
        public GameObject boxHandles;
        public GameObject grapadora;
        public GameObject cinta;

        private WaitForEndOfFrame waitForEndOfFrame;
        private WaitForSeconds fourSeconds;

        private PackingIcons packIcon;

        private bool tutorialDone = false;

        #endregion Components

        #region Unity Functions

        private void Start()
        {
            c_CanvasManager = GetComponent<CanvasManager>();
            a_Audio = GetComponent<AudioManager>();
            o_ObjectsLogic = GetComponentInChildren<ObjectsLogic>();
            p_PocketFlowControl = GetComponent<PocketFlowControl>();
            p_Pocket = FindObjectOfType<PocketManager>();
            container = GetComponent<TR_Container>();
            packIcon = FindObjectOfType<PackingIcons>();

            waitForEndOfFrame = new WaitForEndOfFrame();
            fourSeconds = new WaitForSeconds(4f);

            switch (s_Settings.experienMode)
            {
                case ExperienMode.Entrenamiento:
                    SetColliders(-1);
                    StartCoroutine(LocationTutorials());
                    boxdisassembleHandles.SetActive(false);
                    grapadora.SetActive(false);
                    cinta.SetActive(false);
                    boxHandles.SetActive(false);
                    break;

                case ExperienMode.Evaluacion:
                    p_PocketFlowControl.EnableAllPocketFUnctions();
                    c_CM.SetCheckList(-1);
                    break;
            }
        }

        private void Update()
        {
            if (s_Settings.experienMode == ExperienMode.Entrenamiento && tutorialDone && !ScenesManager.instance.isLoadingScene)
            {
                ScenesManager.instance.StartLoadScene(ScenesManager.instance.menuScene);
            }
        }

        #endregion Unity Functions

        #region functions

        public IEnumerator LocationTutorials()
        {
            while (!b_End)
            {
                Debug.Log("<color=green>Index" + i_index + "</color>");
                yield return StartCoroutine(SetPacking(i_index));
                yield return StartCoroutine(Wait());
                c_CanvasManager.g_PopUpSoundFX.SetActive(false);
                b_Continue = false;
                i_index++;
            }
        }

        public IEnumerator SetPacking(int index)
        {
            p_PocketFlowControl.DisablePocketFunctions();
            c_CanvasManager.isFollow = false;
            c_CanvasManager.isAnim = false;
            StartCoroutine(c_CanvasManager.SetIcono(-1, 0));
            StopCoroutine(c_CanvasManager.SetAnimIco(0));
            switch (index)
            {
                case 0:
                    o_ObjectsLogic.SetObjects(-1, -1);

                    c_CM.SetCheckList(25);

                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 0, 0));
                    a_Audio.SetAudio(0, 0);
                    yield return fourSeconds;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 29, 0));
                    a_Audio.SetAudio(2, 1);
                    yield return fourSeconds;
                    //b_Continue = true;
                    break;

                case 1:
                    c_CanvasManager.isFollow = true;

                    c_CM.SetCheckList(0);

                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 30, 0));
                    //StartCoroutine(c_CanvasManager.SetIcono(3, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(0));

                    p_PocketFlowControl.ActivePocketFunctions(0, 0, 2);
                    break;

                case 2:
                    c_CanvasManager.isFollow = true;

                    c_CM.SetCheckList(1);

                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 1, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(3, 0));
                    p_PocketFlowControl.ActivePocketFunctions(1, 2, 2);
                    break;

                case 3:
                    c_CanvasManager.isFollow = true;

                    c_CM.SetCheckList(2);

                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 2, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(3, 0));
                    p_PocketFlowControl.ActivePocketFunctions(2, 1, 1);
                    break;

                case 4:
                    c_CanvasManager.isFollow = true;

                    c_CM.SetCheckList(3);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 31, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(3, 0));
                    p_PocketFlowControl.ActivePocketFunctions(3, 2, 2);
                    yield return waitForEndOfFrame;
                    break;

                case 5:
                    c_CanvasManager.isFollow = true;

                    c_CM.SetCheckList(4);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 3, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(3, 0));
                    p_PocketFlowControl.ActivePocketFunctions(3, 1, 1);
                    StartCoroutine(c_CanvasManager.FollowTransform(0));
                    break;

                case 6:
                    c_CanvasManager.isFollow = true;

                    c_CM.SetCheckList(5);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 26, 0));
                    p_PocketFlowControl.ActivePocketFunctions(4, 3, 3);
                    StartCoroutine(c_CanvasManager.SetIcono(4, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(2));
                    break;

                case 7:
                    c_CanvasManager.isFollow = true;

                    c_CM.SetCheckList(6);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 32, 0));
                    p_PocketFlowControl.ActivePocketFunctions(6, 0, 0);
                    StartCoroutine(c_CanvasManager.SetIcono(4, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(2));
                    break;

                case 8:
                    c_CanvasManager.isFollow = true;

                    c_CM.SetCheckList(7);

                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 4, 0));
                    p_PocketFlowControl.ActivePocketFunctions(4, 2, 2);
                    StartCoroutine(c_CanvasManager.SetIcono(4, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(2));
                    break;

                case 9:
                    c_CanvasManager.isFollow = true;

                    c_CM.SetCheckList(8);

                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 27, 0));
                    p_PocketFlowControl.ActivePocketFunctions(5, 0, 0);
                    StartCoroutine(c_CanvasManager.SetIcono(4, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(0));
                    break;

                case 10:
                    //c_CanvasManager.isFollow = true;

                    c_CM.SetCheckList(9);
                    p_PocketFlowControl.ActivePocketFunctions(5, 2, 2);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 5, 0));
                    packIcon.OnSetIcoGrab();

                    StartCoroutine(c_CanvasManager.SetIcono(-4, 0));
                    //StartCoroutine(c_CanvasManager.FollowTransform(1));
                    break;

                case 11:
                    //f4 continuar
                    //c_CanvasManager.isFollow = true;

                    c_CM.SetCheckList(10);

                    p_PocketFlowControl.ActivePocketFunctions(4, 1, 1);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 33, 0));
                    //StartCoroutine(c_CanvasManager.SetIcono(4, 0));
                    //StartCoroutine(c_CanvasManager.FollowTransform(1));
                    break;

                case 12:
                    //Escanear Articulo
                    c_CanvasManager.isFollow = true;

                    c_CM.SetCheckList(11);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 6, 0));
                    p_Pocket.canScan = true;
                    c_CanvasManager.StartSetIco(9, 1);
                    StartCoroutine(c_CanvasManager.FollowTransform(1));
                    print("Escanear");
                    break;

                case 13:
                    c_CanvasManager.isFollow = true;

                    c_CM.SetCheckList(12);

                    p_PocketFlowControl.ActivePocketFunctions(7, 3, 3);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 7, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(3, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(0));
                    break;

                case 14:
                    c_CanvasManager.isFollow = true;

                    c_CM.SetCheckList(13);

                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 8, 0));

                    StartCoroutine(c_CanvasManager.SetIcono(4, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(2));
                    break;

                case 15:
                    //c_CanvasManager.isFollow = true;

                    c_CM.SetCheckList(14);

                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 9, 0));
                    boxdisassembleHandles.SetActive(true);

                    StartCoroutine(c_CanvasManager.SetIcono(-20, 0));
                    //StartCoroutine(c_CanvasManager.FollowTransform(2));
                    break;

                case 16:
                    c_CanvasManager.isFollow = true;
                    c_CM.SetCheckList(15);

                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 10, 0));
                    grapadora.SetActive(true);

                    StartCoroutine(c_CanvasManager.SetIcono(21, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(3));
                    break;

                case 17:
                    //Toma el articulo y empacalo
                    //c_CanvasManager.isFollow = true;
                    c_CM.SetCheckList(16);

                    //a_Audio.SetAudio(0, 4);

                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 11, 0));
                    //StartCoroutine(c_CanvasManager.SetIcono(22, 0));
                    //StartCoroutine(c_CanvasManager.FollowTransform(2));
                    //c_CanvasManager.isFollow = true;
                    //StartCoroutine(c_CanvasManager.SetIcono(4, 0));
                    //StartCoroutine(c_CanvasManager.FollowTransform(1));
                    p_PocketFlowControl.EnableAllPocketFUnctions();
                    break;

                case 18:
                    //c_CanvasManager.isFollow = true;
                    c_CM.SetCheckList(17);

                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 12, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(-4, 0));
                    //StartCoroutine(c_CanvasManager.FollowTransform(2));
                    boxHandles.SetActive(true);

                    break;

                case 19:
                    c_CanvasManager.isFollow = true;
                    c_CM.SetCheckList(18);

                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 13, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(23, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(4));
                    cinta.SetActive(true);

                    break;

                case 20:
                    c_CanvasManager.isFollow = true;
                    c_CM.SetCheckList(19);

                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 14, 0));
                    p_PocketFlowControl.ActivePocketFunctions(7, 2, 2);
                    StartCoroutine(c_CanvasManager.SetIcono(3, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(0));
                    p_PocketFlowControl.ActivePocketFunctions(7, 2, 2);

                    break;

                case 21:
                    c_CanvasManager.isFollow = true;
                    c_CM.SetCheckList(20);
                    //a_Audio.SetAudio(0, 3);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 15, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(10, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(5));

                    break;

                case 22:
                    a_Audio.SetAudio(0, 1);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 16, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(-1, 0));
                    yield return fourSeconds;
                    b_Continue = true;
                    break;

                case 23:
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 17, 0));
                    yield return fourSeconds;
                    b_Continue = true;
                    break;

                case 24:
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 18, 0));
                    yield return fourSeconds;
                    ScenesManager.instance.StartLoadScene(ScenesManager.instance.menuScene);
                    //b_Continue = true;
                    break;

                /*case 25:
                    a_Audio.SetAudio(1);
                    yield return fourSeconds;
                    b_Continue = true;
                    break;

                case 26:
                    a_Audio.SetAudio(1);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 19, 0));
                    yield return fourSeconds;
                    tutorialDone = true;
                    ScenesManager.instance.StartLoadScene(ScenesManager.instance.menuScene);
                    break;*/
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

        #endregion functions
    }
}