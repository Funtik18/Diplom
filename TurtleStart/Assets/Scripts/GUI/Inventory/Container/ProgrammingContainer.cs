using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProgrammingContainer : Container {
	BasicSlot hoverSlot;

	int width = 5;////////////////поменять
	protected override void Start() {
		base.Start();
		InventoryOverseer._instance.programmingContainers.Add(this);
	}
	private void Update() {

	}
	protected override void PointerEnter( BasicItem _item, PointerEventData _eventData ) {
		if (isDrag && !bufferItem.IsEmpty()) {//если буфер не пуст
			if (!isFull() || InventoryOverseer._instance.Verify()) {//если контэйнер не полон или это тот контейнер откуда взяли
				hoverSlot = _item.GetComponentInParent<BasicSlot>();
				indexThrow = slots.FindIndex(x => x == hoverSlot);

				//print(indexWhereToThrow);

				if (hoverSlot.IsEmpty() == false) {//если слот на который навели не пуст	
												   //когда драгаешь сам на себя переходит в конец контэйнера
					if (( indexThrow + 1 ) % width == 0) {//если навёл на последний элемент в строке
						slots[indexThrow + 1].SetActiveLeftHover(true);//то подсвечиваем на новой строке
					} else {
						hoverSlot.SetActiveRightHover(true);//то подсвечиваем право для вставки
					}
				} else {//если навёл на пустую ячейку, то подсветка самого последнего или первого
					if (isEmpty())//если контэйнер пуст то подсвечивается первый слот
						slots[0].SetActiveLeftHover(true);
					else {//или же последний
						if (items.Count % width == 0) {
							slots[items.Count].SetActiveLeftHover(true);//не уверен
						} else {
							slots[items.Count - 1].SetActiveRightHover(true);
						}
					}
				}
			} else {
				//IsFull
			}
		}
	}
	protected override void PointerExit( BasicItem _item, PointerEventData _eventData ) {
		if (isDrag) {
			HightLightOff();
			indexThrow = slots.FindIndex(x => x == hoverSlot);
		}
	}

	protected override void OnPointerClick( BasicItem _item, PointerEventData _eventData ) {
		if (!isDrag)
			if (!_item.IsEmpty()) {//если слот не пуст то удаляем объект и нормализуем контэйнер
				RemoveItem(_item.Item);
				_item.Dispose();
				NormalizedContainerBySlots();
			}
	}
}
