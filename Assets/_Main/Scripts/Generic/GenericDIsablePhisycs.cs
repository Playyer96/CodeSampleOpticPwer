using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericDIsablePhisycs : MonoBehaviour {
    public void DisablePhisycs () {
        StartCoroutine (StopPhisycs ());
    }

    IEnumerator StopPhisycs () {
        yield return new WaitForSeconds (.1f);
        GetComponent<Rigidbody> ().isKinematic = true;
        GetComponent<Rigidbody> ().useGravity = false;
        GetComponent<ReportToCheckList> ().SetCheckList (true);
    }
}