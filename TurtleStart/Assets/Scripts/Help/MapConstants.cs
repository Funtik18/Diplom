using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConstants {
	#region Map modifications
	public static int height = 1;
	public static int width = 1;
	public static Cell[,] cells;

	public static Vector2Int startPosition = Vector2Int.zero;
	public static Cell startCell;
	public static Vector2Int endPosition = Vector2Int.zero;
	public static Cell endCell;

	public static Transform hexGrid { 
		get { 
			return GameObject.FindObjectOfType<HexGrid>().map;
		}
	}
	public static Transform prefab {
		get {
			return GameObject.FindObjectOfType<HexGrid>().cellPrefab.transform;
		}
	}
	#endregion

	#region Game
	public static string MapName;
	public static string MapOptions;

	public static Vector2Int sizeMainProgram = new Vector2Int(1,5);
	public static List<Item> startItems = new List<Item>();
	public static List<Program> startPrograms = new List<Program>();
	public static int[] blockedIndexes;
	#endregion
}
