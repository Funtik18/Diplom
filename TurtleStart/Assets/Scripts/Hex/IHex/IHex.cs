using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IHex {

	event Action<Cell> OnMouseEnterEvent;
	event Action<Cell> OnMouseDownEvent;
	event Action<Cell> OnMouseUpAsButtonEvent;
	event Action<Cell> OnMouseUpEvent;
	event Action<Cell> OnMouseExitEvent;

	Material rootMaterial { get; set; }
}
