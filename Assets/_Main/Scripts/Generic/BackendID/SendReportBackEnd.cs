using System.Collections.Generic;
using System.Security.Permissions;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SendReportBackEnd : MonoBehaviour
{
    public static SendReportBackEnd Instance;
    public ReportBackend[] idList;

    public List<string> goodReport;
    public List<string> badReport;

    public UserData userData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        CreateList();
    }

    public void CreateList()
    {
        idList = FindObjectsOfType<ReportBackend>();
    }
    
    public void CreateReport()
    {
        for (int i = 0; i < idList.Length; i++)
        {
            if (idList[i].isReported)
            {
                goodReport.Add(idList[i].report);
            }
            else
            {
                badReport.Add(idList[i].report);
            }
        }   
    }

    public string[] _GoodReport()
    {
        string[] report = new string[goodReport.Count];
        for (int i = 0; i < goodReport.Count; i++)
        {
            report[i] = goodReport[i];
        }
        return report;
    }

    public string[] _BadReport()
    {
        string[] report = new string[badReport.Count];
        for (int i = 0; i < badReport.Count; i++)
        {
            report[i] = badReport[i];
        }

        return report;
    }
}
