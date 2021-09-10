using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CanvasManager))]
public class CanvasManagerEditor : Editor
{
    CanvasManager cm;

    private void OnEnable()
    {
        cm = target as CanvasManager;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Start"))
        {
            cm.indexStep = 0;
            cm.StepToStep(cm.indexStep);
        }
        if (GUILayout.Button("Next"))
        {
            cm.indexStep++;
            cm.StepToStep(cm.indexStep);
        }
        if (GUILayout.Button("Back"))
        {
            cm.indexStep--;
            cm.StepToStep(cm.indexStep);
        }
    }

    private void OnSceneGUI()
    {
        


        for (int i = 0; i < cm.group.Length; i++)
        {
            for (int j = 0; j < cm.group[i].pos.Length; j++)
            {
                if (i == 0)
                {
                    Handles.Label(cm.group[i].pos[j], "Spawn Point PopUp: " + j.ToString(), new GUIStyle { normal = new GUIStyleState() { textColor = Color.white }, alignment = TextAnchor.MiddleCenter, fontSize = 10 });
                }
                else
                {
                    Handles.Label(cm.group[i].pos[j], "Spawn Point Icono: " + j.ToString(), new GUIStyle { normal = new GUIStyleState() { textColor = Color.cyan }, alignment = TextAnchor.MiddleCenter, fontSize = 10 });
                }
            }
        }

        for (int i = 0; i < cm.group.Length; i++)
        {
            for (int j = 0; j < cm.group[i].pos.Length; j++)
            {
                cm.group[i].pos[j] = Handles.DoPositionHandle(cm.group[i].pos[j], Quaternion.identity);
            }
        }
    }
}
