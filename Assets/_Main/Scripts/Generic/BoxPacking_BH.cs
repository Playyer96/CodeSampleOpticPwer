using System;
using UnityEngine.Events;
using UnityEngine;
using Valve.VR;

public class BoxPacking_BH : MonoBehaviour
{
    public Transform[] stickPoints;
    public UnityEvent e_OnStickInvoice;

    private void Start()
    {
        if (e_OnStickInvoice == null)
        {
            e_OnStickInvoice = new SteamVR_Events.Event();
        }
    }

    public void SetInvoice(Transform tr)
    {
        int index = 0;
        float minDis = 0;
        for (int i = 0; i < stickPoints.Length; i++)
        {
            if (i == 0)
            {
                minDis = Vector3.Distance(tr.position, stickPoints[i].position);
                index = 0;
            }

            if (Vector3.Distance(tr.position, stickPoints[i].position) < minDis)
            {
                minDis = Vector3.Distance(tr.position, stickPoints[i].position);
                index = i;
            }
        }

        tr.position = stickPoints[index].position;
        tr.rotation = stickPoints[index].rotation;
        tr.parent = stickPoints[index];
        e_OnStickInvoice.Invoke();
    }
}
