using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexConstructor : MonoBehaviour{
	public static void CreateSurface(int _height, int _width) {
		HelpFunctions.DestroyChilds(MapConstants.hexGrid);
		MapConstants.cells = new Cell[_height, _width];
		for(int z = 0; z < _height; z++) {
			for(int x = 0; x < _width; x++) {
				CreateCell(z, x);
			}
		}
	}
	/*		Cell[,] old = MapConstants.cells;
		int height = MapConstants.height + _height;
		int widht = MapConstants.width + _width];
		MapConstants.cells = new Cell[height, widht];
		for (int z = MapConstants.height; z < height; z++) {
			for (int x = MapConstants.width; x < widht; x++) {
				CreateCell(z, x);
			}
		}*/
	public static void IncreaseSurface(int _height, int _width ) {
		
		int increaseOn = _height - MapConstants.height;
		int oldheight = MapConstants.cells.GetLength(0);
		HelpFunctions.DestroyChilds(MapConstants.hexGrid);
		MapConstants.cells = new Cell[_height, _width];
		for (int z = 0; z < _height; z++) {
			for (int x = 0; x < _width; x++) {
				CreateCell(z, x);
			}
		}
		Debug.Log(increaseOn + "\t" + oldheight);

	
	}


	public static void CreateCell(int _z, int _x) {
		//position
		Vector3 position;
		position.x = ( _x + ( _z * 0.5f ) - (int)( _z * 0.5f ) ) * ( HexMetrics.innerRadius * 2f );
		position.y = 0f;
		position.z = _z * ( HexMetrics.outerRadius * 1.5f );

		Cell cell = MapConstants.cells[_z, _x] = Instantiate<Cell>(MapConstants.prefab.GetComponent<Cell>());
		cell.coordinates = HexPoint.FromOffsetCoordinates(_x, _z);


		cell.transform.SetParent(MapConstants.hexGrid, false);
		cell.transform.localPosition = position;


		//text
		//TextMeshProUGUI label = Instantiate<TextMeshProUGUI>(cellLabelPrefab);
		//label.rectTransform.SetParent(gridCanvas.transform, false);
		//label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);

		//label.text = cell.coordinates.ToStringOnSeparateLines();

	}


	static void ResizeArray<T>( ref T[,] original, int newCoNum, int newRoNum ) {
		var newArray = new T[newCoNum, newRoNum];
		int columnCount = original.GetLength(1);
		int columnCount2 = newRoNum;
		int columns = original.GetUpperBound(0);
		for (int co = 0; co <= columns; co++)
			Array.Copy(original, co * columnCount, newArray, co * columnCount2, columnCount);
		original = newArray;
	}
}
