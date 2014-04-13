using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ListTester))]
public class ListTesterInspector : Editor {
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorList.Show(serializedObject.FindProperty("integers"), true, false);
		EditorList.Show(serializedObject.FindProperty("vectors"));
		EditorList.Show(serializedObject.FindProperty("colorPoints"), false, false);
		EditorList.Show(serializedObject.FindProperty("objects"), false);
		serializedObject.ApplyModifiedProperties();
	}
}
