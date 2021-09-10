using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckListManager : MonoBehaviour
{
    public GameObject[] g_CheckList;

    public void SetCheckList(int index)
    {
        for (int i = 0; i < g_CheckList.Length; i++)
        {
            if (i >= 0)
            {
               g_CheckList[i].SetActive(i == index?true:false);
            }
        }
    }
}
