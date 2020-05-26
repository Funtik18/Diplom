#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsEditor : MonoBehaviour {
	[SerializeField]
	[Tooltip("Стартовые предметы на уровне.")]
	public List<Item> items = new List<Item>();
	[SerializeField]
	[Tooltip("Стартовый размер главной программы.")]
	public Vector2Int sizeMainProgram = new Vector2Int(1, 5);
	[SerializeField]
	[Tooltip("Стартовое количество функций.")]
	public List<Program> programs = new List<Program>();
}
#endif