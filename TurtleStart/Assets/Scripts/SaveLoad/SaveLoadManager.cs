using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour {

	public static string LevelMaterials { get { return "Materials/LevelMaterials/"; } }


	//public static Item LoadItemByName(string _name) {
		//return Resources.Load<Item>("Prefabs/Actions/" + _name);
	//}



	public static void LoadMapByName(Transform _parent, Cell _prefab,string _name = "defaultMap") {
		string map = Resources.Load<TextAsset>("Levels/Maps/" + _name).text;
		if(map != "") {
			Prepare(_parent, _prefab, map);
		}
	}
	public static void LoadMapByPath(Transform _parent, Cell _prefab, string _path) {
		if(_path != "") {
			using(StreamReader sr = new StreamReader(_path)) {
				string data = sr.ReadToEnd();
				Prepare(_parent, _prefab, data);
			}
		}
	}

	public static void SaveMap(string _path) {
		using(StreamWriter sw = new StreamWriter(_path, false, System.Text.Encoding.Default)) {
			string temp = SaveLoadManager.GetMapParams();
			sw.WriteLine(temp);
		}
	}
	

	private static void Prepare(Transform _parent, Cell _prefab, string _data) {
		if(_data != ""){
			string[] transforms, options;
			string[] parts;

			parts = _data.Split(new string[] { "options" }, StringSplitOptions.None);
			transforms = parts[0].Split('\n');//разделение по строкам
			options = parts[1].Split('\n');

			SetConstants(options);
			CreateMap(_parent, _prefab, transforms);
			
		}
	}

	private static void CreateMap(Transform _parent, Cell _prefab, string[] _temp) {
		HelpFunctions.DestroyChilds(_parent);//удаляем старую карту

		string[] cellsCoordinates = _temp.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();//удаление не нужных \n

		for(int z = 0, i = 0; z < MapConstants.height; z++) {
			for(int x = 0; x < MapConstants.width; x++, i++) {
				string[] param = cellsCoordinates[i].Split('|');//отбираем позицию|поворот|размер|и материал

				Vector3 position = Vector3.zero;
				Quaternion rotation = new Quaternion();
				Vector3 scale = Vector3.zero;


				position = HelpFunctions.Vector3FromString(param[0]);
				rotation = HelpFunctions.QuaternionFromString(param[1]);
				scale = HelpFunctions.Vector3FromString(param[2]);
				Material material = Resources.Load(LevelMaterials + param[3], typeof(Material)) as Material;

				//Instantiate
				Cell cell = Instantiate<Cell>(_prefab);//создание
				
				cell.coordinates = HexPoint.FromOffsetCoordinates(x, z);

				cell.transform.SetParent(_parent, false);
				cell.transform.localPosition = position;
				cell.transform.localRotation = rotation;
				cell.transform.localScale = scale;
				cell.rootMaterial = material;

				MapConstants.cells[z, x] = cell;
				//MapConstants.cellsDictionary.Add(cell.ToString(), new Tuple<HexPoint, Cell>(cell.coordinates, cell));
			}
		}
	}
	private static void SetConstants(string[] _temp) {

		string[] cellsCoordinates = _temp.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();//удаление не нужных \n

		string[] size = cellsCoordinates[0].Split(',');//размер карты
		string startPos = cellsCoordinates[1];// стартовая позиция
		string endPos = cellsCoordinates[2];// конечная позиция

		MapConstants.height = int.Parse(size[0]);
		MapConstants.width = int.Parse(size[1]);

		MapConstants.cells = new Cell[MapConstants.height, MapConstants.width];
		
		MapConstants.startPosition = HelpFunctions.Vector2FromString(startPos);
		MapConstants.endPosition = HelpFunctions.Vector2FromString(endPos);
	}



	public static string GetMapParams() {
		string temp = "";
		foreach(Transform child in MapConstants.hexGrid) {
			if(child.GetComponent<Cell>()) {
				temp += child.position.ToString("F3") + '|'
					+ child.rotation.ToString("F3") + '|'
					+ child.localScale.ToString("F3") + '|'
					+ child.GetComponent<Renderer>().sharedMaterial.name + '\n';
			}
		}
		temp += "options" + '\n';
		temp += MapConstants.height.ToString() + "," + MapConstants.width.ToString() + '\n';
		temp += MapConstants.startPosition.ToString() + '\n';
		temp += MapConstants.endPosition.ToString();

		return temp;
	}

}
