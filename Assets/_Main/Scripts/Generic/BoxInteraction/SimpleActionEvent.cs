using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleActionEvent : MonoBehaviour
{
    public bool b_Solapa_1;
    public bool b_Solapa_2;
    public bool b_SimpleAction = false;
    public UnityEvent e_OnSimpleActionReady;

    void Start()
    {
        if (e_OnSimpleActionReady == null)
        {
            e_OnSimpleActionReady = new UnityEvent();
            e_OnSimpleActionReady.AddListener(OnSimpleActionReady);
        }
    }

    void Update()
    {
        if(b_SimpleAction)
        {
            return;
        }
        if(!b_Solapa_1 || !b_Solapa_2 )
        {
            return;
        }
        if(b_Solapa_1 && b_Solapa_2)
        {
            b_SimpleAction = true;
            e_OnSimpleActionReady.Invoke();
        }
    }

    void OnSimpleActionReady()
    {

    }

    public void SetSolapa_1(bool b_Val)
    {
        b_Solapa_1 = b_Val;
    }

    public void SetSolapa_2(bool b_Val)
    {
        b_Solapa_2 = b_Val;
    }
}
