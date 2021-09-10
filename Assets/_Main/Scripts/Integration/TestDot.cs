using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDot : MonoBehaviour
{
    public Transform t_t1;
    public Transform t_t2;

    private void Update() 
    {
        T1_IsLooking();
        t2_IsLookin();
    }

    bool T1_IsLooking()
    {
        bool isLooking = false;
        Vector3 v_f = t_t1.forward;
        Vector3 v_r = t_t1.right;
        Vector3 v_u = t_t1.up;
        Vector3 v_target = (t_t2.position - t_t1.position).normalized;

        if(Vector3.Dot(v_target, v_f) > 0.8f && Vector3.Dot(v_target, v_r)>-.5f && Vector3.Dot(v_target, v_r)<.5f &&Vector3.Dot(v_target, v_u) >-.5f && Vector3.Dot(v_target,v_u)<.5f )
        {
            Debug.Log("<color=green>T1 is looking</color>");
            isLooking = true;
        }
        else
        {
            Debug.Log("<color=red>T1 is NOT looking</color>");
        }
        return isLooking;
    }

    bool t2_IsLookin()
    {
        bool isLooking = false;
        Vector3 v_f = t_t2.TransformDirection(Vector3.forward); 
        Vector3 v_r = t_t2.TransformDirection(Vector3.right);
        Vector3 v_u = t_t2.TransformDirection(Vector3.up);
        Vector3 v_target = (t_t1.position - t_t2.position).normalized;

        if(Vector3.Dot(v_target, v_f) > 0.8f && Vector3.Dot(v_target, v_r)>-.5f && Vector3.Dot(v_target, v_r)<.5f &&Vector3.Dot(v_target, v_u) >-.5f && Vector3.Dot(v_target,v_u)<.5f )
        {
            Debug.Log("<color=blue>T2 is looking</color>");
            isLooking = true;
        }
        else
        {
            Debug.Log("<color=magenta>T2 is NOT looking</color>");
        }
        return isLooking;
    }
}
