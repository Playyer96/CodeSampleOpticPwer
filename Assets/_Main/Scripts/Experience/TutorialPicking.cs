using System.Collections;
using System.Collections.Generic;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPicking : TutorialReception
{
    public PocketPickingProducts p_PocketPickingP;
    public PocketFunctions p_buttonNext;
    public Text txt_ShelfID;

    public override void Start()
    {
        if (s_Settings.experienMode == ExperienMode.Entrenamiento)
        {
            StartCoroutine(ReceptionTutorial());
        }
        else
        {
            p_buttonNext.enabled = true;
        }
    }

    public override IEnumerator SetReception(int index)
    {
        c_CanvasManager.isAnim = false;
        c_CanvasManager.isFollow = false;
        p_PocketFlowControl.DisablePocketFunctions();
        /*StartCoroutine(c_CanvasManager.SetIcono(-1, 0));
        StopCoroutine(c_CanvasManager.SetAnimIco(0));*/
        c_CanvasManager.KillAllCoroutines();
        switch (index)
        {
            case 0:
                //Picking
                yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 0, 0));
                StartCoroutine(c_CanvasManager.SetIcono(-1, 0));
                a_Audio.SetAudio(1, 0);
                yield return new WaitForSeconds(4);
                b_Continue = true;
                break;

            case 1:
            case 2:
            case 3:
                c_CanvasManager.isFollow = true;
                if (index == 1)
                {
                    //Logueate en el pocket
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 1, 0));
                    p_PocketFlowControl.ActivePocketFunctions(0, 0, 2);
                    a_Audio.SetAudio(0, 2);
                }

                if (index == 2)
                {
                    //Menu salidas
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 2, 0));
                    p_PocketFlowControl.ActivePocketFunctions(1, 0, 0);
                }

                if (index == 3)
                {
                    //Picking Guiado
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 3, 0));
                    p_PocketFlowControl.ActivePocketFunctions(2, 0, 0);
                }

                StartCoroutine(c_CanvasManager.SetIcono(3, 0));
                StartCoroutine(c_CanvasManager.FollowTransform(0));
                break;

            case 4:
            case 5:
                if (index == 4)
                {
                    //Rrevisa los articulos a TOMAR
                    c_CanvasManager.isFollow = true;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 4, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(16, 0));
                    StartCoroutine(c_CanvasManager.FollowTransform(0));
                    p_PocketFlowControl.DisablePocketFunctions();
                    yield return new WaitForSeconds(4f);
                    //Escanea Estatnteria
                    c_CanvasManager.isFollow = false;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 5, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(-9, ReturnPos(true)));
                }

                if (index == 5)
                {
                    //Escanea Producto
                    c_CanvasManager.isFollow = false;
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 6, 0));
                    StartCoroutine(c_CanvasManager.SetIcono(9, ReturnPos(false)));
                }

                p_PocketFlowControl.ActivePocketFunctions(3, 0, 0);
                p_Pocket.canScan = true;
                break;

            case 6:
            case 7:
                c_CanvasManager.isFollow = true;
                if (index == 6)
                {
                    //Digita la Cantidad
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 7, 0));
                    p_PocketFlowControl.ActivePocketFunctions(3, 1, 1);
                }

                if (index == 7)
                {
                    //F1 Grabar
                    yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 8, 0));
                    p_PocketFlowControl.ActivePocketFunctions(3, 2, 2);
                }

                StartCoroutine(c_CanvasManager.SetIcono(3, 0));
                StartCoroutine(c_CanvasManager.FollowTransform(0));
                break;

            case 8:
                //Toma Articulos
                yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 9, 0));
                StartCoroutine(c_CanvasManager.SetIcono(4, ReturnPos(false)));
                p_PocketFlowControl.DisablePocketFunctions();
                GrabProduct.b_canCount = true;
                break;

            case 9:
                //El Orden de ubicacion
                GrabProduct.b_canCount = false;
                c_CanvasManager.isFollow = true;
                yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 10, 0));
                StartCoroutine(c_CanvasManager.SetIcono(18, 0));
                StartCoroutine(c_CanvasManager.FollowTransform(1));
                a_Audio.SetAudio(1, 2);
                break;

            case 10:
                //Flecha Abajo
                c_CanvasManager.isFollow = true;
                yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 11, 0));
                StartCoroutine(c_CanvasManager.SetIcono(3, 0));
                StartCoroutine(c_CanvasManager.FollowTransform(0));
                p_PocketFlowControl.ActivePocketFunctions(3, 3, 3);
                break;

            case 11:
                //Repite el procedimiento
                yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 12, 0));
                StartCoroutine(c_CanvasManager.SetIcono(-1, 0));
                p_PocketFlowControl.EnableAllPocketFUnctions();
                c_CanvasManager.b_CanRotateIco = true;
                break;

            case 12:
                p_PocketFlowControl.DisablePocketFunctions();
                StartCoroutine(c_CanvasManager.SetIcono(-1, 0));
                //Muy bien
                yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 13, 0));
                a_Audio.SetAudio(1, 1);
                StartCoroutine(c_CanvasManager.SetIcono(-1, 0));
                yield return new WaitForSeconds(4f);
                //Espera un momento
                yield return StartCoroutine(c_CanvasManager.SetPopUp(0, 14, 0));
                a_Audio.SetAudio(2, 3);
                yield return new WaitForSeconds(5f);
                ScenesManager.instance.StartLoadScene(ScenesManager.instance.packingScene.sceneName);
                break;
        }
    }

    public void SetGrabProduct()
    {
        if (settings.experienMode == ExperienMode.Entrenamiento)
        {
            StartCoroutine(c_CanvasManager.SetIcono(4, ReturnPos(false)));
        }
    }

    public void SetScanProduct()
    {
        if (settings.experienMode == ExperienMode.Entrenamiento)
        {
            StartCoroutine(c_CanvasManager.SetIcono(9, ReturnPos(false)));
        }
    }

    public void HideIco()
    {
        StartCoroutine(c_CanvasManager.SetIcono(-1, 0));
    }

    private int int_ReturnIndex = 0;

    private int ReturnPos(bool isShelf)
    {
        string m_Shelf = txt_ShelfID.text;
        if (m_Shelf == "12153A5A")
        {
            return (isShelf ? 0 : 8);
        }
        else if (m_Shelf == "12153A5B")
        {
            return (isShelf ? 1 : 9);
        }
        else if (m_Shelf == "12153A5C")
        {
            return (isShelf ? 2 : 10);
        }
        else if (m_Shelf == "12153A4D")
        {
            return (isShelf ? 3 : 11);
        }
        else if (m_Shelf == "12153A4E")
        {
            return (isShelf ? 4 : 12);
        }
        else if (m_Shelf == "12153A5F")
        {
            return (isShelf ? 5 : 13);
        }
        else if (m_Shelf == "1213A5G")
        {
            return (isShelf ? 6 : 14);
        }
        else
        {
            return (isShelf ? 7 : 15);
        }
    }
}