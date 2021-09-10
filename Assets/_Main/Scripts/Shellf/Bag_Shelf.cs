using DreamHouseStudios.SofasaLogistica;
using UnityEngine;

public class Bag_Shelf : MonoBehaviour
{
    public DreamHouseStudios.VR.Interactable i_Interactable;
    public bool b_IsGrabbed;
    public bool b_IsDeferred;
    public bool b_HasReceptionInvoice;
    public bool b_IsInShlef;
    public bool b_Ready;
    public Transform t_StickPoint;
    private TutorialReception tr;
    private ShelfPacakage s_sp;
    public bool b_Info;

    private void Start()
    {
        tr = FindObjectOfType<TutorialReception>();
        i_Interactable = GetComponent<DreamHouseStudios.VR.Interactable>();
    }

    private void Update()
    {
        CheckReceptionInvoice();
        UpdateGrabb();
    }

    void UpdateGrabb()
    {
        b_IsGrabbed = i_Interactable.beingGrabbed;
    }

    void CheckReceptionInvoice()
    {
        if (tr != null)
        {
            if (!b_HasReceptionInvoice || !b_IsInShlef)
            {
                b_Ready = false;
                if (tr.b_Products.Contains(this))
                {
                    tr.b_Products.Remove(this);
                }
            }
            else
            {
                if (!b_Ready)
                {
                    b_Ready = true;
                    if (!tr.b_Products.Contains(this))
                    {
                        tr.b_Products.Add(this);
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (b_IsDeferred)
        {
            if (other.tag == "Deferred")
            {
                if (!b_IsGrabbed && other.transform.parent != transform.parent)
                {
                    transform.parent = other.transform;
                    b_IsInShlef = true;
                    s_sp.AddDeferred(this);
                    if (tr.s_Settings.experienMode == ExperienMode.Evaluacion)
                    {
                        tr.g_ShelfHighLight.SetActive(false);
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
            if (other.tag == "Estanteria")
            {
                if (!b_IsGrabbed && other.transform != transform.parent)
                {
                    transform.parent = other.transform;
                    b_IsInShlef = true;
                }
                else
                {
                    return;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (b_IsDeferred)
        {
            if (other.tag == "Deferred")
            {
                b_IsInShlef = false;
                transform.parent = null;
            }
        }
        else
        {
            if (other.tag == "Estanteria")
            {
                b_IsInShlef = false;
                transform.parent = null;
            }
        }
    }

    public void Get_Component()
    {
        s_sp = FindObjectOfType<ShelfPacakage>();
    }

    public void SetReceptionInvoice(Transform tr)
    {
        tr.position = t_StickPoint.position;
        tr.rotation = t_StickPoint.rotation;
        tr.parent = t_StickPoint;
        b_HasReceptionInvoice = true;
    }

    public void SetInfo()
    {
        if (!b_Info )
        {
            if (tr.s_Settings.experienMode == ExperienMode.Entrenamiento)
            {
                Debug.Log("<color=red>Error</color>");
                b_Info = true;
                //tr.StopTutorial();
                tr.i_index = 36;
                StartCoroutine(tr.ReceptionTutorial());
            }
            else
            {
                tr.g_ShelfHighLight.SetActive(true);
            }
        }
    }
}