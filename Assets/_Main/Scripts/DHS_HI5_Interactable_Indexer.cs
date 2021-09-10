using Hi5_Interaction_Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHS_HI5_Interactable_Indexer : MonoBehaviour
{
    private void Awake()
    {
        int objectId = 0;
        foreach (Hi5_Glove_Interaction_Item i in FindObjectsOfType<Hi5_Glove_Interaction_Item>())
        {
            i.idObject = objectId;
            objectId++;
        }
    }
}
