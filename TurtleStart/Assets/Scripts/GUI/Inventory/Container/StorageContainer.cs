using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageContainer : Container{
	public bool IsInfinity;
	public bool WhithClicks;

	protected override void Start() {
		base.Start();
		InventoryOverseer._instance.storageContainers.Add(this);
	}

	protected override void EndDrag(BasicItem _item, PointerEventData _eventData) {
		if(!iCanDrag) return;

		SetCanvasGroup(_item.canvasGroup, 1f, true);

		bufferItem.Dispose();//освобождаем буффер

		if(!IsInfinity && !InventoryOverseer._instance.Verify()) {//если не бесконечный и в другой контейнер
			draggableItem.Dispose();//то удалили в старом
			NormalizedContainer();
		}
		isDrag = false;

	}

	protected override void OnPointerClick(BasicItem _item, PointerEventData _eventData) {
		if(!_item.IsEmpty() &&  WhithClicks) {//если слот не пуст то добавить предмет в контэйнер there
			for(int i = 0; i < InventoryOverseer._instance.programmingContainers.Count; i++) {//вот она самая тупая вещь в моей жизни
				Container temp = InventoryOverseer._instance.programmingContainers[i];
				if (!temp.isFull()) {
					InventoryOverseer._instance.put = temp;
					break;
				}
			}

			InventoryOverseer._instance.put.AddItem(_item.Item);
		}
	}

	
}
