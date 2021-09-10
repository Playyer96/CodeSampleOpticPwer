using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using Hi5_Interaction_Core;
using UnityEngine;

namespace DreamHouseStudios.SofasaLogistica
{
    public class TutorialReception : Tutorials
    {
        [SerializeField] private ReceptionStepToStep receptionStepToStep;
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
        public SphereCollider[] b_Colliders;
        public int i_index = 0;
        public CheckListManager c_CM;
        public ShelfPacakage s_ShelfPackage;
        public GameObject g_ShelfHighLight;
        public GameObject g_TableGhost;
        public BoxManager b_Box;
        public GameObject g_Residuo;
        public List<Bag_Shelf> b_Products;
        private bool b_ProductInShelf = false;
        public AccionTutorial a_AT;
        public ReferenceState r_DigitalCamera;

        public virtual void Start()
        {
            b_Products = new List<Bag_Shelf>();
            c_CanvasManager = GetComponent<CanvasManager>();
            a_Audio = GetComponent<AudioManager>();
            o_ObjectsLogic = GetComponentInChildren<ObjectsLogic>();
            p_PocketFlowControl = GetComponent<PocketFlowControl>();
            m_DinamicProducs = GetComponent<ManageDinamicProducts>();
            p_Pocket = FindObjectOfType<PocketManager>();
            container = GetComponent<TR_Container>();
            c_CM = GetComponent<CheckListManager>();
            if (s_Settings.experienMode == ExperienMode.Entrenamiento)
            {
                SetColliders(-1);
                StartCoroutine(ReceptionTutorial());
            }

            c_CM.SetCheckList(-1);
        }

        public virtual IEnumerator ReceptionTutorial()
        {
            while (!b_End)
            {
                yield return StartCoroutine(SetReception(i_index));
                yield return StartCoroutine(Wait());
                c_CanvasManager.g_PopUpSoundFX.SetActive(false);
                b_Continue = false;
                i_index++;
            }
        }

        public virtual IEnumerator SetReception(int index)
        {
            p_PocketFlowControl.DisablePocketFunctions();
            c_CanvasManager.isFollow = false;
            c_CanvasManager.isAnim = false;
            StartCoroutine(c_CanvasManager.SetIcono(-1, 0));
            StopCoroutine(c_CanvasManager.SetAnimIco(0));
            Debug.Log("<color=green>Index" + index + "</color>");
            switch (index)
            {
                case 0:
                    o_ObjectsLogic.SetObjects(-1, -1);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 0, 0));
                    a_Audio.SetAudio(0, 0);
                    yield return new WaitForSeconds(4);
                    b_Continue = true;
                    break;

