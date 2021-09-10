using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Trackers/DetectControllersScriptable", order = 1)]
public class DetectControllersScriptable : ScriptableObject
{
    public DetectControllers.HandsHandler lastEnabledHandler = DetectControllers.HandsHandler.NONE;
}