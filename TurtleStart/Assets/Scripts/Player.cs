using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{

	public static Player _instance;
	
	HexPoint currentPoint;

	private void Awake() {
		_instance = this;
	}

	public void SetPosition(Cell _newPos) {
		if(_newPos == null) return;
		string temp = _newPos.GetComponent<Renderer>().sharedMaterial.name;

		if (SaveLoadManager.taboo.Contains(temp)) return;


		transform.localPosition = _newPos.transform.localPosition + new Vector3(0, 5, 0);
		currentPoint = _newPos.coordinates;

		CheckWin();//должно происходить если закончился корутин
	}

	public void GoTo(HexPoint _point) {
		
		for(int i = 0; i < MapConstants.height; i++) {
			for(int j = 0; j < MapConstants.width; j++) {
				if(_point == MapConstants.cells[i, j].coordinates) {
					SetPosition(MapConstants.cells[i, j]);
					return;
				}
			}
		}
	}

	public void UpLeft() {
		HexPoint newPoint = new HexPoint(currentPoint.x - 1, currentPoint.z + 1);
		GoTo(newPoint);
	}
	public void DownLeft() {
		HexPoint newPoint = new HexPoint(currentPoint.x, currentPoint.z - 1);
		GoTo(newPoint);
	}
	public void Left() {
		HexPoint newPoint = new HexPoint(currentPoint.x - 1, currentPoint.z );
		GoTo(newPoint);
	}
	public void Right() {
		HexPoint newPoint = new HexPoint(currentPoint.x + 1, currentPoint.z);
		GoTo(newPoint);
	}
	public void UpRight() {
		HexPoint newPoint = new HexPoint(currentPoint.x, currentPoint.z + 1);
		GoTo(newPoint);
	}
	public void DownRight() {
		HexPoint newPoint = new HexPoint(currentPoint.x + 1, currentPoint.z - 1);
		GoTo(newPoint);
	}

	public void RestartPosition() {
		SetPosition(MapConstants.startCell);
	}

	public void CheckWin() {
		if(currentPoint.x == MapConstants.endPosition.x && currentPoint.z == MapConstants.endPosition.y) {
			MainStateButton._instance.State = 2;
			WindowManager._instance.ShowWinWindow();
		}
	}

	

	public bool IsCellExist(HexPoint _point) {
		for(int i = 0; i < MapConstants.height; i++) {
			for(int j = 0; j < MapConstants.width; j++) {
				if(_point == MapConstants.cells[i, j].coordinates) {
					return true;
				}
			}
		}
		return false;
	}
}
