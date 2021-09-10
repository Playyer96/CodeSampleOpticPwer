using Hi5_Interaction_Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGloves : MonoBehaviour
{
    public Hi5_Hand_Visible_Hand leftGlove, rightGlove, leftHand, rightHand;
    public Hi5_Glove_Interaction_Hand leftHi5, rightHi5;

    [ContextMenu("Switch")]
    public void Switch()
    {
        leftHi5.mVisibleHand = leftHi5.mVisibleHand == leftHand ? leftGlove : leftHand;
        rightHi5.mVisibleHand = rightHi5.mVisibleHand == rightHand ? rightGlove : rightHand;

        leftGlove.gameObject.SetActive(leftHi5.mVisibleHand == leftGlove);
        leftHand.gameObject.SetActive(leftHi5.mVisibleHand == leftHand);
        rightGlove.gameObject.SetActive(rightHi5.mVisibleHand == rightGlove);
        rightHand.gameObject.SetActive(rightHi5.mVisibleHand == rightHand);
    }
}
