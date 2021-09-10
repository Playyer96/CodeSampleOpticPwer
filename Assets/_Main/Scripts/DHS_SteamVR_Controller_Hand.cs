using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class DHS_SteamVR_Controller_Hand : MonoBehaviour
{
    //public SteamVR_Action_Single action_single;
    public SteamVR_Input_Sources actionInput;

    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean gripAction;
    public SteamVR_Action_Boolean pointAction;

    public GestureEvent onGrab, onGrabRelease, onPoint, onPointRelease, onGrip, onGripRelease;

    private bool grabbing, pointing;

    private bool initialized;

    public bool indexByUpdate;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        //svrto.index = (SteamVR_TrackedObject.EIndex)SteamVR_Action_Boolean.GetDeviceIndex(actionInput);
        initialized = true;
    }

    private void OnEnable()
    {
        SteamVR_TrackedObject svrto = GetComponent<SteamVR_TrackedObject>();

        grabAction.AddOnUpdateListener(GrabUpdate, actionInput);

        grabAction.AddOnStateDownListener(Grab, actionInput);
        grabAction.AddOnStateUpListener(GrabRelease, actionInput);

        gripAction.AddOnStateDownListener(Grip, actionInput);
        gripAction.AddOnStateUpListener(GripRelease, actionInput);

        pointAction.AddOnStateDownListener(PointDown, actionInput);
        pointAction.AddOnStateUpListener(PointUp, actionInput);
    }

    private void OnDisable()
    {
        grabAction.RemoveOnUpdateListener(GrabUpdate, actionInput);

        grabAction.RemoveOnStateDownListener(Grab, actionInput);
        grabAction.RemoveOnStateUpListener(GrabRelease, actionInput);

        gripAction.RemoveOnStateDownListener(Grip, actionInput);
        gripAction.RemoveOnStateUpListener(GripRelease, actionInput);

        pointAction.RemoveOnStateDownListener(PointDown, actionInput);
        pointAction.RemoveOnStateUpListener(PointUp, actionInput);
    }

    private void PointDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        _Point();
    }

    private void PointUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        _PointRelease();
    }

    private void GrabUpdate(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (!indexByUpdate) return;
        SteamVR_TrackedObject svrto = GetComponent<SteamVR_TrackedObject>();
        svrto.index = (SteamVR_TrackedObject.EIndex)grabAction.GetDeviceIndex(actionInput);
        //svrto.index = (SteamVR_TrackedObject.EIndex)fromAction.trackedDeviceIndex;
        //switch (actionInput)
        //{
        //    case SteamVR_Input_Sources.RightHand:
        //        break;
        //    case SteamVR_Input_Sources.LeftHand:
        //        svrto.index = (SteamVR_TrackedObject.EIndex)OpenVR.System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.LeftHand);
        //        break;
        //}
    }

    private void Grab(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        _Grab();
    }

    private void GrabRelease(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        _GrabRelease();
    }

    private void Grip(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!initialized) return;
        onGrip.Invoke(this);
        Debug.Log("Grip");
    }

    private void GripRelease(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!initialized) return;
        onGripRelease.Invoke(this);
        Debug.Log("GripRelease");
    }

    public void _Grab()
    {
        if (!initialized) return;
        grabbing = true;
        onGrab.Invoke(this);
        Debug.Log("Grab: " + actionInput.ToString());
    }

    public void _GrabRelease()
    {
        if (!initialized || !grabbing) return;
        grabbing = false;
        onGrabRelease.Invoke(this);
        Debug.Log("GrabRelease");
    }

    public void _Point()
    {
        if (!initialized) return;
        pointing = true;
        Debug.Log("Point down");
        onPoint.Invoke(this);
    }

    public void _PointRelease()
    {
        if (!initialized || !pointing) return;
        pointing = false;
        Debug.Log("Point up");
        onPointRelease.Invoke(this);
    }

    [System.Serializable]
    public class GestureEvent : UnityEvent<DHS_SteamVR_Controller_Hand> { }
}