using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidueBH : MonoBehaviour
{
    public string s_Name;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == s_Name)
        {
            StartCoroutine(Residue(other.gameObject));
        }
    }

    IEnumerator Residue(GameObject go)
    {
        yield return new WaitForSeconds(.3f);
        go.GetComponent<Rigidbody>().isKinematic = true;
        go.GetComponent<BoxCollider>().enabled = false;
        go.transform.parent = this.transform;
    }
}
