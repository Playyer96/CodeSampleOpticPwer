using System;
using System.Collections;
using System.Collections.Generic;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;

public class GenericResetChronometer : MonoBehaviour
{
    private Chronometer timer;

    private void Awake()
    {
        timer = FindObjectOfType<Chronometer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        timer.ResetSetionChronometer();
    }
}
