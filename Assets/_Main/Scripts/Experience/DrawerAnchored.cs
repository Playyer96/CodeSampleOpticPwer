using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerAnchored : MonoBehaviour
{
    public Transform t_LeftAnchore;
    public Transform t_RightAnchore;

    private Transform t_ParentLeft;
    private Transform t_ParentRight;


    void Update()
    {
        
    }

    public void SetLeftHand(Transform hand)
    {
        t_LeftAnchore = hand;
    }
    public void SetRightHand(Transform hand)
    {
        t_RightAnchore = hand;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(t_LeftAnchore.position, t_RightAnchore.position);
        Gizmos.DrawWireSphere(new Vector3((t_LeftAnchore.position.x + t_RightAnchore.position.x) / 2f, (t_LeftAnchore.position.y + t_RightAnchore.position.y) / 2f, (t_LeftAnchore.position.z + t_RightAnchore.position.z) / 2f), 0.02f);
    }
}
