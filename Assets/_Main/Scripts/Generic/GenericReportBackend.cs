using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericReportBackend : MonoBehaviour
{

    public static GenericReportBackend Instance;
    public UnityEvent onCreateReport;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (onCreateReport == null)
        {
            onCreateReport = new UnityEvent();
        }
    }

    public void LaunchCreateReport()
    {
        onCreateReport.Invoke();
    }
}
