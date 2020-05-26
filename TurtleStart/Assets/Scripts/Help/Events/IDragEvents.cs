using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IDragEvents : IBeginDragHandler, IDragHandler, IEndDragHandler {
	/*event Action<BasicItem, PointerEventData> OnBeginDragEvent;
	event Action<BasicItem, PointerEventData> OnDragEvent;
	event Action<BasicItem, PointerEventData> OnEndDragEvent;

	new void OnBeginDrag(PointerEventData eventData);
	new void OnDrag(PointerEventData eventData);
	new void OnEndDrag(PointerEventData eventData);
*/}
