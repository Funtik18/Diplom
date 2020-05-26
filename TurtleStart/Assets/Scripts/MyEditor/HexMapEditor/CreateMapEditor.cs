#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateMapEditor : MonoBehaviour{

	[Range(1, 50)]
	[HideInInspector]public int height = 10;
	[Range(1, 50)]
	[HideInInspector]public int width = 10;

}
#endif