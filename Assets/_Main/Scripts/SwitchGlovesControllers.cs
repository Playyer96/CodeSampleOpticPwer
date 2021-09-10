using Hi5_Interaction_Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGlovesControllers : MonoBehaviour
{
    public GameObject[] hands, gloves;

    [ContextMenu("Switch")]
    public void Switch()
    {
        hands[0].SetActive(!hands[0].activeSelf);
        hands[1].SetActive(!hands[1].activeSelf);
        gloves[0].SetActive(!hands[0].activeSelf);
        gloves[1].SetActive(!hands[1].activeSelf);
    }
}
