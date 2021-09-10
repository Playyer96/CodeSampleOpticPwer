using DreamHouseStudios.WayGroup.Util;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VisibleHandsCreator))]
public class CreateHandsEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		GUILayout.Space(5);

		VisibleHandsCreator myScript = (VisibleHandsCreator)target;
		if (GUILayout.Button("Create Hand"))
			myScript.CreateHand();
	}
}