using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConstants {
	public static int height = 1;
	public static int width = 1;

	public static Cell[,] cells;
	//public static Dictionary<string, Tuple<HexPoint, Cell>> cellsDictionary;

	public static Vector2Int startPosition = Vector2Int.zero;
	public static Cell startCell;

	public static Vector2Int endPosition = Vector2Int.zero;
	public static Cell endCell;

	public static Transform hexGrid { 
		get { 
			return GameObject.FindObjectOfType<HexGrid>().transform;
		}
	}
	public static Transform prefab {
		get {
			return GameObject.FindObjectOfType<HexGrid>().cellPrefab.transform;
		}
	}
}
