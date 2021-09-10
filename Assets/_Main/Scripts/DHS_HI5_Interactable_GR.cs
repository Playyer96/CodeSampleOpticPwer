using DreamHouseStudios.WayGroup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DHS_HI5_Interactable_GR : InteractiveObject
{
    public UnityEvent OnGrab, OnRelease;

    private Vector3 startPosition, startEulerAnlges;
    private Transform startParent;

    private Rigidbody rb;

    //[SerializeField]
    //private string ResetCollisionTag;
    private bool released;

    //[SerializeField]
    //public List<TriggerRule> triggerRules;



    //IEnumerator Start_()
    //{
    //    yield return new WaitForSeconds(1f);
    //    gameObject.layer = LayerMask.NameToLayer("Default");
    //}
    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        base.Start();
        //StartCoroutine(Start_());
    }

    protected override void GrabObject()
    {
        released = false;
        OnGrab.Invoke();
    }

    protected override void ReleaseObject()
    {
        released = true;
        OnRelease.Invoke();
        GetComponent<Hi5_Interaction_Core.Hi5_Glove_Interaction_Item>().state = Hi5_Interaction_Core.E_Object_State.EStatic;
        GetComponent<Hi5_Interaction_Core.Hi5_Glove_Interaction_Item>().moveType = Hi5_Interaction_Core.Hi5ObjectMoveType.ENone;
    }

    private void OnEnable()
    {
        startPosition = transform.localPosition;
        startEulerAnlges = transform.localEulerAngles;
        startParent = transform.parent;
    }

    private void OnTriggerEnter(Collider collider)
    {
        //if (!released || (ResetCollisionTag.Trim() != string.Empty && collision.collider.tag != ResetCollisionTag.Trim()))
        if (released)
            return;

        //triggerRules.ForEach(tr =>
        //{
        //    if (tr.tag.Trim() == collider.tag.Trim())
        //        tr.OnTriggerEnter.Invoke(collider.transform);
        //});
    }

    private void OnTriggerExit(Collider collider)
    {
        //triggerRules.ForEach(tr =>
        //{
        //    if (tr.tag.Trim() == collider.tag.Trim())
        //        tr.OnTriggerExit.Invoke(collider.transform);
        //});
    }

    public void DisableCollider(Transform t)
    {
        t.GetComponent<Collider>().enabled = false;
    }

    public void ResetObject()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.parent = startParent;
        transform.localPosition = startPosition;
        transform.localEulerAngles = startEulerAnlges;
    }
}
