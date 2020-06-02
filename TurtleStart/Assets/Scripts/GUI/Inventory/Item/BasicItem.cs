using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class BasicItem : MonoBehaviour, IPointerEvents, IDragEvents {
	
	protected Item item;
	public virtual Item Item {
		get {
			return this.item;
		}
		set {
			this.item = value;
			if(this.item == null) return;
			ChangeAlpha(1);
			this.CurrentImage.sprite = this.item.sprite;
		}
	}

	public virtual Image CurrentImage {
		get {
			return GetComponent<Image>();
		}
	}

	/// <summary>
	/// Сheck the item for emptiness
	/// </summary>
	/// <returns>True if is empty</returns>
	public virtual bool IsEmpty() {
		if(this.CurrentImage.color.a == 0) {
			Dispose();
			return true;
		}
		return false;
	}
	public virtual void Dispose() {
		this.item = null;
		ChangeAlpha(0);
	}
	void ChangeAlpha(float _a) {
		Color c = this.CurrentImage.color;
		c.a = _a;
		this.CurrentImage.color = c;
	}
	
	[HideInInspector]
	public CanvasGroup canvasGroup;
	void OnEnable() {
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public event Action<BasicItem, PointerEventData> OnPointerDownEvent;
	public event Action<BasicItem, PointerEventData> OnPointerClickEvent;
	public event Action<BasicItem, PointerEventData> OnPointerUpEvent;
	public event Action<BasicItem, PointerEventData> OnPointerEnterEvent;
	public event Action<BasicItem, PointerEventData> OnPointerExitEvent;

	public event Action<BasicItem, PointerEventData> OnBeginDragEvent;
	public event Action<BasicItem, PointerEventData> OnDragEvent;
	public event Action<BasicItem, PointerEventData> OnEndDragEvent;

	public void OnPointerDown(PointerEventData eventData) {
		if(OnPointerDownEvent != null)
			OnPointerDownEvent(this, eventData);
	}
	public void OnPointerClick(PointerEventData eventData) {
		if(OnPointerClickEvent != null)
			OnPointerClickEvent(this, eventData);
	}
	public void OnPointerUp(PointerEventData eventData) {
		if(OnPointerUpEvent != null)
			OnPointerUpEvent(this, eventData);
	}
	public void OnPointerEnter(PointerEventData eventData) {
		if(OnPointerEnterEvent != null)
			OnPointerEnterEvent(this, eventData);
	}
	public void OnPointerExit(PointerEventData eventData) {
		if(OnPointerExitEvent != null)
			OnPointerExitEvent(this, eventData);
	}

	public void OnBeginDrag(PointerEventData eventData) {
		if(OnBeginDragEvent != null)
			OnBeginDragEvent(this, eventData);
	}
	public void OnDrag(PointerEventData eventData) {
		if(OnDragEvent != null)
			OnDragEvent(this, eventData);
	}
	public void OnEndDrag(PointerEventData eventData) {
		if(OnEndDragEvent != null)
			OnEndDragEvent(this, eventData);
	}
}
