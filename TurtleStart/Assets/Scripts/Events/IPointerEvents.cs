using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IPointerEvents : IPointerDownHandler, IPointerClickHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {
	/*event Action<BasicItem, PointerEventData> OnPointerDownEvent;
	event Action<BasicItem, PointerEventData> OnPointerClickEvent;
	event Action<BasicItem, PointerEventData> OnPointerUpEvent;
	event Action<BasicItem, PointerEventData> OnPointerEnterEvent;
	event Action<BasicItem, PointerEventData> OnPointerExitEvent;

	new void OnPointerDown(PointerEventData eventData);
	new void OnPointerClick(PointerEventData eventData);
	new void OnPointerUp(PointerEventData eventData);
	new void OnPointerEnter(PointerEventData eventData);
	new void OnPointerExit(PointerEventData eventData);
	*/
}
