using System;
using UnityEngine;
using UnityEngine.Events;

public class TeleportInedx : MonoBehaviour
{
    public int i_MyIndex;
    public bool b_IsShelfSnap;
    public UnityEvent e_OnTp;
    public bool b_Event = false;
    public e_Condition actualCondition;

    private void Awake()
    {
        if (e_OnTp == null && b_Event)
        {
            e_OnTp = new UnityEvent();
        }
    }

    public void LaunchEvent()
    {
        if (b_Event)
        {
            switch (actualCondition)
            {
                case e_Condition.Evnent:
                    if (!b_IsShelfSnap)
                    {
                        e_OnTp.Invoke();
                    }
                    break;
            }
        }
    }
    
    public enum e_Condition
    {
        Evnent
    }

    public void SetBool(bool val)
    {
        b_IsShelfSnap = val;
    }
}
