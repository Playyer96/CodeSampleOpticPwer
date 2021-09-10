using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketFlowControl : MonoBehaviour
{
    [SerializeField]
    public PocketElements[] p_PocketELements;

    void Start()
    {

    }

    public void DisablePocketFunctions()
    {
        for (int i = 0; i < p_PocketELements.Length; i++)
        {
            for (int j = 0; j < p_PocketELements[i].p_PocketFunctions.Length; j++)
            {
                p_PocketELements[i].p_PocketFunctions[j].enabled = false;
            }
        }
    }

    public void EnableAllPocketFUnctions()
    {
        for (int i = 0; i < p_PocketELements.Length; i++)
        {
            for (int j = 0; j < p_PocketELements[i].p_PocketFunctions.Length; j++)
            {
                p_PocketELements[i].p_PocketFunctions[j].enabled = true;
            }
        }
    }

    public void ActivePocketFunctions(int i_IndexScreen, int i_Min, int i_Max)
    {

        for (int i = 0; i < p_PocketELements.Length; i++)
        {
            for (int j = 0; j < p_PocketELements[i].p_PocketFunctions.Length; j++)
            {
                if (i != i_IndexScreen)
                {
                    p_PocketELements[i].p_PocketFunctions[j].enabled = false;
                }
                else
                {
                    if(j >= i_Min && j<= i_Max)
                    {
                         p_PocketELements[i].p_PocketFunctions[j].enabled = true;
                    }
                }
            }
        }
    }
}
[System.Serializable]
public class PocketElements
{
    public PocketFunctions[] p_PocketFunctions;
}
