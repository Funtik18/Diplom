using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpFunctions : MonoBehaviour {

	public static List<T> GetCopy<T>( List<T> _original ) {
		List<T> copy = new List<T>();
		for (int i = 0; i < _original.Count; i++) {
			copy.Add(_original[i]);
		}
		return copy;
	}

	public static int[,] Rotate(int[,] m) {
		var result = new int[m.GetLength(1), m.GetLength(0)];

		for(int i = 0; i < m.GetLength(1); i++)
			for(int j = 0; j < m.GetLength(0); j++)
				result[i, j] = m[m.GetLength(0) - j - 1, i];

		return result;
	}

	public static void DestroyChilds(Transform _transform) {
		GameObject []trashArray = new GameObject[_transform.childCount];

		for(int i = 0; i < trashArray.Length; i++) {
			trashArray[i] = _transform.GetChild(i).gameObject;
		}

		for(int i = 0; i < trashArray.Length; i++) {
			DestroyImmediate(trashArray[i]);
		}
	}
	public static void DestroyObject(GameObject _obj) {
		DestroyImmediate(_obj);
	}

	public static Transform[] TakeAllChilds(Transform _transform) {
		Transform[] childs = new Transform[_transform.childCount];
		for(int i = 0; i < _transform.childCount; i++) {
			childs[i] = _transform.GetChild(i);
		}
		return childs;
	}

	public static Vector3 Vector3FromString(string _s) {
		string[] subs = _s.Split('(')[1].Split(')')[0].Split(',');
		for(int i = 0; i < 3; i++) {
			subs[i] = subs[i].Replace(".", ",");
		}
		Vector3 vector3 = new Vector3(
			float.Parse(subs[0]),
			float.Parse(subs[1]),
			float.Parse(subs[2])
		);
		return vector3;
	}
	public static Vector2Int Vector2FromString(string _s) {
		string[] subs = _s.Split('(')[1].Split(')')[0].Split(',');
		for(int i = 0; i < 2; i++) {
			subs[i] = subs[i].Replace(".", ",");
		}
		Vector2Int vector2 = new Vector2Int(
			int.Parse(subs[0]),
			int.Parse(subs[1])
		);
		return vector2;
	}
	public static Quaternion QuaternionFromString(string _s) {
		string[] subs = _s.Split('(')[1].Split(')')[0].Split(',');
		for(int i = 0; i < 4; i++) {
			subs[i] = subs[i].Replace(".", ",");
		}
		Quaternion vector4 = new Quaternion(
			float.Parse(subs[0]),
			float.Parse(subs[1]),
			float.Parse(subs[2]),
			float.Parse(subs[3])
		);
		return vector4;
	}
}


/*static class Ext {
	public static void MoveFromTo<T>(this List<T> list, int i, int j) {//если i < j
		var elem = list[i];
		list.RemoveAt(i);
		list.Insert(j, elem);
	}

	public static void Swap<T>(this List<T> list, int i, int j) {//только при |i-j|=1
		var elem1 = list[i];
		var elem2 = list[j];

		list[i] = elem2;
		list[j] = elem1;
	}
}*/
