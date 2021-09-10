using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DreamHouseStudios.SofasaLogistica;
using DreamHouseStudios.VR;
using FMODUnity;
using UnityEngine.Events;

public class PocketFunctions : MonoBehaviour
{
    public static int i_CanvasIndex;
    public bool actionCheck;
    public bool b_ActiveEvent;
    public bool b_ChangeLAyout = true;
    public bool b_NeedsProductInvoice = true;
    public bool b_next;
    private bool b_product = false;
    private bool b_shelf = false;
    public bool canPrint;
    public CleanerPocket cp;
    public UnityEvent e_OnClick;
    public UnityEvent e_OnEscanBox;
    public UnityEvent e_OnEscanProduct;
    public UnityEvent e_OnEscanShelf;
    public UnityEvent e_OnPrint;
    public UnityEvent e_OnScanDeferred;
    public UnityEvent e_OnScanReceptionInvoice;
    public PocketFunctions[] examples;
    public float[] f_CanvasPosX;
    public GameObject g_Object;
    private int i_Index = 0;
    private int indexLineasTexto;
    private LocationManager l_LM;
    public int layoutIndex;
    public PocketPickingProducts p_Picking;
    public PocketError pe;
    private ProductInvoice pi;

    public PrinterList pl;
    private PocketManager pm;
    private ReceptionInvoice ri;
    public string s_txtToDisplay;

    /*public static List<DataBox> db;
    [SerializeField] public List<DataBox> debugList;*/
    public StudioEventEmitter soundFx;
    public Transform t_canvasScroll;
    public TextosProductos[] tp;
    public TestPocketPacking tpp;
    public Text txt_ToUse;
    public TypeButton typeButton;
    public GenericIco ico;
    public PackingIcons PI;

    //
    private UbicationIcons u_Icon;

