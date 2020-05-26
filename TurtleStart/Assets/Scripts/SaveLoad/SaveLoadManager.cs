using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour {

	public static string LevelMaps { get { return "Levels/Maps/"; } }
	public static string LevelMaterials { get { return "Materials/LevelMaterials/"; } }
	public static List<string> taboo = new List<string>() { "Disposed", "DisposedBlack", "DisposedClear", "Water" };


	public static string GetMapParams() {//берёт все параметры текущей карты сохранёной в MapConstants.hexGrid
		string errors = FindErrors();

		string temp = "";
		foreach (Transform child in MapConstants.hexGrid) {//все клетки
			if (child.GetComponent<Cell>()) {
				temp += child.position.ToString("F3") + '|'
					+ child.rotation.ToString("F3") + '|'
					+ child.localScale.ToString("F3") + '|'
					+ child.GetComponent<Renderer>().sharedMaterial.name + '\n';
			}
		}
		temp += "options" + '\n';
		temp += MapConstants.height.ToString() + "," + MapConstants.width.ToString() + '\n';//ширина высота карты
		temp += MapConstants.startPosition.ToString() + '\n';//начальная позиция
		temp += MapConstants.endPosition.ToString() + '\n';//конечная позиция
		temp += MapConstants.sizeMainProgram.ToString() + '\n';//стартовый размер главной функции
		if(!errors.Contains("011"))
			temp += String.Join(",", MapConstants.startItems.Select(n => n.name)) + '\n';//стартовые предметы
		else temp += "-" + '\n';
		if (!errors.Contains("100"))
			temp += String.Join(",", MapConstants.startPrograms.Select(n => n.name));//стартовые функции
		else temp += "-" + '\n';
		return temp;
	}


	public static Item LoadItemByName(string _name) {
		return Resources.Load<Item>("Actions/" + _name);
	}
	public static Program LoadProgramByName( string _name ) {
		return Resources.Load<Program>("Prefabs/UI/Programs/" + _name);
	}
	public static BasicSlot LoadSlotByName(string _name) {
		return Resources.Load<BasicSlot>("Prefabs/UI/Slots/" + _name);
	}
	public static TextAsset[] GetMaps() {
		return Resources.LoadAll<TextAsset>(LevelMaps);
	}

	public static void LoadScene( string _sceneName, string _mapName="", string _mapOptions = "" ) {
		if (!string.IsNullOrEmpty(_mapName)) MapConstants.MapName = _mapName;
		if (!string.IsNullOrEmpty(_mapOptions)) MapConstants.MapOptions = _mapOptions;

		SceneManager.LoadScene(_sceneName);
	}

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

	


	private static void Prepare( Transform _parent, Cell _prefab, string _data ) {//загрузка-чтение карты
		if (_data != "") {
			string[] transforms, options;
			string[] parts;

			parts = _data.Split(new string[] { "options" }, StringSplitOptions.None);
			transforms = parts[0].Split('\n');//разделение по строкам
			options = parts[1].Split('\n');

			SetConstants(options);
			CreateGame(_parent, _prefab, transforms);

		}
	}
	private static void SetConstants( string[] _temp ) {

		string[] cellsCoordinates = _temp.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();//удаление не нужных \n

		string[] size = cellsCoordinates[0].Split(',');//размер карты
		string startPos = cellsCoordinates[1];// стартовая позиция
		string endPos = cellsCoordinates[2];// конечная позиция
		string sizeMProg = cellsCoordinates[3];//размер главной функции
		string[] startItems = cellsCoordinates[4].Split(',');//стартовые действия
		string[] startPrograms = cellsCoordinates[5].Split(',');//стартовые программы

		MapConstants.startItems.Clear();
		MapConstants.startPrograms.Clear();


		MapConstants.height = int.Parse(size[0]);
		MapConstants.width = int.Parse(size[1]);

		MapConstants.cells = new Cell[MapConstants.height, MapConstants.width];

		MapConstants.startPosition = HelpFunctions.Vector2FromString(startPos);
		MapConstants.endPosition = HelpFunctions.Vector2FromString(endPos);

		MapConstants.sizeMainProgram = HelpFunctions.Vector2FromString(sizeMProg);
		if (startItems.Length != 0) {
			if (startItems[0] != "-") {
				List<Item> items = new List<Item>();
				for (int i = 0; i < startItems.Length; i++) {
					items.Add(LoadItemByName(startItems[i]));
				}
				MapConstants.startItems = items;
			}
		}
		if (startPrograms.Length != 0) {
			if (startPrograms[0] != "-") {
				List<Program> programs = new List<Program>();

				for (int i = 0; i < startPrograms.Length; i++) {
					programs.Add(LoadProgramByName(startPrograms[i]));
				}
				MapConstants.startPrograms = programs;
			}
		}
		
		
		
	}
	private static void CreateGame( Transform _parent, Cell _prefab, string[] _temp ) {
		#region Map
		HelpFunctions.DestroyChilds(_parent);//удаляем старую карту

		string[] cellsCoordinates = _temp.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();//удаление не нужных \n

		for (int z = 0, i = 0; z < MapConstants.height; z++) {
			for (int x = 0; x < MapConstants.width; x++, i++) {
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
			}
		}
		#endregion
		#region Interface
		ProgrammingContainer container = GameObject.FindObjectOfType<ProgrammingContainer>();
		Transform mainFunction = container.transform;
		HelpFunctions.DestroyChilds(mainFunction);

		GameObject obj = LoadSlotByName("Slot").gameObject;
		for (int i =0; i< MapConstants.sizeMainProgram.y; i++ ) {
			for (int j = 0; j < MapConstants.sizeMainProgram.x; j++) {
				Transform slot = Instantiate(obj).transform;
				slot.SetParent(mainFunction, false);
				slot.localPosition = Vector3.one;
				slot.localScale = Vector3.one;		
			}
		}
		if(MapConstants.startItems.Count!=0)
			container.startItems = MapConstants.startItems;
		if (MapConstants.startPrograms.Count != 0) {
			Transform parent = mainFunction.parent.parent;//куда создавать програмы
			for (int i = 0; i < MapConstants.startPrograms.Count; i++) {
				GameObject p = LoadProgramByName(MapConstants.startPrograms[i].name).gameObject;
				Transform program = Instantiate(p).transform;
				program.SetParent(parent, false);
				program.localPosition = Vector3.one;
				program.localScale = Vector3.one;
			}
		}
		#endregion
	}

	private static string FindErrors() {
		string str = "";
		if(MapConstants.height <= 2 || MapConstants.width <= 2) {
			Debug.LogError("Error Height == " + MapConstants.height + " Widht == " + MapConstants.width);
			str += "|000|";
		}
		if(MapConstants.startPosition == MapConstants.endPosition) {
			Debug.LogError("Error start position == end");
			str += "|001|";
		}
		if(MapConstants.sizeMainProgram.x<=0 || MapConstants.sizeMainProgram.y <= 0) {
			Debug.LogError("Error size main program <= 0");
			str += "|010|";
		}
		if (MapConstants.startItems.Count == 0) {
			Debug.LogWarning("Warnig no start items");
			str += "|011|";
		}
		if(MapConstants.startPrograms.Count == 0) {
			Debug.LogWarning("Warnig no start programs");
			str += "|100|";
		}

		return str;
	}
}
