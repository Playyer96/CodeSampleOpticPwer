using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class DetectControllers : MonoBehaviour
{
    public HandsHandler priority;

    public List<HandsHandlerEvents> handsHandlerEvents = new List<HandsHandlerEvents>() { new HandsHandlerEvents() { handler = HandsHandler.NONE }, new HandsHandlerEvents() { handler = HandsHandler.HI5 }, new HandsHandlerEvents() { handler = HandsHandler.Vive_Controller } };

    public List<HandsHandler> handsHandlers;

    //public GameObject hi5Hands, controllerHands;

    public HandsHandler currentHandler
    {
        get
        {
            handsHandlers.RemoveAll(i => i == HandsHandler.NONE);
            if (handsHandlers.Count == 0)
                return HandsHandler.NONE;

            if (priority == HandsHandler.NONE)
            {
                return handsHandlers.FindAll(i => i == HandsHandler.HI5).Count > handsHandlers.FindAll(i => i == HandsHandler.Vive_Controller).Count ? HandsHandler.HI5 : HandsHandler.Vive_Controller;
            }
            else
            {
                return handsHandlers.FindAll(i => i == priority).Count > 0 ? priority : (handsHandlers.FindAll(i => i == HandsHandler.HI5).Count > 0 ? HandsHandler.HI5 : HandsHandler.Vive_Controller);
            }
        }
    }

    public DetectControllersScriptable detectControllersScriptable;

    private void Start()
    {
        Debug.Log(detectControllersScriptable.lastEnabledHandler);

        //Debug.Break();
        //foreach (HandsHandlerEvents hhe in handsHandlerEvents)

        //Debug.Break();

        foreach (HandsHandlerEvents hhe in handsHandlerEvents)
        {
            hhe.isCurrent = false;
            if (hhe.handler == detectControllersScriptable.lastEnabledHandler)
            {
                if (!hhe.isCurrent)
                {
                    hhe.isCurrent = true;
                    hhe.onChoose.Invoke();
                }
            }
            else
            {
                if (hhe.isCurrent)
                {
                    hhe.isCurrent = false;
                    hhe.onDiscard.Invoke();
                }
            }
        }

        //SteamVR_Events.DeviceConnected.Listen(OnDeviceConnected);
        //Activate();
    }

    private void OnDestroy()
    {
        //SteamVR_Events.DeviceConnected.Remove(OnDeviceConnected);
    }

    private void OnEnable()
    {
        SteamVR_Events.DeviceConnected.Listen(OnDeviceConnected);
        //HI5.HI5_GloveStatus e = new HI5.HI5_GloveStatus();
        //bool av = e.IsGloveAvailable(HI5.Hand.LEFT);
    }

    private void OnDisable()
    {
        SteamVR_Events.DeviceConnected.Remove(OnDeviceConnected);
    }

    private void OnDeviceConnected(int index, bool connected)
    {
        var error = ETrackedPropertyError.TrackedProp_Success;
        var result = new System.Text.StringBuilder((int)64);
        OpenVR.System.GetStringTrackedDeviceProperty((uint)index, ETrackedDeviceProperty.Prop_RenderModelName_String, result, 64, ref error);
        if (result.ToString().Contains("tracker"))
        {
            if (connected)
                handsHandlers.Add(HandsHandler.HI5);
            else
            {
                int item = handsHandlers.FindIndex(i => i == HandsHandler.HI5);
                if (item >= 0)
                    handsHandlers.RemoveAt(item);
            }
        }
        else if (result.ToString().Contains("controller"))
        {
            if (connected)
                handsHandlers.Add(HandsHandler.Vive_Controller);
            else
            {
                int item = handsHandlers.FindIndex(i => i == HandsHandler.Vive_Controller);
                if (item >= 0)
                    handsHandlers.RemoveAt(item);
            }
        }

        // if (OpenVR.System != null)
        // {
        //     //lets figure what type of device got connected
        //     ETrackedDeviceClass deviceClass = OpenVR.System.GetTrackedDeviceClass((uint)index);

        //     if (handsHandlers == null)
        //         handsHandlers = new List<HandsHandler>();

        //     switch (deviceClass)
        //     {
        //         case ETrackedDeviceClass.GenericTracker:
        //             if (connected)
        //                 handsHandlers.Add(HandsHandler.HI5);
        //             else
        //             {
        //                 int item = handsHandlers.FindIndex(i => i == HandsHandler.HI5);
        //                 if (item >= 0)
        //                     handsHandlers.RemoveAt(item);
        //             }
        //             break;
        //         case ETrackedDeviceClass.Controller:
        //             if (connected)
        //                 handsHandlers.Add(HandsHandler.Vive_Controller);
        //             else
        //             {
        //                 int item = handsHandlers.FindIndex(i => i == HandsHandler.Vive_Controller);
        //                 if (item >= 0)
        //                     handsHandlers.RemoveAt(item);
        //             }
        //             break;
        //     }
        // }
        Activate();
    }

    public void Activate()
    {
        foreach (HandsHandlerEvents hhe in handsHandlerEvents)
        {
            if (hhe.handler == currentHandler)
            {
                if (!hhe.isCurrent)
                {
                    hhe.isCurrent = true;
                    hhe.onChoose.Invoke();
                }
            }
            else
            {
                if (hhe.isCurrent)
                {
                    hhe.isCurrent = false;
                    hhe.onDiscard.Invoke();
                }
            }
        }

        detectControllersScriptable.lastEnabledHandler = currentHandler;

        //switch (currentHandler)
        //{
        //    case HandsHandler.NONE:
        //        hi5Hands.SetActive(false);
        //        controllerHands.SetActive(false);

        //        break;
        //    case HandsHandler.HI5:
        //        hi5Hands.SetActive(true);
        //        controllerHands.SetActive(false);
        //        break;
        //    case HandsHandler.Vive_Controller:
        //        hi5Hands.SetActive(false);
        //        controllerHands.SetActive(true);
        //        break;
        //}
    }

    public enum HandsHandler
    {
        NONE = -1,
        HI5,
        Vive_Controller
    }

    [System.Serializable]
    public class HandsHandlerEvents
    {
        public HandsHandler handler;
        public bool isCurrent = false;
        public UnityEvent onChoose, onDiscard;
    }
}