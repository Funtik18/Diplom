using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageContainer : Container {
	public bool IsInfinity;
	public bool WhithClicks;

	protected override void Start() {
		base.Start();
		InventoryOverseer._instance.storageContainers.Add(this);
	}

	protected override void onBeginDrag( BasicItem _item, PointerEventData _eventData ) {
		if (Interpreter._instance.isReadAssignments) return;
		draggableItem = _item;
		//я не могу перетаскивать объекты которых нету
		iCanDrag = !draggableItem.IsEmpty();

		if (!iCanDrag) return;
		isDrag = true;

		SetCanvasGroup(draggableItem.canvasGroup, 0.6f, false);

		bufferItem.transform.position = _eventData.pointerCurrentRaycast.worldPosition;
		bufferItem.Item = draggableItem.Item;

		InventoryOverseer._instance.from = this;//от куда взял
	}

	protected override void EndDrag( BasicItem _item, PointerEventData _eventData ) {
		if (Interpreter._instance.isReadAssignments) return;
		if (!iCanDrag) return;

		SetCanvasGroup(_item.canvasGroup, 1f, true);

		bufferItem.Dispose();//освобождаем буффер

		if (!IsInfinity && !InventoryOverseer._instance.Verify()) {//если не бесконечный и в другой контейнер
			draggableItem.Dispose();//то удалили в старом
			NormalizedContainerBySlots();
		}
		isDrag = false;
	}

	protected override void OnPointerClick( BasicItem _item, PointerEventData _eventData ) {
		if (Interpreter._instance.isReadAssignments) return;
		if (!_item.IsEmpty() && WhithClicks) {
			if(InventoryOverseer._instance.put == null) {
				InventoryOverseer._instance.put = InventoryOverseer._instance.programmingContainers[1];
			}
			if (InventoryOverseer._instance.put.gameObject.activeSelf) {
				int counter = 0;
				for (int i = 0; i < InventoryOverseer._instance.programmingContainers.Count; i++) {
					Container temp = InventoryOverseer._instance.programmingContainers[i];
					if (!temp.isFull() && temp.gameObject.activeSelf) {
						InventoryOverseer._instance.put = temp;
						break;
					} else {
						counter++;
					}
				}
				if (counter != InventoryOverseer._instance.programmingContainers.Count)
					InventoryOverseer._instance.put.AddItem(_item.Item);
			} else {
				int counter = 0;
				for (int i = 0; i < InventoryOverseer._instance.programmingContainers.Count; i++) {
					Container temp = InventoryOverseer._instance.programmingContainers[i];
					if (!temp.isFull() && temp.gameObject.activeSelf) {
						InventoryOverseer._instance.put = temp;
						break;
					} else {
						counter++;
					}
				}
				if (counter != InventoryOverseer._instance.programmingContainers.Count)
					InventoryOverseer._instance.put.AddItem(_item.Item);
				//OpenCloseFunction._instance.DeleteDisabledFunctions();
			}
		}
	}
}

