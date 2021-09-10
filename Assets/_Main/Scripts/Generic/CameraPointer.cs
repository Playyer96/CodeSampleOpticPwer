using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class CameraPointer : MonoBehaviour
{
    public Transform t_Out;
    public Transform t_In;
    public LayerMask l_LM;
    public UnityEvent e_OnPhotoOut;
    public UnityEvent e_OnPhotoIn;
    public bool b_In = false;
    public bool b_Out = false;

    private void Start()
    {
        if (e_OnPhotoIn == null)
        {
            e_OnPhotoIn = new UnityEvent();
        }

        if (e_OnPhotoOut == null)
        {
            e_OnPhotoOut = new UnityEvent();
        }
    }

    public Transform GetTransform()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f, l_LM))
        {
            return hit.transform;
        }
        else
        {
            return null;
        }
    }

    public void GetPhotoFocus()
    {
        if (GetTransform() == t_In)
        {
            if (!b_In)
            {
                b_In = true;
                e_OnPhotoIn.Invoke();
            }
        }
        else if(GetTransform() == t_Out)
        {
            if (!b_Out)
            {
                b_Out = true;
                e_OnPhotoOut.Invoke();
            }
        }
    }
}
