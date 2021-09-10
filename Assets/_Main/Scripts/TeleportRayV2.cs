using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DreamHouseStudios.VR;

public class TeleportRayV2 : MonoBehaviour
{
    public Teleport teleport;
    public DreamHouseStudios.VR.Interactable pocketInteractable;
    public DreamHouseStudios.VR.Interactable cameraInteractable;
    [SerializeField] private LayerMask layerMask;
    public Teleport[] teleports;
    public LineRenderer l_lr;
    public Transform t_CanvasUI;
    public Image i_ImgCarga;
    private float f_Time;
    public float f_TimeToWait;
    public TypeInput actualInput;
    public Teleport t_Teleport;
    public HandGestures handGestures;

    private void Awake()
    {
        i_ImgCarga = t_CanvasUI.GetComponentInChildren<Image>();
        t_Teleport = FindObjectOfType<Teleport>();
    }

    private void Start()
    {
        teleports = FindObjectsOfType<Teleport>();
        f_Time = 0;
        i_ImgCarga.fillAmount = 0;
    }

    private void OnEnable()
    {
        f_Time = 0;
        i_ImgCarga.fillAmount = 0;
        t_CanvasUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (actualInput == TypeInput.Gloves)
        {
            l_lr.SetPosition(0, Vector3.zero);
            l_lr.SetPosition(1, Vector3.zero);
        }
        else
        {
            l_lr.SetPosition(0, transform.position);
            l_lr.SetPosition(1, transform.position);
        }

        if (handGestures != null && handGestures.isPointing == false) return;
        if (cameraInteractable && cameraInteractable.beingGrabbed) return;
        if (pocketInteractable && pocketInteractable.beingGrabbed) return;

        //if (Vector3.Dot(l_lr.transform.forward, Vector3.up) < 0f)
        //{
        RaycastHit hit;
        if (Physics.Raycast(l_lr.transform.position, l_lr.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            if (actualInput == TypeInput.Gloves)
            {
                l_lr.SetPosition(1, Vector3.forward * 100f);
            }
            else
            {
                l_lr.SetPosition(1, transform.position + transform.forward * 100f);
            }

            if (hit.collider.tag == "Teleport" || hit.collider.tag == "Button")
            {
                if (actualInput == TypeInput.Gloves)
                {
                    l_lr.SetPosition(1, new Vector3(0, 0, Vector3.Distance(transform.position, hit.point)));
                }
                else
                {
                    l_lr.SetPosition(1, hit.point);
                }

                f_Time += Time.deltaTime;
                if (f_Time >= f_TimeToWait)
                {
                    f_Time = 0;
                    i_ImgCarga.fillAmount = 0;
                    if (hit.transform.GetComponent<TeleportInedx>() != null)
                    {
                        if (hit.transform.GetComponent<TeleportInedx>().i_MyIndex != t_Teleport.i_ActualIndex)
                        {
                            teleport.SetPlayerTransform(hit.transform);
                            t_Teleport.i_ActualIndex = hit.transform.GetComponent<TeleportInedx>().i_MyIndex;
                        }
                    }
                    else if (hit.transform.GetComponent<BotonCambio>() != null)
                    {
                        hit.transform.GetComponent<BotonCambio>().ButtonBH();
                    }
                }
                t_CanvasUI.position = Vector3.Lerp(hit.point, transform.position, 0.1f);
                t_CanvasUI.LookAt(transform.position);
                t_CanvasUI.gameObject.SetActive(true);
                i_ImgCarga.fillAmount = f_Time / f_TimeToWait;
            }
            else
            {
                InfiniteTeleportRay();
            }
        }
        else
        {
            InfiniteTeleportRay();
        }

        /*}
        else
        {
            ResetTeleportRay();
        }*/
    }

    public void ResetTeleportRay()
    {
        if (actualInput == TypeInput.Gloves)
        {
            l_lr.SetPosition(1, new Vector3(0, 0, 0f));
        }
        else
        {
            l_lr.SetPosition(1, transform.position);
        }

        f_Time = 0;
        t_CanvasUI.gameObject.SetActive(false);
        i_ImgCarga.fillAmount = 0;
    }

    private void InfiniteTeleportRay()
    {
        if (actualInput == TypeInput.Gloves)
        {
            l_lr.SetPosition(1, Vector3.forward * 100f);
        }
        else
        {
            l_lr.SetPosition(1, transform.position + transform.forward * 100f);
        }

        f_Time = 0;
        t_CanvasUI.gameObject.SetActive(false);
        i_ImgCarga.fillAmount = 0;
    }
}

public enum TypeInput
{
    Gloves,
    Controls
}