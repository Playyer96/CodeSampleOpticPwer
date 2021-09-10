using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Hi5_Interaction_Core;

[CustomEditor(typeof(DHS_Hi5_Interactable))]
[CanEditMultipleObjects]
public class DHS_Hi5_InteractableEditor : Editor
{
    private void OnEnable()
    {
        DHS_Hi5_Interactable item = (target as DHS_Hi5_Interactable);
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        bool hasCollider = item.GetComponent<BoxCollider>() != null || item.GetComponent<MeshCollider>() != null || item.GetComponent<SphereCollider>() != null;

        if (!hasCollider)
        {
            Debug.LogWarning("This GameObject does not contains any collider.");
        }

        Hi5_Object_Property hop = item.GetComponent<Hi5_Object_Property>();
        if (hop != null)
        {
            hop.IsLift = false;
            hop.IsClap = false;
        }
    }
}