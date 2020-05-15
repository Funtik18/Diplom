using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOverseer : MonoBehaviour{

	public static InventoryOverseer _instance;
	[HideInInspector]
	public List<Container> containers = new List<Container>();

	[Tooltip("Откуда брать, куда ложить")] public Container take, put; // от куда брать и куда ложить


	[HideInInspector]
	public Container from, to;// юзер перетаскивает из, в

	private void OnEnable() {
		_instance = this;
	}


	public bool Verify() {
		return from == to ? true : false;
	}

	public bool IsContains(Container _container) {
		return containers.Contains(_container);
	}

	public bool VerumPut(Container _container) {//если контэйнер который надо ложить
		if(_container == put) return true;
		return false;
	}
	public bool VerumTake(Container _container) {//если контэйнер который надо ложить
		if(_container == take) return true;
		return false;
	}
	public bool VerumTo(Container _container) {//если контэйнер который я хочу положить
		if(_container == to) return true;
		return false;
	}
	public bool VerumFrom(Container _container) {//если контэйнер который я хочу положить
		if(_container == from) return true;
		return false;
	}

	public void Dispose() {
		from = null;
		to = null;
	}
	public void DisposeContainers() {
		foreach(var container in containers) {
			if(container == take) continue;
			container.DisposeAll();
		}
	}
}
