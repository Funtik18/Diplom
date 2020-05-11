using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//[RequireComponent(typeof(BasicItem))]
public class BasicSlot : MonoBehaviour, ISlot {

	/*event Action<BasicItem> OnPointerDownEvent;
	event Action<BasicItem> OnPointerClickEvent;
	event Action<BasicItem> OnPointerUpEvent;
	event Action<BasicItem> OnPointerEnterEvent;
	event Action<BasicItem> OnPointerExitEvent;*/
	
	public event Action<BasicSlot, PointerEventData> OnDropEvent;

	public BasicItem currentItem;
	public Image CurrentImageRightHover;
	public Image CurrentImageLeftHover;
	public Image CurrentImageHover;

	private void OnEnable() {
		currentItem = GetComponentInChildren<BasicItem>();
	}

	
	public void OnDrop(PointerEventData eventData) {
		if(OnDropEvent != null)
			OnDropEvent(this, eventData);
	}


	public bool IsEmpty() {
		return currentItem.IsEmpty();
	}


	public void SetActiveRightHover(bool _enable) {
		CurrentImageRightHover.gameObject.SetActive(_enable);
	}
	public void SetActiveLeftHover(bool _enable) {
		CurrentImageLeftHover.gameObject.SetActive(_enable);
	}
	public void SetActiveHoverItem(bool _enable) {
		CurrentImageHover.gameObject.SetActive(_enable);
	}

}
