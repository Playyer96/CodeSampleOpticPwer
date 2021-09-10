using System.Collections;
using System.Collections.Generic;
using DreamHouseStudios.SofasaLogistica;
using UnityEngine;

public class ResetUserData : MonoBehaviour
{
    private ScenesManager sm;
    // Start is called before the first frame update
    void Start()
    {
        sm = FindObjectOfType<ScenesManager>();
        sm.userData.Init();
    }
}
