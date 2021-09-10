using DreamHouseStudios.VR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DHS_HI5_Pose_Copy : MonoBehaviour
{
    public bool isLeft;
    public bool copyName = true;
    public bool copyEnabled = true;
    public bool copyLocalScale = true;
    public Transform copyFrom;

    //[ContextMenu("CopyPose")]
    public void CopyPose()
    {
        if (copyFrom == null) return;

        List<Transform> tl = new List<Transform>();
        tl.AddRange(copyFrom.GetComponentsInChildren<Transform>(true));

        List<Transform> bones = new List<Transform>();

        for (int i = 1; i < 5; i++)
            bones.Add(tl.Find(I => I.name == "Human_" + (isLeft ? "Left" : "Right") + "HandThumb" + i.ToString()));

        for (int i = 1; i < 5; i++)
            bones.Add(tl.Find(I => I.name == "Human_" + (isLeft ? "Left" : "Right") + "HandIndex" + i.ToString()));

        for (int i = 1; i < 5; i++)
            bones.Add(tl.Find(I => I.name == "Human_" + (isLeft ? "Left" : "Right") + "HandMiddle" + i.ToString()));

        for (int i = 1; i < 5; i++)
            bones.Add(tl.Find(I => I.name == "Human_" + (isLeft ? "Left" : "Right") + "HandPinky" + i.ToString()));

        for (int i = 1; i < 5; i++)
            bones.Add(tl.Find(I => I.name == "Human_" + (isLeft ? "Left" : "Right") + "HandRing" + i.ToString()));

        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
        {
            Transform tc = bones.Find(b => b.name == t.name);
            if (tc != null)
                t.localRotation = tc.localRotation;
        }

        if (copyName)
            transform.name = copyFrom.transform.name;
        if (copyEnabled)
            gameObject.SetActive(copyFrom.gameObject.activeSelf);
        //if (copyLocalScale)
            transform.localScale = copyFrom.transform.localScale;
            transform.localRotation = copyFrom.transform.localRotation;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DHS_HI5_Pose_Copy))]
public class DHS_HI5_Pose_CopyEditor : Editor
{

    SerializedProperty
        isLeft,
        copyFrom,
        copyName,
        copyEnabled;
    DHS_HI5_Pose_Copy copyTo;

    void OnEnable()
    {
        copyTo = target as DHS_HI5_Pose_Copy;

        isLeft = serializedObject.FindProperty("isLeft");
        copyFrom = serializedObject.FindProperty("copyFrom");
        copyName = serializedObject.FindProperty("copyName");
        copyEnabled = serializedObject.FindProperty("copyEnabled");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(isLeft);
        EditorGUILayout.PropertyField(copyName);
        EditorGUILayout.PropertyField(copyEnabled);
        EditorGUILayout.PropertyField(copyFrom);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Copy pose"))
            CopyPose();

        if (GUILayout.Button("Remove colliders"))
        {
            foreach (Collider c in copyTo.transform.GetComponentsInChildren<Collider>())
                if (Application.isPlaying)
                    Destroy(c);
                else
                    DestroyImmediate(c);
        }

        if (GUILayout.Button("Remove rigidbodies"))
        {
            foreach (Rigidbody rb in copyTo.transform.GetComponentsInChildren<Rigidbody>())
                if (Application.isPlaying)
                    Destroy(rb);
                else
                    DestroyImmediate(rb);
        }

        if (GUILayout.Button("Remove animators"))
        {
            foreach (Animator anim in copyTo.transform.GetComponentsInChildren<Animator>())
                if (Application.isPlaying)
                    Destroy(anim);
                else
                    DestroyImmediate(anim);
        }

        serializedObject.ApplyModifiedProperties();
    }

    void CopyPose()
    {
        if (copyTo == null) return;

        copyTo.CopyPose();
    }

}
#endif