using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.Events;

public class ItemID : MonoBehaviour {
    public string id;
    public bool progressSet = false;
    public bool isSet = false;
    private bool b_Check = false;
    public UnityEvent e_OnTrashEnter;

    public bool needReport;
    public ReportBackend rp;

    private void Start()
    {
        if (e_OnTrashEnter == null)
        {
            e_OnTrashEnter = new UnityEvent();
        }

        if (rp == null && needReport)
        {
            rp = GetComponent<ReportBackend>();
        }
    }

    private void Update()
    {
        if (b_Check)
        {
            return;
        }
        else
        {
            if (isSet)
            {
                if (needReport)
                {
                    rp.isReported = true;
                }

                b_Check = true;
                e_OnTrashEnter.Invoke();
            }
        }
    }
}