using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILabel : MonoBehaviour
{
    public LeanTweenType inType = LeanTweenType.linear;

    public float time = 0.6f;
    public float moveAmount = 65f;

    RectTransform rt;
    Vector3 originalPos;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        originalPos = rt.anchoredPosition3D;
    }

    private void OnEnable()
    {
        rt = GetComponent<RectTransform>();
        rt.anchoredPosition3D = originalPos;
        LeanTween.move(rt, rt.anchoredPosition3D + new Vector3(moveAmount, 0f, 0f), time);
    }
}
