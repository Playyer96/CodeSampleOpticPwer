using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class TrackerDetectTest : MonoBehaviour
{
    public Text text;

    private void OnEnable()
    {
        SteamVR_Events.DeviceConnected.Listen(OnDeviceConnected);
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
        text.text += result.ToString() + " " + (connected ? "On." : "Off.") + Environment.NewLine;
    }
}
