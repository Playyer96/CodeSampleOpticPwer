using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnapSolapa : MonoBehaviour
{
    public Transform t_SnapPoint;
    public float f_Distance;
    public UnityEvent e_OnSnap;
    public bool b_IsSnap = false;
    public bool b_SetRotation;
    public GameObject g_ObjetToSet;
    public Vector3 v_Rotation;

    void Start()
    {
        if (e_OnSnap == null)
        {
            e_OnSnap = new UnityEvent();
            e_OnSnap.AddListener(OnSnap);
        }
    }

    void Update()
    {
        if(b_IsSnap)
        {
            return;
        }
        else
        {
            if(Vector3.Distance(transform.position, t_SnapPoint.position) < f_Distance)
            {
                b_IsSnap = true;
                e_OnSnap.Invoke();
                if (b_SetRotation)
                {
                    g_ObjetToSet.transform.localEulerAngles = v_Rotation;
                }
            }
        }
    }

    void OnSnap()
    {

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(t_SnapPoint.position, f_Distance);
    }
}