    private void Awake()
    {
        pm = FindObjectOfType<PocketManager>();
        i_CanvasIndex = 0;

        if (e_OnClick == null)
        {
            e_OnClick = new UnityEvent();
        }

        if (e_OnEscanProduct == null)
        {
            e_OnEscanProduct = new UnityEvent();
        }

        if (e_OnEscanBox == null)
        {
            e_OnEscanBox = new UnityEvent();
        }

        if (e_OnEscanShelf == null)
        {
            e_OnEscanShelf = new UnityEvent();
        }

        if (e_OnPrint == null)
        {
            e_OnPrint = new UnityEvent();
        }

        if (e_OnScanReceptionInvoice == null)
        {
            e_OnScanReceptionInvoice = new UnityEvent();
        }

        if (e_OnScanDeferred == null)
        {
            e_OnScanDeferred = new UnityEvent();
        }

        /*if (typeButton == TypeButton.UpdateListOfTexts)
        {
            db = new List<DataBox>();
        }
        debugList = new List<DataBox>();*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
        {
            return;
        }

        CanvasInteractor ci = other.GetComponent<CanvasInteractor>();
        if (ci.canInterac)
        {
            ci.canInterac = false;
            MaintInteraction();
        }
    }

    private void MaintInteraction()
    {
        PlaySound(soundFx);
        switch (typeButton)
        {
            case TypeButton.Simple:
                txt_ToUse.text = s_txtToDisplay;
                actionCheck = true;
                if (b_ActiveEvent)
                {
                    e_OnClick.Invoke();
                }

                break;

            case TypeButton.Check:
                if (Check())
                {
                    PlaySound(soundFx);
                    if (b_ChangeLAyout)
                    {
                        pm.SetLayout(layoutIndex);
                    }

                    e_OnClick.Invoke();
                }

                break;

            case TypeButton.Container:
                // if (pm.actualState == PocketState.Containers)
                // {
                pm.SetLayout(layoutIndex);
                // }
                break;

            case TypeButton.Box:
                // if (pm.actualState == PocketState.Box)
                // {
                pm.SetLayout(layoutIndex);
                // }
                break;

            case TypeButton.UnlockScan:
                pm.canScan = true;
                typeButton = TypeButton.Simple;
                MaintInteraction();
                break;

            case TypeButton.UpdatePocketStatus:
                PlaySound(soundFx);
                pm.actualState = PocketState.Box;
                typeButton = TypeButton.Check;
                MaintInteraction();
                break;

            case TypeButton.UpdateListOfTexts:
                DataBox mdb = new DataBox();
                mdb.pi = pi;
                mdb.b_hasPrinted = false;
                for (int i = 0; i < PrinterList.db.Count; i++)
                {
                    if (PrinterList.db[i].pi == mdb.pi)
                    {
                        return;
                    }
                }

                PrinterList.db.Add(mdb);
                tp[indexLineasTexto].txt_Linea[0].text = "1"; //Cantidad
                tp[indexLineasTexto].txt_Linea[1].text = "0"; //Averia
                tp[indexLineasTexto].txt_Linea[2].text = pi.Product.productId; //Material
                tp[indexLineasTexto].txt_Linea[3].text = pi.Product.description; //Descripcion
                tp[indexLineasTexto].txt_Linea[4].text = "10"; //Max
                tp[indexLineasTexto].txt_Linea[5].text = "PLI"; //T.D
                tp[indexLineasTexto].txt_Linea[6].text = "0"; //None

                //StartCoroutine(PrintList());
                break;

            case TypeButton.F1Grab:
                PlaySound(soundFx);
                if (Check())
                {
                    if (tp.Length > 0 && examples.Length > 0)
                    {
                        if (tp[0].txt_Linea[0].text == tp[0].txt_Linea[1].text &&
                            tp[0].txt_Linea[2].text == tp[0].txt_Linea[3].text && examples[0].actionCheck)
                        {
                            if (b_ChangeLAyout)
                            {
                                pm.SetLayout(layoutIndex);
                            }
                        }
                        else
                        {
                            Debug.Log("<color=red>Debe salir pantalla de error</color>");
                        }
                    }
                    actionCheck = true;
                }
                else
                {
                    return;
                }
                e_OnClick.Invoke();
                break;

            case TypeButton.MoveCanvas:
                if (b_next)
                {
                    if (i_CanvasIndex < f_CanvasPosX.Length - 1)
                    {
                        i_CanvasIndex++;
                    }
                }
                else
                {
                    if (i_CanvasIndex > 0)
                    {
                        i_CanvasIndex--;
                    }
                }

                t_canvasScroll.transform.localPosition = new Vector3(f_CanvasPosX[i_CanvasIndex], t_canvasScroll.localPosition.y, 0);
                break;

            case TypeButton.Print:
                if (canPrint)
                {
                    canPrint = false;
                    if (b_NeedsProductInvoice)
                    {
                        //StartCoroutine(pl.PrintList(PrinterList.db));
                        pl.StartPrintInvoices();
                    }
                    else
                    {
                        Printer.instance.PrintInvoice();
                        e_OnPrint.Invoke();
                    }
                }

                break;

            case TypeButton.UbicationScreen:
                if (l_LM == null)
                {
                    l_LM = FindObjectOfType<LocationManager>();
                }

                if (u_Icon == null)
                {
                    u_Icon = FindObjectOfType<UbicationIcons>();
                }

                u_Icon.GetReceptionInvoice(ri);
                if (tp.Length > 0)
                {
                    tp[0].txt_Linea[0].text = ri.Product.productId;
                    tp[0].txt_Linea[1].text = ri.Product.productId;
                    tp[0].txt_Linea[2].text = ri.Product.description;
                    for (int i = 0; i < l_LM.shelves.Length; i++)
                    {
                        if (ri.Product.shelfId == l_LM.shelves[i].shelfId)
                        {
                            tp[0].txt_Linea[3].text = l_LM.shelves[i].totalProductsOnShelf.ToString();
                            tp[0].txt_Linea[4].text = l_LM.shelves[i].shelfId;
                            tp[0].txt_Linea[5].text = ri.Product.productId;
                            tp[0].txt_Linea[6].text = l_LM.shelves[i].totalProductsOnShelf.ToString();
                            tp[0].txt_Linea[7].text = ri.Product.description;
                            ico.SetPos(l_LM.shelves[i].shelfId);
                            break;
                        }
                    }
                }

                break;

            case TypeButton.DinamicAmount:
                if (examples[0].b_shelf && examples[0].b_product)
                {
                    s_txtToDisplay = tp[0].txt_Linea[0].text;
                    txt_ToUse.text = s_txtToDisplay;
                    actionCheck = true;
                    e_OnClick.Invoke();
                }

                break;

            case TypeButton.NextItemPicking:

                if (!examples[2].actionCheck)
                {
                    return;
                }

                if (!p_Picking.NextItem())
                {
                    gameObject.SetActive(false);
                }

                if (pm.s_Settings.experienMode == ExperienMode.Entrenamiento)
                {
                    GetComponent<PocketFunctions>().enabled = false;
                    GetComponent<BoxCollider>().enabled = false;
                }
                e_OnClick.Invoke();
                break;

            case TypeButton.Toggle:
                e_OnClick.Invoke();
                examples[0].actionCheck = true;
                examples[1].MaintInteraction();
                examples[2].transform.GetChild(0).gameObject.SetActive(false);
                g_Object.SetActive(true);
                cp.b_Clean = false;
                break;

            case TypeButton.PackingScreen:
                if (tpp == null)
                {
                    tpp = FindObjectOfType<TestPocketPacking>();
                }
                if (PI == null)
                {
                    PI = FindObjectOfType<PackingIcons>();
                }

                PI.SetProductInvoices(pi);
                e_OnEscanProduct.Invoke();

                for (int i = 0; i < tpp.l_Data.Count; i++)
                {
                    if (pi.Product.productId == tpp.l_Data[i].s_ID)
                    {
                        for (int j = 0; j < tp.Length; j++)
                        {
                            if (tp[j].txt_Linea[1].text == pi.Product.productId)
                            {
                                return;
                            }
                        }
                        tp[i_Index].txt_Linea[0].GetComponentInParent<PocketFunctions>().s_txtToDisplay = tpp.l_Data[i].i_Amount.ToString();
                        tp[i_Index].txt_Linea[1].text = tpp.l_Data[i].s_ID;
                        tp[i_Index].txt_Linea[2].text = tpp.l_Data[i].s_Description;
                        i_Index++;
                        break;
                    }
                }

                /*if (i_Index == tpp.l_Data.Count - 1)
                {
                    examples[0].actionCheck = true;
                }*/

                break;
        }
    }

    private bool Check()
    {
        bool checker = true;
        for (int i = 0; i < examples.Length; i++)
        {
            if (!examples[i].actionCheck)
            {
                return false;
            }
        }

        return checker;
    }

    public void GetData(Transform t)
    {
        if (t.GetComponent<BoxInvoice>() != null)
        {
            s_txtToDisplay = t.GetComponent<BoxInvoice>().Data.boxId.ToString();
            t.GetComponent<BoxInvoice>().SetBool(true);
            MaintInteraction();
            pm.canScan = false;
            e_OnEscanBox.Invoke();
        }
        else if (t.GetComponent<ProductInvoice>() != null)
        {
            pi = t.GetComponent<ProductInvoice>();
            if (typeButton != TypeButton.PackingScreen)
            {
                if (typeButton != TypeButton.InfoLocation)
                {
                    pi.SetBool(true);
                    s_txtToDisplay = pi.Product.productId;
                    for (int i = 0; i < tp.Length; i++)
                    {
                        if (tp[i].txt_Linea[2].text == pi.Product.productId)
                        {
                            return;
                        }
                    }

                    MaintInteraction();
                    indexLineasTexto++;
                    examples[0].pi = pi;
                    examples[0].canPrint = true;
                    pi.SetBool(true);
                    if (t.GetComponentInParent<Bag_Shelf>() != null && t.GetComponentInParent<Bag_Shelf>().b_IsDeferred)
                    {
                        t.GetComponentInParent<Bag_Shelf>().SetInfo();
                    }

                    e_OnEscanProduct.Invoke();
                }
                else
                {
                    if (b_shelf)
                    {
                        if (tp.Length > 1)
                        {
                            if (tp[1].txt_Linea[0].text == pi.Product.productId)
                            {
                                tp[0].txt_Linea[0].text = pi.Product.productId;
                                pi.SetBool(true);
                                b_product = true;
                                e_OnEscanProduct.Invoke();
                            }
                            else
                            {
                                pe.s_c1 = pi.Product.productId;
                                pe.s_c2 = tp[1].txt_Linea[1].text;
                                pm.SetLayout(11); //pantalla de error
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                MaintInteraction();
            }
        }
        else if (t.GetComponent<ShelfInvoice>() != null)
        {
            ShelfInvoice si = t.GetComponent<ShelfInvoice>();
            if (tp[1].txt_Linea[1].text == si.Data.shelfId)
            {
                tp[0].txt_Linea[1].text = si.Data.shelfId;
                t.GetComponent<ShelfInvoice>().SetBool(true);
                e_OnEscanShelf.Invoke();
                b_shelf = true;
            }
            else
            {
                pe.s_c1 = si.Data.shelfId;
                pe.s_c2 = tp[1].txt_Linea[1].text;
                pm.SetLayout(11);
            }
        }
        else if (t.GetComponent<ReceptionInvoice>() != null)
        {
            if (typeButton == TypeButton.UpdateListOfTexts)
            {
                return;
            }

            if (typeButton != TypeButton.InfoLocation)
            {
                ri = t.GetComponent<ReceptionInvoice>();
                actionCheck = true;
                MaintInteraction();
                pm.canScan = false;
                e_OnScanReceptionInvoice.Invoke();
                t.GetComponent<ReceptionInvoice>().SetInvoiceScanned(true);
                Invoke("OnlyForVisual", 1f);
            }
        }
    }

    public void PlaySound(StudioEventEmitter soundFx)
    {
        if (soundFx)
        {
            if (soundFx.IsPlaying())
                soundFx.Stop();

            soundFx.Play();
        }
    }

    public void ExternalCallPrint()
    {
        e_OnPrint.Invoke();
    }

    public void OnlyForVisual()
    {
        pm.SetLayout(layoutIndex);
        Debug.Log("<color=green>Index: " + layoutIndex + "</color>");
    }

    /*pm.canScan = true;
    if (b_NeedsProductInvoice)
    {
        Printer.instance.product = pi.Product;
    }
    Printer.instance.PrintInvoice();
    e_OnPrint.Invoke();
    */

    public void ResetForNextButton()
    {
        for (int i = 0; i < examples.Length; i++)
        {
            if (i == 0)
            {
                examples[i].b_shelf = false;
                examples[i].b_product = false;
            }
            else
            {
                examples[i].actionCheck = false;
            }
        }
    }
}

public enum TypeButton
{
    Simple,
    Check,
    Container,
    Box,
    UnlockScan,
    UpdatePocketStatus,
    UpdateListOfTexts,
    F1Grab,
    MoveCanvas,
    Print,
    UbicationScreen,
    InfoLocation,
    DinamicAmount,
    NextItemPicking,
    Toggle,
    PackingScreen,
}

[System.Serializable]
public class TextosProductos
{
    public Text[] txt_Linea;
}

[System.Serializable]
public class DataBox
{
    public bool b_hasPrinted;
    public ProductInvoice pi;
}