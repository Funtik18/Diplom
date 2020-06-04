using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Container : MonoBehaviour, IAllEvents {

	[Tooltip("Стартовые предметы")] [SerializeField] public List<Item> startItems = new List<Item>();
	protected List<Item> items = new List<Item>();
	public List<BasicSlot> slots = new List<BasicSlot>();

	protected virtual void OnEnable() {
		
	}
	protected virtual void Awake() {

	}
	protected virtual void Start() {
		foreach (Transform child in transform) {
			BasicSlot slot = child.GetComponent<BasicSlot>();
			slots.Add(slot);
		}
		FindBuffer();
		InventoryOverseer._instance.allContainers.Add(this);
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
		for(int i = 0; i < slots.Count; i++) {
			if(slots[i].IsEmpty()) {
				slots[i].currentItem.Item = _newItem;
				items.Add(_newItem);
				break;
			}
		}
	}
	public void AddItems(List<Item> _newItems) {
		for(int i = 0; i < _newItems.Count; i++) {
			AddItem(_newItems[i]);
		}
	}
	public void RemoveItem( Item _item ) {
		items.Remove(_item);
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

	protected int indexThrow = -1;
	private int indexTake = -1;

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
		//container
	protected virtual void PointerEnter(BasicItem _item, PointerEventData _eventData) {
	}
	protected virtual void PointerExit(BasicItem _item, PointerEventData _eventData) {
	}

	protected virtual void OnPointerDown(BasicItem _item, PointerEventData _eventData) {
	}
	protected virtual void OnPointerUp(BasicItem _item, PointerEventData _eventData) {
	}
	protected virtual void OnPointerClick(BasicItem _item, PointerEventData _eventData) {
	}
	
	//items slots
	protected virtual void onBeginDrag(BasicItem _item, PointerEventData _eventData) {
		draggableItem = _item;
		//я не могу перетаскивать объекты которых нету
		iCanDrag = !draggableItem.IsEmpty();

		if(!iCanDrag) return;
		isDrag = true;


		bufferItem.transform.position = _eventData.pointerCurrentRaycast.worldPosition;
		bufferItem.Item = draggableItem.Item;

		indexTake = slots.FindIndex(x => x.currentItem == draggableItem);

		RemoveItem(draggableItem.Item);
		draggableItem.Dispose();

		NormalizedContainerBySlots();

		BasicSlot slot = _item.GetComponentInParent<BasicSlot>();
		indexThrow = slots.FindIndex(x => x == slot);
		slots[indexThrow].SetActiveRightHover(true);

		InventoryOverseer._instance.from = this;//от куда взял

	}
	protected virtual void Drag(BasicItem _item, PointerEventData _eventData) {
		if (!iCanDrag) return;
		bufferRectTranform.anchoredPosition += _eventData.delta;
	}
	private void OnDrop(BasicSlot _slot, PointerEventData _eventData) {//нельзя переобределять, тк функция OnDrop должна быть на каждом слоте
		if (bufferItem.IsEmpty()) return;
		InventoryOverseer._instance.to = this;//куда положил
		Container to = this;
		if (!InventoryOverseer._instance.Verify() && !InventoryOverseer._instance.VerumTake(to)) {//если перетащили в другой контейнер то добавили	
			if (indexThrow != -1) {//если хотим добваить
				if(!to.isFull())
					if (to.slots[indexThrow].IsEmpty())
						to.AddItem(bufferItem.Item);//добавляем в конец
					else
						to.InsertItemTo(bufferItem.Item);
			} else {
				print("+");
			}
		} else 
			return;
		
		isDrag = false;

		HightLightOff();
	}
	protected virtual void EndDrag(BasicItem _item, PointerEventData _eventData) {
		if (!iCanDrag) return;
		InventoryOverseer._instance.to = this;//куда положил
		
		if (InventoryOverseer._instance.Verify()) {//если этот контейнер тот же от куда взяли
			if (indexThrow != -1 && !isEmpty()) {//сдвинули
				if(!slots[indexThrow].IsEmpty())
					InsertItemTo(bufferItem.Item);//что бы всё время не добавлять в конец
				else AddItem(bufferItem.Item);
			}else if(indexThrow == -1) {
				
				
				print(indexThrow);
				//InsertItemTo(bufferItem.Item);
			}
		}

		InventoryOverseer._instance.Dispose();//если не освобождать, то удаление старого предмета не будет и произойдёт добавление его в конец
		bufferItem.Dispose();//освобождаем буффер

		indexThrow = -1;
		isDrag = false;

		HightLightOff();
	}
	#endregion


	/// <summary>
	/// Добавление item в items
	/// </summary>
	/// <param name="_item"></param>
	/// <param name="_shift">Если 0 то добавляет за место этого item, если 1 то перед ним.</param>
	public void InsertItemTo( Item _item, int _shift = 1 ) {

		List<Item> newitems = HelpFunctions.GetCopy<Item>(items);

		newitems.Insert(indexThrow + _shift, _item);

		DisposeAll();
		AddItems(newitems);

		NormalizedContainerBySlots();
	}


	public bool isFull() {
		List<BasicSlot> containerSlots = GetSlots();
		for(int i = 0; i < containerSlots.Count; i++) {
			if(containerSlots[i].IsEmpty())
				return false;
		}
		return true;
	}
	public bool isEmpty() {
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
	protected void SetCanvasGroup( CanvasGroup _canvasGroup, float _a, bool _raycast ) {
		_canvasGroup.alpha = _a;//альфа
		_canvasGroup.blocksRaycasts = _raycast;//выключаем рэйкаст чтобы можно было выполнить onDrop эвент
	}

	/// <summary>
	/// делает предметы слотов попорядку слева направо сверху вниз
	/// </summary>
	protected void NormalizedContainerBySlots() {
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

	protected void NormalizedContainerByItems() {

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

		indexThrow = -1;
	}

	public virtual void OnBeginDrag(PointerEventData eventData) {
		
	}
	public virtual void OnDrag(PointerEventData eventData) {
	}
	public virtual void OnEndDrag(PointerEventData eventData) {
		
	}
}
