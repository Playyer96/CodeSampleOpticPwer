using System.Collections;
using System.Collections.Generic;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

public class TwoHandInteractor : MonoBehaviour
{
    [Header("Two Hand Interactor")]
    public bool b_LeftAnchore;
    public bool b_RightAnchore;
    public DreamHouseStudios.VR.Interactable i_InteractableLefth;
    public DreamHouseStudios.VR.Interactable i_InteractableRight;
    public Transform t_DesirePosition;
    [Header("Snap Logic")]
    public Transform t_SnapDestination;
    public bool b_Snap = false;
    public float f_Distance;
    [Header("Phisycs Handle")]
    public bool b_GiveBackPhisycs;
    bool b_UseGravity;
    bool b_IsKinematic;
    Rigidbody r_Rb;
    bool b_SetPhisycs = false;
    [Header("Is Parentable Snap")]
    public bool b_IsParentable;
    Transform t_OriginParent;
    [Header("Unity Events")]
    public UnityEvent e_OnGrabTwoHand;
    public UnityEvent e_OnGrabEndTwoHand;
    bool b_IsTwoHandGrab;
    public UnityEvent e_OnSnap;
    bool b_Check;
    public bool isShelf;
    public SpectraUISettings settings;

    void Start()
    {
        if (e_OnSnap == null)
        {
            e_OnSnap = new UnityEvent();
            e_OnSnap.AddListener(OnSnap);
        }

        if (e_OnGrabTwoHand == null)
        {
            e_OnGrabTwoHand = new UnityEvent();
            e_OnGrabTwoHand.AddListener(OnTwoHandGrab);
        }

        if (e_OnGrabEndTwoHand == null)
        {
            e_OnGrabEndTwoHand = new UnityEvent();
            e_OnGrabEndTwoHand.AddListener(OnTwoHandGrabEnd);
        }

        t_DesirePosition = FindObjectOfType<ShllefAnchore>().transform;
        if (b_GiveBackPhisycs)
        {
            r_Rb = GetComponent<Rigidbody>();
            b_UseGravity = r_Rb.useGravity;
            b_IsKinematic = r_Rb.isKinematic;
        }
        if (b_IsParentable)
        {
            t_OriginParent = transform.parent;
        }
    }

    void Update()
    {
        if (b_Snap)
        {
            transform.parent = null;
            return;
        }

        b_LeftAnchore = i_InteractableLefth.beingGrabbed;
        b_RightAnchore = i_InteractableRight.beingGrabbed;
        
        if (b_LeftAnchore && b_RightAnchore)
        {
            if (b_GiveBackPhisycs && !b_SetPhisycs)
            {
                r_Rb.isKinematic = true;
                r_Rb.useGravity = false;
                b_SetPhisycs = true;
            }
            if (!b_IsParentable)
            {
                transform.position = new Vector3(t_DesirePosition.position.x, transform.position.y, t_DesirePosition.position.z);
                transform.localEulerAngles = new Vector3(0, t_DesirePosition.localEulerAngles.y, 0);
            }
            else
            {
                transform.parent = t_DesirePosition;
            }
            if (!b_IsTwoHandGrab)
            {
                b_IsTwoHandGrab = true;
                e_OnGrabTwoHand.Invoke();
            }
        }
        else
        {
            RestoreLogic(b_LeftAnchore);
            RestoreLogic(b_RightAnchore);
        }

        if (settings.experienMode == ExperienMode.Evaluacion && isShelf)
        {
            return;
        }
        
        if (Vector3.Distance(transform.position, t_SnapDestination.position) < f_Distance)
        {
            b_Snap = true;
            transform.SetPositionAndRotation(t_SnapDestination.position, t_SnapDestination.rotation);

            if (e_OnSnap != null)
            {
                e_OnSnap.Invoke();
            }
        }
    }

    /*public void Set_Left(bool b_Val)
    {
        b_LeftAnchore = b_Val;
        RestoreLogic(b_Val);
    }

    public void Set_Right(bool b_Val)
    {
        b_RightAnchore = b_Val;
        RestoreLogic(b_Val);
    }*/

    public void RestoreLogic(bool b_Val)
    {
        if (b_GiveBackPhisycs && !b_Val)
        {
            r_Rb.useGravity = b_UseGravity;
            r_Rb.isKinematic = b_IsKinematic;
            b_SetPhisycs = false;
        }

        if (b_IsParentable)
        {
            transform.parent = t_OriginParent;
        }
        if (!b_Val)
        {
            b_IsTwoHandGrab = false;
            e_OnGrabEndTwoHand.Invoke();
        }
    }

    void OnSnap()
    {

    }

    void OnTwoHandGrab()
    {

    }

    void OnTwoHandGrabEnd()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(t_SnapDestination.position, f_Distance);
    }
}
