using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Container : MonoBehaviour, IAllEvents {

	[Tooltip("Стартовые предметы")] [SerializeField] private List<Item> startItems = new List<Item>();
	protected List<Item> items = new List<Item>();
	protected List<BasicSlot> slots = new List<BasicSlot>();

	protected virtual void OnEnable() {
		foreach(Transform child in transform) {
			BasicSlot slot = child.GetComponent<BasicSlot>();
			slots.Add(slot);
		}
		FindBuffer();
	}
	protected virtual void Awake() {
		
	}
	protected virtual void Start() {
		InventoryOverseer._instance.containers.Add(this);
		Setup(slots);

		foreach(Item item in startItems) {//заполнение предметами
			AddItem(item);
		}
	}
	void Setup(List<BasicSlot> _slots) {
		foreach(BasicSlot slot in _slots) {
			//BasicSlot
			SubscribeSlotEvents(slot);
			//BasicItem
			BasicItem item = slot.currentItem;
			
			SubscribeItemEvents(item);

			slot.currentItem.Item = null;
		}
	}

	public void AddItem(Item _newItem) {
		
		foreach(BasicSlot slot in slots) {
			if(slot.IsEmpty()) {
				slot.currentItem.Item = _newItem;
				items.Add(_newItem);
				break;
			}
		}
	}

	public List<Item> GetItems() {
		return items;
	}
	public List<BasicSlot> GetSlots() {
		return slots;
	}

	public void DisposeItems() {
		items.Clear();
	}
	public void DisposeContainer() {
		for(int i = 0; i < slots.Count; i++) {
			slots[i].currentItem.Dispose();
		}
	}
	public void DisposeAll() {
		DisposeItems();
		DisposeContainer();
	}



	protected void SubscribeSlotEvents(BasicSlot _slot) {
		_slot.OnDropEvent += OnDrop;
	}
	protected void SubscribeItemEvents(BasicItem _item) {
		_item.OnPointerEnterEvent += PointerEnter;
		_item.OnPointerExitEvent += PointerExit;

		_item.OnPointerDownEvent += OnPointerDown;
		_item.OnPointerUpEvent += OnPointerUp;
		_item.OnPointerClickEvent += OnPointerClick;

		_item.OnBeginDragEvent += onBeginDrag;
		_item.OnDragEvent += Drag;
		_item.OnEndDragEvent += EndDrag;
	}

	protected void FindBuffer() {
		bufferItem = GameObject.FindGameObjectWithTag("BUFFER").GetComponent<BasicItem>();
		bufferRectTranform = bufferItem.GetComponent<RectTransform>();
	}

	
	#region Events

	//public bool ICanOverride;

	protected static bool isDrag = false;
	protected bool iCanDrag = false;

	protected int indexWhereToThrow = -1;

	protected BasicItem bufferItem;
	protected RectTransform bufferRectTranform;

	protected BasicItem draggableItem;


	/*
		Rect newRect= new Rect(50,50, 100, 100);
		private void OnGUI() {
			DrawRectangle(newRect, 1, Color.red);
		}
		void DrawRectangle(Rect area, int frameWidth, Color color) {
			//Create a one pixel texture with the right color
			var texture = new Texture2D(1, 1);
			texture.SetPixel(0, 0, color);
			texture.Apply();

			Rect lineArea = area;
			lineArea.height = frameWidth; //Top line
			GUI.DrawTexture(lineArea, texture);
			lineArea.y = area.yMax - frameWidth; //Bottom
			GUI.DrawTexture(lineArea, texture);
			lineArea = area;
			lineArea.width = frameWidth; //Left
			GUI.DrawTexture(lineArea, texture);
			lineArea.x = area.xMax - frameWidth;//Right
			GUI.DrawTexture(lineArea, texture);
		}
		public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2) {
			// Move origin from bottom left to top left
			screenPosition1.y = Screen.height - screenPosition1.y;
			screenPosition2.y = Screen.height - screenPosition2.y;
			// Calculate corners
			var topLeft = Vector3.Min(screenPosition1, screenPosition2);
			var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
			// Create Rect
			return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
		}
		*/

	protected virtual void PointerEnter(BasicItem _item, PointerEventData _eventData) {
		
	}
	protected virtual void PointerExit(BasicItem _item, PointerEventData _eventData) {}

	protected virtual void OnPointerDown(BasicItem _item, PointerEventData _eventData) {}
	protected virtual void OnPointerUp(BasicItem _item, PointerEventData _eventData) {}
	protected virtual void OnPointerClick(BasicItem _item, PointerEventData _eventData) {}

	protected virtual void onBeginDrag(BasicItem _item, PointerEventData _eventData) {
		draggableItem = _item;
		//я не могу перетаскивать объекты которых нету
		iCanDrag = !draggableItem.IsEmpty();

		if(!iCanDrag) return;
		isDrag = true;

		SetCanvasGroup(draggableItem.canvasGroup, 0.6f, false);

		bufferItem.transform.position = _eventData.pointerCurrentRaycast.worldPosition;
		bufferItem.Item = draggableItem.Item;

		InventoryOverseer._instance.from = this;//от куда взял

	}
	private void Drag(BasicItem _item, PointerEventData _eventData) {
		if(!iCanDrag) return;
		bufferRectTranform.anchoredPosition += _eventData.delta;
	}
	private void OnDrop(BasicSlot _slot, PointerEventData _eventData) {//слот дроп
		if(bufferItem.IsEmpty()) return;
		InventoryOverseer._instance.to = this;//куда положил

		if(!InventoryOverseer._instance.Verify() && !InventoryOverseer._instance.VerumTake(this)) {//если перетащили в другой контейнер то добавили
			if(indexWhereToThrow != -1 && !isEmpty()) {
				ShiftAllFrom(indexWhereToThrow);//что бы всё время не добавлять в конец
			}
			AddItem(bufferItem.Item);
		}
		isDrag = false;

		HightLightOff();
		NormalizedContainer();
	}
	protected virtual void EndDrag(BasicItem _item, PointerEventData _eventData) {
		if(!iCanDrag) return;
		
		SetCanvasGroup(_item.canvasGroup, 1f, true);

		 if(InventoryOverseer._instance.Verify()) {//если этот контейнер тот же от куда взяли
			draggableItem.Dispose();//удалили старый
			NormalizedContainer();
			if(indexWhereToThrow != -1 && !isEmpty()) {//сдвинули
				ShiftAllFrom(indexWhereToThrow);//что бы всё время не добавлять в конец
			}
			AddItem(bufferItem.Item);
		}else {
			draggableItem.Dispose();//удаляется тот который взяли из одного контэйнера и положили в другой или выбросили
		}

		InventoryOverseer._instance.Dispose();//если не освобождать, то удаление старого предмета не будет и произойдёт добавление его в конец
		bufferItem.Dispose();//освобождаем буффер

		NormalizedContainer();

		isDrag = false;
	}
	#endregion


	protected bool isFull() {
		List<BasicSlot> containerSlots = GetSlots();
		for(int i = 0; i < containerSlots.Count; i++) {
			if(containerSlots[i].IsEmpty())
				return false;
		}
		return true;
	}
	protected bool isEmpty() {
		List<BasicSlot> containerSlots = GetSlots();
		for(int i = 0; i < containerSlots.Count; i++) {
			if(!containerSlots[i].IsEmpty())
				return false;
		}
		return true;
	}

	protected void HightLightOff() {
		for(int i = 0; i < slots.Count; i++) {//убираем подсветки у всех слотовs
			slots[i].SetActiveRightHover(false);
			slots[i].SetActiveLeftHover(false);
		}
	}
	private void ShiftAllFrom(int _index) {//сдвиг всех предметов от индекса на 1
		_index++;
		DisposeContainer();
		for(int i = 0; i < items.Count; i++) {
			for(int j = 0; j < slots.Count; j++) {
				if(j == _index) continue;
				if(slots[j].IsEmpty()) {
					slots[j].currentItem.Item = items[i];
					break;
				}
			}
		}
	}
	protected void NormalizedContainer() {//делает предметы слотов попорядку слева направо сверху вниз
		List<Item> currentItems = new List<Item>();
		for(int i = 0; i < slots.Count; i++) {
			if(!slots[i].IsEmpty()) {
				currentItems.Add(slots[i].currentItem.Item);
			}
		}
		DisposeAll();
		foreach(Item item in currentItems) {
			AddItem(item);
		}
	}
	protected void SetCanvasGroup(CanvasGroup _canvasGroup, float _a, bool _raycast) {
		_canvasGroup.alpha = _a;//альфа
		_canvasGroup.blocksRaycasts = _raycast;//выключаем рэйкаст чтобы можно было выполнить onDrop эвент
	}

	public void OnPointerDown(PointerEventData eventData) {

	}
	public void OnPointerClick(PointerEventData eventData) {
	}
	public void OnPointerUp(PointerEventData eventData) {

	}

	public void OnPointerEnter(PointerEventData eventData) {

	}
	public virtual void OnPointerExit(PointerEventData eventData) {
		slots[0].SetActiveLeftHover(false);
	}

	public virtual void OnBeginDrag(PointerEventData eventData) {
		
	}
	public virtual void OnDrag(PointerEventData eventData) {
	}
	public virtual void OnEndDrag(PointerEventData eventData) {

	}
}
