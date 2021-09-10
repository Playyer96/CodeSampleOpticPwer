using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellfController : MonoBehaviour
{
    public Transform t_Target;

    private void Update() 
    {
        transform.position = t_Target.position;
        transform.rotation = t_Target.rotation;    
    }
}
