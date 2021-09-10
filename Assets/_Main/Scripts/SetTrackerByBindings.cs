using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class SetTrackerByBindings : MonoBehaviour
{
    public ETrackedControllerRole role;

    // public SteamVR_Input_Sources actionInput;
    // public SteamVR_Action_Boolean grabAction;
    // public SteamVR_Action_Single steamVR_Action_Single;
    public SteamVR_TrackedObject tracker;

    // Start is called before the first frame update
    private IEnumerator Check()
    {
        for (int i = 0; i < 10; i++)
        //while (svrto.index == SteamVR_TrackedObject.EIndex.None)
        {
            SteamVR_TrackedObject svrto = GetComponent<SteamVR_TrackedObject>();
            yield return new WaitForSeconds(1f);
            svrto.index = (SteamVR_TrackedObject.EIndex)OpenVR.System.GetTrackedDeviceIndexForControllerRole(role);
            Debug.Log("Role: " + role.ToString() + ". index: " + svrto.index);
            if (svrto.index != SteamVR_TrackedObject.EIndex.None)
                break;
        }
        // svrto.index = (SteamVR_TrackedObject.EIndex)steamVR_Action_Single.GetDeviceIndex(actionInput);

        // for (int i = 0; i < 16; i++)
        // {
        //     var error = ETrackedPropertyError.TrackedProp_Success;
        //     var result = new System.Text.StringBuilder((int)64);
        //     // OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_RenderModelName_String, result, 64, ref error);
        //     OpenVR.System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.LeftHand)
        //     // Debug.Log(result);
        // }
    }

    private void OnEnable()
    {
        StartCoroutine(Check());
    }
}