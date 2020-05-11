using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item", order = 0)]
[System.Serializable]
public class Item : ScriptableObject {
	[SerializeField]
	[Header("Название предмета")]
	public string name;
	[SerializeField]
	public Sprite sprite;
	public TypeItem type = TypeItem.None;
	
	/*[Header("Колличество предметов")]
	[Range(1, 64, order = 0)]
	public int size = 1;
	public bool IsInfinity;*/

}
[CreateAssetMenu(fileName = "NewRefItem", menuName = "RefItem", order = 0)]
[System.Serializable]
public class ReferenceItem : Item {
	//[HideInInspector]
	//public Container container;

	/*[Header("Колличество предметов")]
	[Range(1, 64, order = 0)]
	public int size = 1;
	public bool IsInfinity;*/

}


public enum TypeItem {
	None,
	Direction,
	Reference

}