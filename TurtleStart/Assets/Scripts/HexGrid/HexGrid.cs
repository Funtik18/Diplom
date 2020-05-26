using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class HexGrid : MonoBehaviour {

	public Player playerPrefab;
	public Cell cellPrefab;
	public GameObject particle;

	string levelName = "Level2";
	string levelOptions = "";
	void Awake() {
		if (!string.IsNullOrEmpty(MapConstants.MapName)) levelName = MapConstants.MapName;
		if (!string.IsNullOrEmpty(MapConstants.MapOptions)) levelOptions = MapConstants.MapOptions;
		
		SetMap(levelName);

	}
	public void SetMap(string level ) {
		SaveLoadManager.LoadMapByName(transform, cellPrefab, level);

		Transform[] child = HelpFunctions.TakeAllChilds(transform);
		for (int i = 0; i < child.Length; i++) {
			Cell cell = child[i].GetComponent<Cell>();

			

			if (cell.coordinates.x == MapConstants.startPosition.x && cell.coordinates.z == MapConstants.startPosition.y) {//player
				Player player = Instantiate<Player>(playerPrefab);
				player.SetPosition(cell);
				player.transform.SetParent(transform, false);
				MapConstants.startCell = cell;
			}
			if (cell.coordinates.x == MapConstants.endPosition.x && cell.coordinates.z == MapConstants.endPosition.y) {//end
				GameObject obj = Instantiate<GameObject>(particle);
				obj.transform.localPosition = cell.transform.localPosition + new Vector3(0, 0.1f, 0);
				obj.transform.eulerAngles = new Vector3(
					obj.transform.eulerAngles.x - 90,
					obj.transform.eulerAngles.y,
					obj.transform.eulerAngles.z
				);
				obj.transform.SetParent(transform, false);
				MapConstants.endCell = cell;
			}
		}
	}
}