                case 1:
                    a_AT.StopAnim();
                    c_CM.SetCheckList(0);
                    o_ObjectsLogic.SetObjects(0, 6);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 1, 0));
                    StartCoroutine(a_AT.ActiveScreen(2, 0));
                    g_TableGhost.SetActive(true);
                    a_Audio.SetAudio(0, 1);
                    break;

                case 2:
                case 18:
                    //toma la camara
                    if (index == 2)
                    {
                        c_CM.SetCheckList(1);
                    }

                    if (index == 18)
                    {
                        a_AT.StopAnim();
                        c_CM.SetCheckList(16);
                    }

                    o_ObjectsLogic.SetObjects(6, 6);
                    c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 2, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(4, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(0));
                    if (index == 2)
                    {
                        StartCoroutine(a_AT.ActiveScreen(0, 0));
                    }
                    a_Audio.SetAudio(0, 5);
                    break;

                case 3:
                    a_AT.StopAnim();
                    c_CM.SetCheckList(2);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 3, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(5, 1));
                    StartCoroutine(a_AT.ActiveScreen(1, 4));
                    break;

                case 4:
                case 6:
                //case 16:
                case 20:
                case 24:
                    if (index == 4)
                    {
                        c_CM.SetCheckList(3);
                        a_AT.StopAnim();
                    }

                    if (index == 6)
                    {
                        c_CM.SetCheckList(5);
                        i_index = 29;
                    }

                    if (index == 16)
                    {
                        c_CM.SetCheckList(14);
                    }

                    if (index == 20)
                    {
                        c_CM.SetCheckList(18);
                        r_DigitalCamera.b_CanCall = true;
                    }

                    if (index == 24)
                    {
                        c_CM.SetCheckList(22);
                    }

                    c_CanvasManager.g_PopUpSoundFX.SetActive(false);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, -1, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(-1, 0));
                    break;

                case 5:
                    //Pasa el visturi sobre las cintas
                    c_CM.SetCheckList(4);
                    o_ObjectsLogic.SetObjects(7, 7);
                    //c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 4, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(6, 10));
                    //StartCoroutine(c_CanvasManager.FollowTransform(1));
                    break;

                case 30:
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(2, 13, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(-1, 0));
                    a_Audio.SetAudio(0, 4);
                    yield return new WaitForSeconds(5f);
                    b_Continue = true;
                    break;

                case 31:
                case 33:
                    c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(2, 16, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(4, 0));
                    if (index == 31)
                    {
                        StartCoroutine(c_CanvasManager.FollowTransform(5));
                        o_ObjectsLogic.SetObjects(16, 16);
                    }
                    else
                    {
                        StartCoroutine(c_CanvasManager.FollowTransform(6));
                        o_ObjectsLogic.SetObjects(17, 17);
                    }

                    SetColliders(0);
                    break;

                case 32:
                case 34:
                    c_CanvasManager.isFollow = false;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(2, 17, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(13, 6));
                    StartCoroutine(c_CanvasManager.FollowTransform(5));
                    SetColliders(0);
                    if (index == 34)
                    {
                        i_index = 6;
                    }

                    break;

                case 7:
                    //logueate en el pocket
                    c_CM.SetCheckList(6);
                    o_ObjectsLogic.SetObjects(8, 8);
                    c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(1, 0, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(16, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(2));
                    a_Audio.SetAudio(0, 2);
                    p_PocketFlowControl.ActivePocketFunctions(0, 0, 2);
                    break;

                case 8:
                    c_CM.SetCheckList(7);
                    c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(1, 1, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(8, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(2));
                    p_PocketFlowControl.ActivePocketFunctions(1, 0, 0);
                    break;

                case 9:
                    c_CM.SetCheckList(8);
                    c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(1, 2, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(8, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(2));
                    p_PocketFlowControl.ActivePocketFunctions(2, 0, 0);
                    break;

                case 10:

                    yield return StartCoroutine(c_CanvasManager.SetPopUp(1, 3, 0));
                    yield return StartCoroutine(c_CanvasManager.SetIcono(-1, 0));
                    yield return new WaitForSeconds(5f);
                    b_Continue = true;
                    break;

                case 11:
                    c_CM.SetCheckList(9);
                    c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(1, 4, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(8, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(2));
                    p_PocketFlowControl.ActivePocketFunctions(3, 0, 0);
                    break;

                case 12:
                    c_CM.SetCheckList(10);
                    c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(1, 5, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(8, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(2));
                    p_PocketFlowControl.ActivePocketFunctions(4, 0, 0);
                    break;

                case 13:
                    c_CM.SetCheckList(11);
                    c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(1, 6, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(8, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(2));
                    p_PocketFlowControl.ActivePocketFunctions(6, 0, 0);
                    break;

                case 14:
                    c_CM.SetCheckList(12);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(1, 7, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(9, 0));
                    StartCoroutine(a_AT.ActiveScreen(4, 4));
                    p_PocketFlowControl.ActivePocketFunctions(6, 1, 1);
                    break;

                case 15:
                    a_AT.StopAnim();
                    c_CM.SetCheckList(13);
                    c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(1, 8, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(8, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(2));
                    p_PocketFlowControl.ActivePocketFunctions(6, 2, 2);
                    i_index = 16;

                    break;

                case 17:
                    //Abre la caja
                    o_ObjectsLogic.SetObjects(8, 14);
                    c_CM.SetCheckList(15);
                    if (b_Box.g_boxInPlace[0].activeSelf)
                    {
                        yield return StartCoroutine(c_CanvasManager.SetPopUp(2, 1, 0));
                        StartCoroutine(c_CanvasManager.SetIcono(15, 0));
                        StartCoroutine(a_AT.ActiveScreen(5, 4));
                    }
                    else
                    {
                        yield return StartCoroutine(c_CanvasManager.SetPopUp(2, 20, 0));
                        StartCoroutine(c_CanvasManager.SetIcono(24, 0));
                        StartCoroutine(a_AT.ActiveScreen(6, 4));
                    }
                    if (b_Box.g_boxInPlace[1].activeInHierarchy)
                    {
                        i_index = 36;
                    }

                    break;

                case 19:
                    c_CM.SetCheckList(17);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(2, 3, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(5, 1));
                    break;

                case 21:
                    c_CM.SetCheckList(19);
                    o_ObjectsLogic.SetObjects(-1, -1);
                    ReceptionManager rm = FindObjectOfType<ReceptionManager>();
                    container.playerItems[3] = rm.productInvoices[0].transform;
                    c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(2, 6, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(4, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(3));
                    m_DinamicProducs.ManageProducts(0);
                    break;

                case 22:
                    c_CM.SetCheckList(20);
                    c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(2, 7, 0));
                    o_ObjectsLogic.SetObjects(8, 8);
                    StartCoroutine(c_CanvasManager.SetIcono(9, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(3));
                    p_Pocket.canScan = true;
                    break;

                case 23:
                    c_CM.SetCheckList(21);
                    p_Pocket.canScan = false;
                    c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(1, 12, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(8, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(2));
                    p_PocketFlowControl.ActivePocketFunctions(7, 1, 1);
                    break;

                case 25:
                    c_CM.SetCheckList(23);
                    o_ObjectsLogic.SetObjects(-1, -1);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(1, 13, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(10, 7));
                    yield return new WaitForSeconds(5f);
                    b_Continue = true;
                    break;

                case 26:
                    //agarra la etiqueta Continuar aca
                    c_CM.SetCheckList(23);
                    c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(2, 10, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(4, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(4));
                    o_ObjectsLogic.SetObjects(15, 15);
                    break;

                case 27:
                    //pegar etiqueta
                    c_CM.SetCheckList(24);
                    c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(2, 11, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(17, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(3));
                    StartCoroutine(a_AT.ActiveScreen(7, 0));
                    break;

                case 28:
                    //Poner en estanteria
                    a_AT.StopAnim();
                    c_CM.SetCheckList(25);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(2, 12, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(18, 8));
                    a_Audio.SetAudio(0, 3);
                    if (!b_ProductInShelf)
                    {
                        while (b_Products.Count == 0)
                        {
                            yield return null;
                        }

                        Checklist.Set("Check_26", "Accion_1", true);
                        b_ProductInShelf = true;
                    }

                    break;

                case 29:
                    //repite el procedimiento
                    g_ShelfHighLight.SetActive(false);
                    c_CM.SetCheckList(26);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(1, 14, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(1, 3));
                    m_DinamicProducs.EnableAll();
                    o_ObjectsLogic.SetObjects(3, 8);
                    p_PocketFlowControl.ActivePocketFunctions(7, 1, 1);
                    i_index = 37;
                    break;

                case 35:
                    //Por favor espera un momento
                    g_ShelfHighLight.SetActive(false);
                    SetColliders(-1);
                    o_ObjectsLogic.SetObjects(-1, -1);
                    p_PocketFlowControl.DisablePocketFunctions();
                    m_DinamicProducs.DisabeProducts();
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(2, 18, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(-1, 6));
                    a_Audio.SetAudio(1, 3);
                    yield return new WaitForSeconds(5f);
                    ScenesManager.instance.StartLoadScene(ScenesManager.instance.locationScene.sceneName);
                    break;

                case 36:
                    //producto diferido
                    c_CM.SetCheckList(28);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(1, 15, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(-1, 0));
                    p_PocketFlowControl.ActivePocketFunctions(7, 1, 1);
                    if (b_Products.Count == b_Box.i_Products - 1)
                    {
                        i_index = 37;
                    }
                    else
                    {
                        i_index = 28;
                    }
                    g_ShelfHighLight.SetActive(true);
                    break;

                case 37:
                    //Pon el sujetador
                    a_AT.StopAnim();
                    c_CM.SetCheckList(29);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(2, 0, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(14, 9));
                    i_index = 17;
                    break;

                case 38:
                    //El residuo de las cintas es reciclable
                    c_CanvasManager.isFollow = true;
                    c_CM.SetCheckList(30);
                    g_Residuo.GetComponent<BoxCollider>().isTrigger = false;
                    g_Residuo.GetComponent<Rigidbody>().isKinematic = false;
                    g_ShelfHighLight.SetActive(false);
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(2, 19, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(4, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(7));
                    SetColliders(2);
                    o_ObjectsLogic.SetObjects(18, 18);
                    break;

                case 39://Deposita en la caneca verde
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(2, 14, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(13, 4));
                    i_index = 34;
                    break;
            }
        }

        public virtual IEnumerator Wait()
        {
            while (!b_Continue)
            {
                yield return null;
            }
        }

        public virtual void UnlockTutorial()
        {
            b_Continue = true;
        }

        public virtual void SetColliders(int i_Index)
        {
            for (int i = 0; i < b_Colliders.Length; i++)
            {
                if (i > -1)
                {
                    if (i == i_Index ? b_Colliders[i].enabled = true : b_Colliders[i].enabled = false) ;
                }
            }
        }

        public virtual void StopTutorial()
        {
            StopAllCoroutines();
        }
    }
}