using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class EditorStuff : MonoBehaviour
{
    public List<GameObject> hide;

    public void Start()
    {
        Enable();
    }

    [ContextMenu("Disable")]
    public void Disable()
    {
        foreach (GameObject go in hide)
            go.SetActive(false);
    }

    [ContextMenu("Enable")]
    public void Enable()
    {
        foreach (GameObject go in hide)
            go.SetActive(true);
    }
}
