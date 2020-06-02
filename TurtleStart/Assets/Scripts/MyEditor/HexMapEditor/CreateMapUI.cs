#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
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

	private void RefreshParams(int h,int w) {
		height.intValue = h;
		width.intValue = w;
	}
	private void RefreshConstatnts() {
		//если произошло обновление высоты и ширины
		if (MapConstants.height != height.intValue || MapConstants.width != width.intValue) {
			//HexConstructor.IncreaseSurface(height.intValue, width.intValue);
			MapConstants.height = height.intValue;
			MapConstants.width = width.intValue;

			
			HexConstructor.CreateSurface(height.intValue, width.intValue);
		}
	}

	public override void OnInspectorGUI() {
		//DrawDefaultInspector();
		string path = "";
		obj.Update();
		EditorGUI.BeginChangeCheck();

		RefreshParams(MapConstants.height, MapConstants.width);

		GUILayout.Label("MapConstants.height: " + height.intValue);
		GUILayout.Label("MapConstants.width: " + width.intValue);

		EditorGUILayout.PropertyField(height);
		EditorGUILayout.PropertyField(width);

		RefreshConstatnts();
		

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
			RefreshParams(10, 10);
			HexConstructor.CreateSurface(height.intValue, width.intValue);
			RefreshConstatnts();
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

		RefreshParams(MapConstants.height, MapConstants.width);
	}
}
#endif