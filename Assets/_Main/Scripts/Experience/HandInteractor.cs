using UnityEngine;
using DreamHouseStudios.SofasaLogistica;
using DreamHouseStudios.VR;
using HI5;
using UnityEngine.Events;

public class HandInteractor : MonoBehaviour
{
    [Header("Two Hand Interactor")] public bool b_Anchore;
    public DreamHouseStudios.VR.Interactable i_Interactable;
    public Transform t_DesirePosition;
    [Header("Snap Logic")] public Transform t_SnapDestinationStart;

    public Transform t_SnapDestinationEnd;
    public bool b_Snap = false;
    public float f_Distance;
    [Header("Phisycs Handle")] public bool b_GiveBackPhisycs;
    bool b_UseGravity;
    bool b_IsKinematic;
    Rigidbody r_Rb;
    bool b_SetPhisycs = false;
    [Header("Is Parentable Snap")] public bool b_IsParentable;
    Transform t_OriginParent;
    [Header("Unity Events")] public UnityEvent e_OnGrabHand;
    public UnityEvent e_OnGrabEndHand;
    bool b_IsHandGrab;
    public UnityEvent e_OnSnap;
    bool b_Check;

    public GetHandOnCollision getHandOnCollision;

    [HideInInspector] public Transform t_LeftAnchore;
    [HideInInspector] public Transform t_RightAnchore;

    void Start()
    {
        if (e_OnSnap == null)
        {
            e_OnSnap = new UnityEvent();
            e_OnSnap.AddListener(OnSnap);
        }

        if (e_OnGrabHand == null)
        {
            e_OnGrabHand = new UnityEvent();
            e_OnGrabHand.AddListener(OnHandGrab);
        }

        if (e_OnGrabEndHand == null)
        {
            e_OnGrabEndHand = new UnityEvent();
            e_OnGrabEndHand.AddListener(OnHandGrabEnd);
        }

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
        t_DesirePosition = getHandOnCollision.t_DesirePosition;

        if (b_Snap)
        {
            transform.parent = null;
            return;
        }

        b_Anchore = i_Interactable.beingGrabbed;

        if (b_Anchore)
        {
            if (b_GiveBackPhisycs && !b_SetPhisycs)
            {
                r_Rb.isKinematic = true;
                r_Rb.useGravity = false;
                b_SetPhisycs = true;
            }

            if (!b_IsParentable)
            {
                if (t_DesirePosition == null) return;
                transform.position = new Vector3(transform.position.x, transform.position.y,
                    t_DesirePosition.position.z);
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                if (t_DesirePosition == null) return;
                transform.parent = t_DesirePosition;
            }

            if (!b_IsHandGrab)
            {
                b_IsHandGrab = true;
                e_OnGrabHand.Invoke();
            }
        }
        else
        {
            RestoreLogic(b_Anchore);
        }

        if (Vector3.Distance(transform.position, t_SnapDestinationEnd.position) < f_Distance)
        {
            b_Snap = true;

            transform.SetPositionAndRotation(t_SnapDestinationEnd.position, t_SnapDestinationEnd.rotation);

            if (e_OnSnap != null)
            {
                e_OnSnap.Invoke();
            }
        }

        if (Vector3.Distance(transform.position, t_SnapDestinationStart.position) < f_Distance)
        {
            transform.SetPositionAndRotation(t_SnapDestinationStart.position, t_SnapDestinationStart.rotation);
        }
    }

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
            b_IsHandGrab = false;
            e_OnGrabEndHand.Invoke();
        }
    }

    void OnSnap()
    {
    }

    void OnHandGrab()
    {
    }

    void OnHandGrabEnd()
    {
    }

    public void SetLeftHand(Transform hand)
    {
        t_LeftAnchore = hand;
    }

    public void SetRightHand(Transform hand)
    {
        t_RightAnchore = hand;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(t_SnapDestinationEnd.position, f_Distance);
        Gizmos.DrawWireSphere(t_SnapDestinationStart.position, f_Distance);
    }
}