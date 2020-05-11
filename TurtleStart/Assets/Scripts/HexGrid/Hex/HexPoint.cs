using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class HexPoint {
	
	public  int x, z;

	public int X { get { return x; } }

	public int Z { get { return z; } }

	public int Y {
		get {
			return -X - Z;
		}
	}

	public HexPoint(int x, int z) {
		this.x = x;
		this.z = z;
	}

	public static HexPoint FromOffsetCoordinates(int x, int z) {
		return new HexPoint(x - (int)( z * 0.5f ), z);
	}

	public override string ToString() {
		return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
	}
	public string ToStringOnSeparateLines() {
		return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
	}


	public static bool operator == (HexPoint point1, HexPoint point2) {
		if(point1.x == point2.x && point1.z == point2.z)
			return true;
		return false;
	}
	public static bool operator !=(HexPoint point1, HexPoint point2) {
		if(point1.x != point2.x || point1.z != point2.z)
			return true;
		return false;
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(HexPoint))]
	public class HexCoordinatesDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			HexPoint coordinates = new HexPoint(
				property.FindPropertyRelative("x").intValue,
				property.FindPropertyRelative("z").intValue
			);
			label = new GUIContent("Coordinates");
			position = EditorGUI.PrefixLabel(position, label);
			GUI.Label(position, coordinates.ToString());
		}
	}
#endif
}
