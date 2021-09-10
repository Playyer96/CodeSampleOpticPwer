using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideToolSet : MonoBehaviour
{
   public GameObject[] g_toolset;
   public DreamHouseStudios.VR.GrabInteractables[] hands;
   public DreamHouseStudios.VR.Interactable[] interactables;
   public CleanerPocket[] cp;
    void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < hands.Length; i++)
        {
            for (int j = 0; j < interactables.Length; j++)
            {
                if (hands[i].hovered.Contains(interactables[j]))
                {
                    hands[i].hovered.Remove(interactables[j]);
                }
            }
        }

        for (int i = 0; i < cp.Length; i++)
        {
            if (cp[i].gameObject.activeSelf)
            {
                cp[i].b_Clean = false;
            }
        }
        
        for (int i = 0; i < g_toolset.Length; i++)
        {
            g_toolset[i].SetActive(false);
        }
    }

        void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < g_toolset.Length; i++)
        {
            g_toolset[i].SetActive(true);
        }
    }
}
