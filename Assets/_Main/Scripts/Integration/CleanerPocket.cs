using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleanerPocket : MonoBehaviour
{
    public PocketFunctions[] p_pf;
    public Text[] txt_inputs;
    public PocketFunctions p_pfUnlockScan;
    public PocketPickingProducts p_pocket;
    public GameObject[] g_ObjectsToHide;
    public bool b_Clean = true;

    private void OnEnable()
    {
        if (b_Clean)
        {
            for (int i = 0; i < txt_inputs.Length; i++)
            {
                txt_inputs[i].text = "";
            }

            for (int j = 0; j < p_pf.Length; j++)
            {
                p_pf[j].actionCheck = false;
            }

            if (p_pfUnlockScan != null)
            {
                p_pfUnlockScan.typeButton = TypeButton.UnlockScan;
            }

            if (p_pocket != null)
            {
                p_pocket.StartInfo();
            }

            for (int i = 0; i < g_ObjectsToHide.Length; i++)
            {
                g_ObjectsToHide[i].SetActive(false);
            }
        }
        b_Clean = true;
    }

    public void CLeanPocketFunction()
    {
        for (int i = 0; i < txt_inputs.Length; i++)
        {
            txt_inputs[i].text = "";
        }

        for (int j = 0; j < p_pf.Length; j++)
        {
            p_pf[j].actionCheck = false;
        }

        if (p_pfUnlockScan != null)
        {
            p_pfUnlockScan.typeButton = TypeButton.UnlockScan;
        }

        if (p_pocket != null)
        {
            p_pocket.StartInfo();
        }

        for (int i = 0; i < g_ObjectsToHide.Length; i++)
        {
            g_ObjectsToHide[i].SetActive(false);
        }
    }
}
