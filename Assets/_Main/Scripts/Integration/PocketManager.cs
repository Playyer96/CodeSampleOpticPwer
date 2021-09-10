using System.Collections;
using System.Collections.Generic;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;

public class PocketManager : MonoBehaviour
{
    public bool canScan = false;
    public PocketScaner p_pocketScaner;
    public bool canSize = false;
    public Transform t_Canvas;
    public Transform t_Target;
    public GameObject[] g_Layouts;
    public PositionAndSize[] c_PositionAndSize;
    public float f_wait;
    float f_refT = 0;
    float f_refT2 = 0;
    bool isBig = false;
    public PocketState actualState;
    public bool b_isGloves = false;
    float t_t1;
    float t_t2;
    public SpectraUISettings s_Settings;
    void Start()
    {
        p_pocketScaner.GetComponent<BoxCollider>().enabled = false;
        InitializeLayout();
    }

    void Update()
    {
        if (Vector3.Dot(t_Canvas.up, Vector3.up) < .4f)
        {
            t_Canvas.localScale = c_PositionAndSize[0].v_Size;
            t_Canvas.localPosition = c_PositionAndSize[0].v_Position;
            if (canScan)
            {
                p_pocketScaner.CanScan();
                if (b_isGloves)
                {
                    ScannGloves();
                }
            }
        }
        else
        {
            t_t1 = 0;
            t_t2 = 0;
            p_pocketScaner.NotScan();
            if (b_CanvasIsLooking() && b_TargetIsLooking() && canSize)
            {
                if (!isBig)
                {
                    f_refT += Time.deltaTime;
                    if (f_refT >= f_wait)
                    {
                        isBig = true;
                    }
                }
                else
                {
                    f_refT2 = 0;
                    t_Canvas.localScale = c_PositionAndSize[1].v_Size;
                    t_Canvas.localPosition = c_PositionAndSize[1].v_Position;
                }
            }
            else
            {
                if (isBig)
                {
                    f_refT2 += Time.deltaTime;
                    if (f_refT2 >= f_wait)
                    {
                        isBig = false;
                    }
                }
                else
                {
                    f_refT = 0;
                    t_Canvas.localScale = c_PositionAndSize[0].v_Size;
                    t_Canvas.localPosition = c_PositionAndSize[0].v_Position;
                }
            }
        }
        if (!canSize)
        {
            t_Canvas.localScale = c_PositionAndSize[0].v_Size;
            t_Canvas.localPosition = c_PositionAndSize[0].v_Position;
        }
    }
    public void InitializeLayout()
    {
        SetLayout(0);
        t_Canvas.localScale = c_PositionAndSize[0].v_Size;
        t_Canvas.localPosition = c_PositionAndSize[0].v_Position;
    }
    public void SetLayout(int index)
    {
        canScan = false;
        for (int i = 0; i < g_Layouts.Length; i++)
        {
            g_Layouts[i].SetActive(false);
        }
        g_Layouts[index].SetActive(true);
        if (s_Settings.experienMode == ExperienMode.Evaluacion || s_Settings.experienMode == ExperienMode.Entrenamiento)
        {
            if (index == 7 || index == 8 ||index == 10||index == 13||index == 18)
            {
                canScan = true;
            }
        }
    }

    bool b_CanvasIsLooking()
    {
        bool isLooking = false;
        Vector3 v_Back = t_Canvas.TransformDirection(Vector3.back);
        Vector3 v_Right = t_Canvas.TransformDirection(Vector3.right);
        Vector3 v_UP = t_Canvas.TransformDirection(Vector3.up);
        Vector3 target = (t_Target.position - t_Canvas.position).normalized;
        if (Vector3.Dot(target, v_Back) > 0.5f && Vector3.Dot(target, v_Right) > -0.5f && Vector3.Dot(target, v_Right) < 0.5f && Vector3.Dot(target, v_UP) > -0.5 && Vector3.Dot(target, v_UP) < 0.5f)
        {
            isLooking = true;
        }
        return isLooking;
    }

    bool b_TargetIsLooking()
    {
        bool isLooking = false;
        Vector3 v_forward = t_Target.TransformDirection(Vector3.forward);
        Vector3 v_Right = t_Target.TransformDirection(Vector3.right);
        Vector3 v_Up = t_Target.TransformDirection(Vector3.up);
        Vector3 target = (t_Canvas.position - t_Target.position).normalized;
        if (Vector3.Dot(target, v_forward) > 0.5f && Vector3.Dot(target, v_Right) > -0.5f && Vector3.Dot(target, v_Right) < 0.5f && Vector3.Dot(target, v_Up) > -0.5f && Vector3.Dot(target, v_Up) < 0.5f)
        {
            isLooking = true;
        }
        return isLooking;
    }

    public void OnPocketGrip()
    {
        canSize = true;
    }

    public void OnPocketRelease()
    {
        canSize = false;
    }
    public void ActuallizePocketState(PocketState state)
    {
        actualState = state;
    }

    public void GripToScan()
    {
        if (canScan && p_pocketScaner.canScan)
        {
            StartCoroutine(ActiveScan());
        }
    }

    IEnumerator ActiveScan()
    {
        p_pocketScaner.GetComponent<BoxCollider>().enabled = true;
        p_pocketScaner.ActiveFlash();
        yield return new WaitForSeconds(0.3f);
        p_pocketScaner.DeactiveFlash();
        yield return new WaitForSeconds(0.95f);
        p_pocketScaner.GetComponent<BoxCollider>().enabled = false;
    }

    void ScannGloves()
    {
        p_pocketScaner.GetComponent<BoxCollider>().enabled = true;
        p_pocketScaner.ActiveFlash();
        t_t1 += Time.deltaTime;
        if (t_t1 > 0.15f)
        {
            p_pocketScaner.DeactiveFlash();
            t_t2 += Time.deltaTime;
            if(t_t2 > .8f)
            {
                p_pocketScaner.GetComponent<BoxCollider>().enabled = false;
            }
        }        
    }

    public void BoolSetGloves(bool b_value)
    {
        b_isGloves = b_value;
    }
}

[System.Serializable]
public class PositionAndSize
{
    public Vector3 v_Size;
    public Vector3 v_Position;
}

public enum PocketState
{
    Containers,
    Box
}
