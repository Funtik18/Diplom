#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(ItemsEditor))]
public class ItemsUI : Editor {
    SerializedObject obj;

    ItemsEditor myScript;

	SerializedProperty items;
	SerializedProperty sizeMainProgram;
	SerializedProperty programs;
	private void OnEnable() {
		myScript = (ItemsEditor)target;

		obj = new SerializedObject(target);

		items = obj.FindProperty("items");
		sizeMainProgram = obj.FindProperty("sizeMainProgram");
		programs = obj.FindProperty("programs");
	}
	SerializedProperty temp;
	public override void OnInspectorGUI() {

		obj.Update();

		EditorGUILayout.PropertyField(sizeMainProgram);
		EditorGUILayout.PropertyField(programs);
		EditorGUILayout.PropertyField(items);
		
		obj.ApplyModifiedProperties();

		if (GUILayout.Button("Save")) {
			List<Item> temp = myScript.items;
			MapConstants.startItems.Clear();
			for (int i = 0; i < temp.Count; i++) {
				if (temp[i] != null) {
					MapConstants.startItems.Add(temp[i]);
				}
			}

			List<Program> temp2 = myScript.programs;
			MapConstants.startPrograms.Clear();
			for (int i = 0; i < temp2.Count; i++) {
				if (temp2[i] != null) {
					MapConstants.startPrograms.Add(temp2[i].name);
				}
			}

			MapConstants.sizeMainProgram = myScript.sizeMainProgram;
		}
		if (MapConstants.sizeMainProgram.x != 0) {
			GUILayout.Label("Main program size:" + MapConstants.sizeMainProgram.ToString());
		}
		
		if (MapConstants.startItems.Count != 0) {
			GUILayout.Label("Inventory contains:");
			GUILayout.Label("In main program:");
			for (int i = 0; i < MapConstants.startItems.Count; i++) {
				GUILayout.Label(MapConstants.startItems[i].name);
			}
			GUILayout.Space(10f);
		}
		if (MapConstants.startPrograms.Count != 0) {
			GUILayout.Label("Count functions:");
			for (int i = 0; i < MapConstants.startPrograms.Count; i++) {
				GUILayout.Label("Func " + i.ToString());
			}
			
		}
		/*
		if (MapConstants.blockedIndexes.Length != 0) {
			for (int i = 0; i < MapConstants.blockedIndexes.Length; i++) {
				GUILayout.Label(MapConstants.blockedIndexes[i].ToString());
			}
		}*/
	}
}
#endif