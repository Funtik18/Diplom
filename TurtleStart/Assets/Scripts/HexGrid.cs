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
	//public TextMeshProUGUI cellLabelPrefab;

	string level = "Level1";

	void Awake() {
		SaveLoadManager.LoadMapByName(transform, cellPrefab, level);

		Transform[] child = HelpFunctions.TakeAllChilds(transform);
		for(int i = 0; i < child.Length; i++) {
			Cell cell = child[i].GetComponent<Cell>();
			

			if(cell.coordinates.x == MapConstants.startPosition.x && cell.coordinates.z == MapConstants.startPosition.y) {
				Player player = Instantiate<Player>(playerPrefab);
				player.SetPosition(cell);
				player.transform.SetParent(transform, false);
				MapConstants.startCell = cell;
			}
		}
	}
}
