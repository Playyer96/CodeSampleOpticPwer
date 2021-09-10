using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsLogic : MonoBehaviour
{
    public Transform[] i_Interactable;

    public void SetObjects(int i_Min, int i_Max)
    {
        for (int i = 0; i < i_Interactable.Length; i++)
        {
            if (i >= i_Min && i <= i_Max)
            {
                if (i_Interactable[i] != null)
                {
                    if (i_Interactable[i].GetComponent<DreamHouseStudios.VR.Interactable>() != null)
                    {
                        i_Interactable[i].GetComponent<DreamHouseStudios.VR.Interactable>().enabled = true;
                    }
                    else
                    {
                        i_Interactable[i].GetComponentInChildren<DreamHouseStudios.VR.Interactable>().enabled = true;
                    }
                }
            }
            else
            {
                if (i > -1)
                {
                    if (i_Interactable[i] != null)
                    {
                        if (i_Interactable[i].GetComponent<DreamHouseStudios.VR.Interactable>() != null)
                        {
                            i_Interactable[i].GetComponent<DreamHouseStudios.VR.Interactable>().enabled = false;
                        }
                        else
                        {
                            i_Interactable[i].GetComponentInChildren<DreamHouseStudios.VR.Interactable>().enabled = false;
                        }
                    }
                }
            }
        }
    }
}
