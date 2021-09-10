using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfSnap : MonoBehaviour
{
    public Transform t_Target;
    public float f_minDistance;
    /*
    public bool b_snap = false;
    public ShllefAnchore s_sh;
    public bool is_TapeBox;
    */

    void Update()
    {
        /*if (!b_snap)
        {
            if (Vector3.Distance(transform.position, t_Target.position) < f_minDistance)
            {
                b_snap = true;
                s_sh.ResetLeft();
                s_sh.ResetRight();
                if (!is_TapeBox)
                {
                    transform.position = t_Target.position;
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
        }*/
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(t_Target.position, f_minDistance);
    }
}
