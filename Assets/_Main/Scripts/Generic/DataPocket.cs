using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataPocket : MonoBehaviour
{
    [SerializeField] 
    public PocketData[] l_List;
    public bool b_ShowInfo;
    public TestPocketPacking tp;

    private void OnEnable()
    {
        if (b_ShowInfo)
        {
            for (int i = 0; i < tp.l_Data.Count; i++)
            {
                l_List[i].t_Txt[0].text = tp.l_Data[i].i_Amount.ToString();
                l_List[i].t_Txt[1].text = tp.l_Data[i].s_ID;
                l_List[i].t_Txt[2].text = tp.l_Data[i].s_Description;
            }
        }
    }
}


[System.Serializable]
public class PocketData
{
    public Text[] t_Txt;
}
