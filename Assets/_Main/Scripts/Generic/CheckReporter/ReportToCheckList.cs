using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportToCheckList : MonoBehaviour
{
    public string s_Name;
    public string s_Item;

    public void SetCheckList(bool b_Val)
    {
        Checklist.Set(s_Name, s_Item, b_Val);
    }
}
