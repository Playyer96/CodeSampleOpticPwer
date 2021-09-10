using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PocketError : MonoBehaviour
{
    public string s_c1, s_c2;
    public Text txt;

    private void OnEnable()
    {
        txt.text = "El campo Z1 " + s_c1 + " no esta asignado a la ubicacion " + s_c2;
    }
}
