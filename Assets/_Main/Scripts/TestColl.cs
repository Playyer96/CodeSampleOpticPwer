using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestColl : MonoBehaviour
{
    // Start is called before the first frame update
    //public Transform target_;

    //void Update()
    //{
    //    transform.position = new Vector3(target_.position.x, target_.position.y, target_.position.z);
    //}

    public Vector3 v;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Break();
        v = transform.position;
        Debug.Break();
        transform.SetParent(other.transform);
        Debug.Break();
        Debug.Log(transform.position);
        Debug.Break();
    }

    private void OnTriggerExit(Collider other)
    {
        transform.SetParent(null);
    }
}
