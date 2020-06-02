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
		if (!_item.IsEmpty() && WhithClicks) {//если слот не пуст то добавить предмет в контэйнер there
			int counter = 0;
			for (int i = 0; i < InventoryOverseer._instance.programmingContainers.Count; i++) {//вот она самая тупая вещь в моей жизни
				Container temp = InventoryOverseer._instance.programmingContainers[i];
				if (!temp.isFull()) {
					InventoryOverseer._instance.put = temp;
					break;
				} else {
					counter++;
				}
			}
			if (counter != InventoryOverseer._instance.programmingContainers.Count && InventoryOverseer._instance.put!=null)
				InventoryOverseer._instance.put.AddItem(_item.Item);
		}
	}
}

