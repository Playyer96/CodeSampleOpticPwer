using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;

namespace DreamHouseStudios.VR
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Interactable))]
    public class InteractableEditor : Editor
    {
        Interactable interactableObject;

        SerializedProperty
            snappable,
            parentable,
            snapTo,
            beingGrabbed,
            updateVelocity,
            onGrab,
            onRelease,
            onGrip,
            onGripRelease,
            onPoint,
            onPointRelease;

        void OnEnable()
        {
            interactableObject = (target as Interactable);

            snappable = serializedObject.FindProperty("snappable");
            parentable = serializedObject.FindProperty("parentable");
            snapTo = serializedObject.FindProperty("snapTo");
            beingGrabbed = serializedObject.FindProperty("beingGrabbed");
            updateVelocity = serializedObject.FindProperty("updateVelocity");
            onGrab = serializedObject.FindProperty("onGrab");
            onRelease = serializedObject.FindProperty("onRelease");
            onGrip = serializedObject.FindProperty("onGrip");
            onGripRelease = serializedObject.FindProperty("onGripRelease");
            onPoint = serializedObject.FindProperty("onPoint");
            onPointRelease = serializedObject.FindProperty("onPointRelease");
            bool hasCollider = interactableObject.GetComponent<BoxCollider>() != null || interactableObject.GetComponent<MeshCollider>() != null || interactableObject.GetComponent<SphereCollider>() != null;

            if (!hasCollider)
            {
                Debug.LogWarning("This GameObject does not contains any collider.");
            }
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(snappable);
            EditorGUILayout.PropertyField(parentable);
            if (snappable.boolValue)
            {
                EditorGUILayout.PropertyField(snapTo);
                snapTo.intValue = Mathf.Max(0, snapTo.intValue);
            }
            GUI.enabled = false;
            EditorGUILayout.PropertyField(beingGrabbed);
            GUI.enabled = true;
            EditorGUILayout.PropertyField(updateVelocity);
            EditorGUILayout.PropertyField(onGrab);
            EditorGUILayout.PropertyField(onRelease);
            EditorGUILayout.PropertyField(onGrip);
            EditorGUILayout.PropertyField(onGripRelease);
            EditorGUILayout.PropertyField(onPoint);
            EditorGUILayout.PropertyField(onPointRelease);
            serializedObject.ApplyModifiedProperties();
        }
    }
}