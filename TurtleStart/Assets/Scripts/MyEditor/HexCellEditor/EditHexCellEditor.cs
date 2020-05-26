#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditHexCellEditor : MonoBehaviour {

	[HideInInspector] public Vector2Int startPosition = Vector2Int.zero;
	[HideInInspector] public Vector2Int endPosition = Vector2Int.zero;

}
#endif