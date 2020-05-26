using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AlphaItem))]
public class ReferenceContainer : Container{

	AlphaItem reference;

	protected override void Start() {
		InventoryOverseer._instance.refContainers.Add(this);
		Prepare();
	}

	public void Prepare() {
		reference = GetComponent<AlphaItem>();

		ReferenceItem refItem = ScriptableObject.CreateInstance(typeof(ReferenceItem)) as ReferenceItem;


		refItem.type = TypeItem.Reference;
		refItem.sprite = Resources.Load<Sprite>("Textures/GUI/Directions/None");
		refItem.container = transform.parent.GetChild(1).GetComponent<Container>();
		refItem.name = "ref " + refItem.container;
		reference.Item = refItem;

		SubscribeItemEvents(reference);
		FindBuffer();
	}

	public override void OnBeginDrag(PointerEventData eventData) {
		iCanDrag = true;

		if(!iCanDrag) return;
		isDrag = true;

		bufferItem.transform.position = eventData.pointerCurrentRaycast.worldPosition;
		bufferItem.Item = reference.Item;
	}
	protected override void EndDrag(BasicItem _item, PointerEventData _eventData) {
		if(!iCanDrag) return;

		bufferItem.Dispose();//освобождаем буффер

		isDrag = false;
	}
	protected override void onBeginDrag(BasicItem _item, PointerEventData _eventData) { }
	
	protected override void OnPointerClick(BasicItem _item, PointerEventData _eventData) {}
	public override void OnPointerExit(PointerEventData eventData) {}

}
