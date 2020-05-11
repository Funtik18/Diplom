using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ISlot : IDropHandler{
	void OnDrop(PointerEventData eventData);
}
