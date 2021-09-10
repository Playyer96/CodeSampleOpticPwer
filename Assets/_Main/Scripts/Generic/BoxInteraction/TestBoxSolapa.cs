using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoxSolapa : MonoBehaviour
{
    public Transform t_Target;
    public Transform t_Handle;
    public Transform t_Anchore;
    public Vector3 v_Test;
    Quaternion q_TargetRotation;
    public Vector3 initialUpVector;
    public bool b_OnGrab;
    private void Start()
    {
        initialUpVector = transform.up;
    }
    public void SetProjectedPosition()
    {
        t_Handle.SetPositionAndRotation(t_Anchore.position, t_Anchore.rotation);
        Vector3 projectedPosition = transform.InverseTransformPoint(t_Target.position);
        projectedPosition = new Vector3(projectedPosition.x, 0f, projectedPosition.z);
        projectedPosition = transform.TransformPoint(projectedPosition);
        transform.rotation = Quaternion.LookRotation((projectedPosition - transform.position).normalized, initialUpVector);
    }
    void Update()
    {
        initialUpVector = transform.up;
        if (!b_OnGrab)
        {
            return;
        }
        else
        {
            SetProjectedPosition();
        }
    }
    public void SetGrab(bool b_Val)
    {
        if(b_Val)
        {
            t_Target = t_Handle.GetComponent<DreamHouseStudios.VR.Interactable>().currentGrabber.transform;
        }
        else
        {
            t_Target = null;
        }
        b_OnGrab = b_Val;
    }
}
