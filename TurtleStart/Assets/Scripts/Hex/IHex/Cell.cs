using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IHex {
	
	public event Action<Cell> OnMouseEnterEvent;
	public event Action<Cell> OnMouseDownEvent;
	public event Action<Cell> OnMouseUpAsButtonEvent;
	public event Action<Cell> OnMouseUpEvent;
	public event Action<Cell> OnMouseExitEvent;

	private void OnMouseEnter() {
		if(OnMouseEnterEvent != null)
			OnMouseEnterEvent(this);
	}
	private void OnMouseDown() {
		if(OnMouseDownEvent != null)
			OnMouseDownEvent(this);
	}
	private void OnMouseUpAsButton() {
		if(OnMouseUpAsButtonEvent != null)
			OnMouseUpAsButtonEvent(this);
	}
	private void OnMouseUp() {
		if(OnMouseUpEvent != null)
			OnMouseUpEvent(this);
	}
	private void OnMouseExit() {
		if(OnMouseExitEvent != null)
			OnMouseExitEvent(this);
	}
	[SerializeField]
	public HexPoint coordinates;
	
	public Material rootMaterial {
		get { return GetComponent<Renderer>().material; }
		set { GetComponent<Renderer>().material = value; }
	}
}
