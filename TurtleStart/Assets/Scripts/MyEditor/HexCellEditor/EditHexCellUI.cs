#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEditor;


[CustomEditor(typeof(EditHexCellEditor))]
public class EditHexCellUI : Editor {
	
	EditHexCellEditor myScript;
	SerializedObject obj;

	SerializedProperty startPos;
	SerializedProperty endPos;

	List<GameObject> selectedObjs;

	Material[] materials;
	Material selectedMaterial;
	string[] materialsList;
	int index = 0;


	bool selectionUpDown = false;
	string selectionUpDownText { get { return selectionUpDown == true ? "UP" : "DOWN"; } }

	bool selectionLeftRight = false;
	string selectionLeftRightText { get { return selectionLeftRight == true ? "LEFT" : "RIGHT"; } }
	int selectionSide4 = 0;

	int borderSize = 0;
	int tempborderSize;

	private void OnEnable() {
		myScript = (EditHexCellEditor)target;

		obj = new SerializedObject(target);

		startPos = obj.FindProperty("startPosition");
		endPos = obj.FindProperty("endPosition");


		materials = Resources.LoadAll<Material>(SaveLoadManager.LevelMaterials);
		materialsList = new string[materials.Length];
		for(int i = 0; i < materials.Length; i++) {
			materialsList[i] = materials[i].name;
		}

	}
	private void RefreshParams( Vector2Int start, Vector2Int end ) {
		startPos.vector2IntValue = start;
		endPos.vector2IntValue = end;
	}
	public override void OnInspectorGUI() {
		selectedObjs = GetSelectedCells();

		Rect rect = EditorGUILayout.GetControlRect();

		GUILayout.BeginVertical("Box");
		EditorGUI.BeginChangeCheck();
		GUILayout.BeginHorizontal();

		RefreshParams(MapConstants.startPosition, MapConstants.endPosition);
		#region Select
		GUILayout.Label("Start Position on " + startPos.vector2IntValue.ToString());
		if(GUILayout.Button("Select")) {
			GameObject[] cells = new GameObject[1];
			cells[0] = MapConstants.startCell.gameObject;
			Selection.objects = cells;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("End Position on " + endPos.vector2IntValue.ToString());
		if(GUILayout.Button("Select")) {
			GameObject[] cells = new GameObject[1];
			cells[0] = MapConstants.endCell.gameObject;
			Selection.objects = cells;
		}
		GUILayout.EndHorizontal();
		
		GUILayout.Label("Selected Cells: " + selectedObjs.Count);
		GUILayout.BeginHorizontal();//select all square left right etc
		int height = 10, width = 10;
		try {
			height = MapConstants.cells.GetLength(0);
			width = MapConstants.cells.GetLength(1);
		}catch(Exception e) {
			Debug.Log(e.Message);
		}
		if (GUILayout.Button("All")) {//ALL
			GameObject[] cells = new GameObject[MapConstants.height * MapConstants.width];
			int g = 0;
			for(int i = 0; i < height; i++) {
				for(int j = 0; j < width; j++) {
					cells[g] = MapConstants.cells[i, j].gameObject;
					g++;
				}
			}
			Selection.objects = cells;
		}
		if(GUILayout.Button(selectionUpDownText)) {//UP
			GameObject[] cells = new GameObject[( MapConstants.height * MapConstants.width )];
			if(selectionUpDown) {
				int g = 0;
				for(int i = 0; i < height / 2; i++) {
					for(int j = 0; j < width; j++) {
						cells[g] = MapConstants.cells[i, j].gameObject;
						g++;
					}
				}
				selectionUpDown = !selectionUpDown;
			} else {//DOWN
				int g = 0;
				for(int i = height / 2; i < height; i++) {
					for(int j = 0; j < width; j++) {
						cells[g] = MapConstants.cells[i, j].gameObject;
						g++;
					}
				}
				selectionUpDown = !selectionUpDown;
			}

			Selection.objects = cells;
		}
		if(GUILayout.Button(selectionLeftRightText)) {
			GameObject[] cells = new GameObject[( MapConstants.height * MapConstants.width )];
			if(selectionLeftRight) {
				int g = 0;
				for(int i = 0; i < height; i++) {
					for(int j = 0; j < width / 2; j++) {
						cells[g] = MapConstants.cells[i, j].gameObject;
						g++;
					}
				}
				selectionLeftRight = !selectionLeftRight;
			} else {
				int g = 0;
				for(int i = 0; i < height; i++) {
					for(int j = width / 2; j < width; j++) {
						cells[g] = MapConstants.cells[i, j].gameObject;
						g++;
					}
				}
				selectionLeftRight = !selectionLeftRight;
			}

			Selection.objects = cells;
		}
		if(GUILayout.Button("square")) {
			GameObject[] cells = new GameObject[( MapConstants.height * MapConstants.width )];
			if(selectionSide4 == 0) {
				int g = 0;
				for(int i = 0; i < height / 2; i++) {
					for(int j = 0; j < width / 2; j++) {
						cells[g] = MapConstants.cells[i, j].gameObject;
						g++;
					}
				}
				selectionSide4 = 1;
			} else if(selectionSide4 == 1) {
				int g = 0;
				for(int i = 0; i < height / 2; i++) {
					for(int j = width / 2; j < width; j++) {
						cells[g] = MapConstants.cells[i, j].gameObject;
						g++;
					}
				}
				selectionSide4 = 2;
			} else if(selectionSide4 == 2) {
				int g = 0;
				for(int i = height / 2; i < height; i++) {
					for(int j = width / 2; j < width; j++) {
						cells[g] = MapConstants.cells[i, j].gameObject;
						g++;
					}
				}
				selectionSide4 = 3;
			} else if(selectionSide4 == 3) {
				int g = 0;
				for(int i = height / 2; i < height; i++) {
					for(int j = 0; j < width / 2; j++) {
						cells[g] = MapConstants.cells[i, j].gameObject;
						g++;
					}
				}
				selectionSide4 = 0;
			}
			Selection.objects = cells;
		}
		GUILayout.EndHorizontal();
		EditorGUILayout.LabelField(new GUIContent("Border:"));//select border
		GUILayout.BeginHorizontal();

		tempborderSize = borderSize;
		
		borderSize = EditorGUILayout.IntSlider(borderSize, 1, width);
		GUILayout.BeginVertical();
		if(GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(10))){
			if(borderSize<width)
				borderSize++;
		}
		if(GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(10))){
			if(borderSize>0)
				borderSize--;
		}
		GUILayout.EndVertical();
		try {
			if (tempborderSize != borderSize) {
				GameObject[] cells = new GameObject[( MapConstants.height * MapConstants.width )];
				int g = 0;
				for (int i = 0; i < height; i++) {
					for (int j = 0; j < width; j++) {
						if (i == borderSize - 1 || i == height - borderSize || j == borderSize - 1 || j == width - borderSize) {
							cells[g] = MapConstants.cells[i, j].gameObject;
							g++;
						}
					}
				}
				Selection.objects = cells;
			}
		}catch(Exception e) {
			Debug.Log(e.Message);
		}
		GUILayout.EndHorizontal();

