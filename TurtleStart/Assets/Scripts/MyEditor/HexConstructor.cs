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
}
