using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;



[CustomEditor(typeof(CreateMapEditor))]
public class CreateMapUI : Editor {


	SerializedObject obj;

	SerializedProperty height;
	SerializedProperty width;

	CreateMapEditor myScript;

	private void OnEnable() {
		myScript = (CreateMapEditor)target;

		obj = new SerializedObject(target);

		height = obj.FindProperty("height");
		width = obj.FindProperty("width");
		
	}

	public override void OnInspectorGUI() {
		//DrawDefaultInspector();
		string path = "";
		obj.Update();
		EditorGUI.BeginChangeCheck();

		GUILayout.Label("MapConstants.height: " + MapConstants.height);
		GUILayout.Label("MapConstants.width: " + MapConstants.width);

		EditorGUILayout.PropertyField(height);
		EditorGUILayout.PropertyField(width);

		//если произошло обновление высоты и ширины
		if(MapConstants.height != height.intValue || MapConstants.width != width.intValue) {

			MapConstants.height = height.intValue;
			MapConstants.width = width.intValue;

			HexConstructor.CreateSurface(MapConstants.height, MapConstants.width);
		}
		

		if(EditorGUI.EndChangeCheck()) {
			obj.ApplyModifiedProperties();
			GUI.FocusControl(null);
		}

		if(GUILayout.Button("Save File")) {
			path = SaveDialog("Save File", "txt");
			if(path != "")
				SaveSurface(path);
		}
		if(GUILayout.Button("Open File")) {
			path = OpenDialog("Open File", "*");
			if(path != "")
				LoadSurface(path);
		}

		if(GUILayout.Button("Refresh Map")) {
			HexConstructor.CreateSurface(MapConstants.height, MapConstants.width);
		}

		EditorGUILayout.HelpBox("Save and Open file *.txt for level map", MessageType.Info);
	}


	

	public string OpenDialog(string _title, string _extension, string _directory = "") {
		string pathLoad = EditorUtility.OpenFilePanel(_title, _directory, _extension);
		return pathLoad;
	}
	public string SaveDialog(string _title, string _extension, string _defaultName = "defaultMap", string _directory = "") {
		string pathSave = EditorUtility.SaveFilePanel(_title, _directory, _defaultName, _extension);
		return pathSave;
	}

	void SaveSurface(string _path) {
		SaveLoadManager.SaveMap(_path);
	}
	void LoadSurface(string _path) {
		SaveLoadManager.LoadMapByPath(MapConstants.hexGrid, MapConstants.prefab.GetComponent<Cell>(), _path);

		height.intValue = MapConstants.height;
		width.intValue = MapConstants.width;
	}

	
}
#endif