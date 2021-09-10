using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShllefAnchore : MonoBehaviour
{
    public Transform t_MidlePoint;
    [SerializeField]
    public Transform t_LeftAnchore;
    [SerializeField]
    public Transform t_RightAnchore;
    Transform t_ParentLeft;
    Transform t_ParentRight;
    /*public bool b_Left;
    public bool b_Right;
    Transform t_RealParent;
    public bool b_NeedsParent;
    public bool b_realParent = true;
    public bool b_DeactiveObj = false;
    */

    private void Update()
    {
        t_MidlePoint.transform.position = new Vector3((t_LeftAnchore.position.x + t_RightAnchore.position.x) / 2f, (t_LeftAnchore.position.y + t_RightAnchore.position.y) / 2f, (t_LeftAnchore.position.z + t_RightAnchore.position.z) / 2f);
        t_MidlePoint.LookAt(t_LeftAnchore.position);
        t_MidlePoint.rotation *= Quaternion.Euler(0, 90, 0);
        /*Vector3 dirToLookAt = (t_LeftAnchore.position-t_MidlePoint.position).normalized;
        t_MidlePoint.rotation = Quaternion.LookRotation(dirToLookAt, Vector3.Cross(dirToLookAt, t_RightAnchore.right ) );
        t_MidlePoint.rotation = Quaternion.Slerp(t_LeftAnchore.rotation, t_RightAnchore.rotation, .5f);*/
        
        
        
        /*if (t_ParentLeft == t_ParentRight)
        {
            if (t_ParentLeft != null && !t_ParentLeft.GetComponent<ShelfSnap>().b_snap)
            {
                if (b_NeedsParent)
                {
                    if (b_realParent)
                    {
                        t_RealParent = t_ParentLeft.transform.parent;
                        b_realParent = false;
                    }
                    t_ParentLeft.GetComponent<Rigidbody>().isKinematic = true;
                    t_ParentLeft.GetComponent<Rigidbody>().useGravity = false;
                    t_ParentLeft.transform.parent = t_MidlePoint;
                }
                else
                {
                    t_ParentLeft.position = new Vector3(t_MidlePoint.position.x, t_ParentLeft.position.y, t_MidlePoint.position.z);
                    t_ParentLeft.localEulerAngles = new Vector3(0, t_MidlePoint.localEulerAngles.y, 0);
                }
            }
        }*/
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

    /*public void SetLeft(Transform t)
    {
        t_ParentLeft = t;
    }

    public void SetRight(Transform t)
    {
        t_ParentRight = t;
    }*/

    /*public void ResetLeft()
    {
        if (b_NeedsParent)
        {
            if (t_ParentLeft != null)
            {
                if (!b_DeactiveObj)
                {
                    t_ParentLeft.transform.parent = t_RealParent;
                    t_ParentLeft.GetComponent<Rigidbody>().isKinematic = false;
                    t_ParentLeft.GetComponent<Rigidbody>().useGravity = true;
                }
            }
                b_realParent = true;
        }
        t_ParentLeft = null;
    }

    public void ResetRight()
    {
        if (b_NeedsParent)
        {
            if (t_ParentRight != null)
            {
                if (!b_DeactiveObj)
                {
                    t_ParentRight.transform.parent = t_RealParent;
                    t_ParentRight.GetComponent<Rigidbody>().isKinematic = false;
                    t_ParentRight.GetComponent<Rigidbody>().useGravity = true;
                }
            }
            b_realParent = true;
        }
        t_ParentRight = null;
    }

    public void SetBoolNeedsParent(bool b_value)
    {
        b_NeedsParent = b_value;
    }
    */
}