		GUILayout.Space(20f);
		#endregion
		#region Edit
		GUILayout.BeginHorizontal();

		GUILayout.Label("Y: ");
		if(GUILayout.Button("+1") && selectedObjs.Count != 0) {
			for(int i = 0; i < selectedObjs.Count; i++) {
				Vector3 oldpos = selectedObjs[i].transform.position;
				selectedObjs[i].transform.position = new Vector3(oldpos.x, oldpos.y + 10, oldpos.z);
			}
		}
		if(GUILayout.Button("-1") && selectedObjs.Count != 0) {
			for(int i = 0; i < selectedObjs.Count; i++) {
				Vector3 oldpos = selectedObjs[i].transform.position;
				selectedObjs[i].transform.position = new Vector3(oldpos.x, oldpos.y - 10, oldpos.z);
			}
		}
		if(GUILayout.Button("0") && selectedObjs.Count != 0) {
			for(int i = 0; i < selectedObjs.Count; i++) {
				Vector3 oldpos = selectedObjs[i].transform.position;
				selectedObjs[i].transform.position = new Vector3(oldpos.x, 0, oldpos.z);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Y solid: ");
		if(GUILayout.Button("+1") && selectedObjs.Count != 0) {
			for(int i = 0; i < selectedObjs.Count; i++) {
				Vector3 oldscale = selectedObjs[i].transform.localScale;
				Vector3 oldpos = selectedObjs[i].transform.position;
				selectedObjs[i].transform.localScale = new Vector3(oldscale.x, oldscale.y, oldscale.z + 10);
				selectedObjs[i].transform.position = new Vector3(oldpos.x, oldpos.y + 5, oldpos.z);
			}
		}
		if(GUILayout.Button("-1") && selectedObjs.Count != 0) {
			for(int i = 0; i < selectedObjs.Count; i++) {
				Vector3 oldscale = selectedObjs[i].transform.localScale;
				Vector3 oldpos = selectedObjs[i].transform.position;
				if(oldscale.z == 10) oldscale.z -= 20;
				selectedObjs[i].transform.localScale = new Vector3(oldscale.x, oldscale.y, oldscale.z - 10);
				selectedObjs[i].transform.position = new Vector3(oldpos.x, oldpos.y - 5, oldpos.z);
			}
		}
		if(GUILayout.Button("0") && selectedObjs.Count != 0) {
			for(int i = 0; i < selectedObjs.Count; i++) {
				Vector3 oldscale = selectedObjs[i].transform.localScale;
				Vector3 oldpos = selectedObjs[i].transform.position;
				selectedObjs[i].transform.localScale = new Vector3(oldscale.x, 1, 1);
				selectedObjs[i].transform.position = new Vector3(oldpos.x, 0, oldpos.z);
			}
		}
		GUILayout.EndHorizontal();
		if(GUILayout.Button("Road")) {
			for (int i = 0; i < selectedObjs.Count; i++) {
				Vector3 oldpos = selectedObjs[i].transform.position;
				selectedObjs[i].transform.position = new Vector3(oldpos.x, -1, oldpos.z);
			}
			/*for(int i = 0; i < selectedObjs.Count; i++) {//down
				Vector3 oldpos = selectedObjs[i].transform.position;
				selectedObjs[i].transform.position = new Vector3(oldpos.x, oldpos.y - 10, oldpos.z);
			}
			for(int i = 0; i < selectedObjs.Count; i++) {//down s
				Vector3 oldscale = selectedObjs[i].transform.localScale;
				Vector3 oldpos = selectedObjs[i].transform.position;
				if(oldscale.z == 10) oldscale.z -= 20;
				selectedObjs[i].transform.localScale = new Vector3(oldscale.x, oldscale.y, oldscale.z - 10);
				selectedObjs[i].transform.position = new Vector3(oldpos.x, oldpos.y - 5, oldpos.z);
			}
			for(int i = 0; i < selectedObjs.Count; i++) {//up s
				Vector3 oldscale = selectedObjs[i].transform.localScale;
				Vector3 oldpos = selectedObjs[i].transform.position;
				selectedObjs[i].transform.localScale = new Vector3(oldscale.x, oldscale.y, oldscale.z + 10);
				selectedObjs[i].transform.position = new Vector3(oldpos.x, oldpos.y + 5, oldpos.z);
			}
			for(int i = 0; i < selectedObjs.Count; i++) {//up s
				Vector3 oldscale = selectedObjs[i].transform.localScale;
				Vector3 oldpos = selectedObjs[i].transform.position;
				selectedObjs[i].transform.localScale = new Vector3(oldscale.x, oldscale.y, oldscale.z + 10);
				selectedObjs[i].transform.position = new Vector3(oldpos.x, oldpos.y + 5, oldpos.z);
			}*/
		}
		GUILayout.BeginHorizontal();
		GUILayout.Label("X: ");
		if(GUILayout.Button("+1") && selectedObjs.Count != 0) {
			for(int i = 0; i < selectedObjs.Count; i++) {
				Vector3 oldpos = selectedObjs[i].transform.position;
				Vector3 position;
				position.x = ( 1 + ( 0 * 0.5f ) - (int)( 0 * 0.5f ) ) * ( HexMetrics.innerRadius * 2f );
				position.y = 0f;
				position.z = 0 * ( HexMetrics.outerRadius * 1.5f );

				selectedObjs[i].transform.position = oldpos + position;
			}
		}
		if(GUILayout.Button("-1") && selectedObjs.Count != 0) {
			for(int i = 0; i < selectedObjs.Count; i++) {
				Vector3 oldpos = selectedObjs[i].transform.position;
				Vector3 position;
				position.x = ( 1 + ( 0 * 0.5f ) - (int)( 0 * 0.5f ) ) * ( HexMetrics.innerRadius * 2f );
				position.y = 0f;
				position.z = 0 * ( HexMetrics.outerRadius * 1.5f );
				selectedObjs[i].transform.position = oldpos - position;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Z: ");
		if(GUILayout.Button("+1") && selectedObjs.Count != 0) {
			for(int i = 0; i < selectedObjs.Count; i++) {
				Vector3 oldpos = selectedObjs[i].transform.position;
				Vector3 position;
				position.x = ( 0 + ( 1 * 0.5f ) - (int)( 1 * 0.5f ) ) * ( HexMetrics.innerRadius * 2f );
				position.y = 0f;
				position.z = 1 * ( HexMetrics.outerRadius * 1.5f );

				selectedObjs[i].transform.position = oldpos + position;
			}
		}
		if(GUILayout.Button("-1") && selectedObjs.Count != 0) {
			for(int i = 0; i < selectedObjs.Count; i++) {
				Vector3 oldpos = selectedObjs[i].transform.position;
				Vector3 position;
				position.x = ( 0 + ( 1 * 0.5f ) - (int)( 1 * 0.5f ) ) * ( HexMetrics.innerRadius * 2f );
				position.y = 0f;
				position.z = 1 * ( HexMetrics.outerRadius * 1.5f );

				selectedObjs[i].transform.position = oldpos - position;
			}
		}

		GUILayout.EndHorizontal();

		#endregion
		#region Set Material
		GUILayout.BeginHorizontal();

		if(materialsList != null) {
			index = EditorGUILayout.Popup("Select Material", index, materialsList);
		}
		if(GUILayout.Button("Take", GUILayout.Width(40))) {
			GameObject[] cells = new GameObject[MapConstants.height * MapConstants.width];
			int g = 0;
			for(int i = 0; i < height; i++) {
				for(int j = 0; j < width; j++) {
					if(MapConstants.cells[i, j].GetComponent<Renderer>().sharedMaterial == selectedMaterial) {
						cells[g] = MapConstants.cells[i, j].gameObject;
						g++;
					}
				}
			}
			Selection.objects = cells;
		}
		if(index < materials.Length) {
			selectedMaterial = materials[index];

			rect = EditorGUILayout.GetControlRect();
			rect = new Rect(rect.x, rect.y - 1, 20, 20);

			if(selectedMaterial.mainTexture == null) {
				//EditorGUI.DrawRect(rect, selectedMaterial.GetColor("_BaseColor"));
			} else {
				EditorGUI.DrawPreviewTexture(rect, selectedMaterial.mainTexture, selectedMaterial, ScaleMode.ScaleToFit);
			}
		} else {
			selectedMaterial = null;
		}

		GUILayout.EndHorizontal();

		if(GUILayout.Button("Set Material")) {
			ChangeMaterial();
		}
		#endregion
		//set start pos set end pos
		if(selectedObjs.Count == 1) {
			Cell cell = selectedObjs[0].GetComponent<Cell>();
			if(cell.coordinates.x != MapConstants.startPosition.x || cell.coordinates.z != MapConstants.startPosition.y) {
				if(GUILayout.Button("SetStartPosition")) {
					MapConstants.startCell = cell;
					MapConstants.startPosition = new Vector2Int(cell.coordinates.x, cell.coordinates.z);
				}
			}
			if(cell.coordinates.x != MapConstants.endPosition.x || cell.coordinates.z != MapConstants.endPosition.y) {
				if(GUILayout.Button("SetEndPosition")) {
					MapConstants.endCell = cell;
					MapConstants.endPosition = new Vector2Int(cell.coordinates.x, cell.coordinates.z);
				}
			}
		}
		if(EditorGUI.EndChangeCheck()) {
			obj.ApplyModifiedProperties();
			GUI.FocusControl(null);
		}
		GUILayout.EndVertical();
	}

	private List<GameObject> GetSelectedCells() {
		List<GameObject> selectedObjs = new List<GameObject>();
		try {
			foreach(GameObject obj in Selection.objects) {
				if(obj.GetComponent<Cell>()) {
					selectedObjs.Add(obj);
				}
			}
		} catch(Exception e) {

		}
		return selectedObjs;
	}
	private void ChangeMaterial() {
		if(selectedMaterial == null) return;
		for(int i = 0; i < selectedObjs.Count; i++) {
			selectedObjs[i].GetComponent<Renderer>().sharedMaterial = selectedMaterial;
		}
	}
}
#endif