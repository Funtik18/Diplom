using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpreter : MonoBehaviour {

	public static Interpreter _instance;

	[Tooltip("Главная программа")] public Container mainContainer;

	public TMPro.TextMeshProUGUI debugText;

	List<BasicSlot> slots = new List<BasicSlot>();
	List<Item> items = new List<Item>();

	public float timerHighLight;

	private int deepRecursion = 100;//max ~4500

	private void Awake() {
		_instance = this;
	}

	private void Update() {
		if(isReadAssignments)
			Stop();
	}
	void UndestandingProgram(List<Item> _functs, List<BasicSlot> _slots, int _level) {
		if(_level > deepRecursion) {
			Debug.Log("Error");
			//AddError("StackOverflow so many calls");
			return;
		}
		for(int i = 0; i < _functs.Count; i++) {
			slots.Add(_slots[i]);
			if(_functs[i].type == TypeItem.Reference) {
				
				ReferenceItem refItem = _functs[i] as ReferenceItem;
				List<Item> innerItems = refItem.container.GetItems();
				List<BasicSlot> innerSlots = refItem.container.GetSlots();

				UndestandingProgram(innerItems, innerSlots, _level +1);

			}
			
			items.Add(_functs[i]);	
			

		}
	}

	/// <summary>
	/// ReadAssignments
	/// </summary>
	Coroutine readAssignments;
	public bool isReadAssignments { get { return readAssignments != null; } }
	public void StartReadAssignments() {
		StopReadAssignments();

		items.Clear();
		slots.Clear();

		UndestandingProgram(mainContainer.GetItems(), mainContainer.GetSlots(), 0);

		if(readAssignments == null)
			readAssignments = this.StartCoroutine(ReadAssignments());
	}
	IEnumerator ReadAssignments() {
		for(int i = 0; i < items.Count; i++) {
			Item item = items[i];
			slots[i].SetActiveHoverItem(true);
			lastSlot = slots[i];
			yield return new WaitForSeconds(timerHighLight);
			if(item.type != TypeItem.Reference)
				yield return StartCoroutine(Analyst(item));

			slots[i].SetActiveHoverItem(false);
			lastSlot = null;
		}
		StopReadAssignments();
	}
	public void StopReadAssignments() {
		if(isReadAssignments) {
			this.StopCoroutine(readAssignments);
		}
		readAssignments = null;
		Restart();
		
	}
	public void RestartReadAssignments() {
		StopReadAssignments();
		StartReadAssignments();
	}
	private BasicSlot lastSlot = null;
	public void CheckLastSlot() {
		if (lastSlot != null) lastSlot.SetActiveHoverItem(false);
	}
	void Restart() {
		ButtonMainState._instance.MakeUpToRestart();//btn make Restart
	}
	void Stop() {
		ButtonMainState._instance.MakeUpToStop();//btn make Stop
	}


	IEnumerator Analyst(Item _item) {
		switch(_item.type) {
			case TypeItem.Direction: {
				switch(_item.name) {
					case "Left": {

						Player._instance.Left();
						break;
					}
					case "Right": {
						Player._instance.Right();
						break;
					}
					case "DownLeft": {

						Player._instance.DownLeft();
						break;
					}
					case "DownRight": {
						Player._instance.DownRight();
						break;
					}
					case "UpLeft": {

						Player._instance.UpLeft();
						break;
					}
					case "UpRight": {
						Player._instance.UpRight();
						break;
					}
					default: {
						Debug.LogError(_item.name + " такой команды нету");
						break;
					}
				}
				break;
			}
			default: {
				print("OUT");
				break;
			}
		}
		yield return new WaitForEndOfFrame();
	}

	public void AddError(string _textError) {
		if(!debugText.gameObject.activeSelf) {
			debugText.gameObject.SetActive(true);
		}
		if(debugText.text != "") {
			debugText.text += _textError;
		} else {
			debugText.text = _textError;
		}

	}

}
/*Debug.Log(Time.deltaTime + "+++");
		yield return new WaitForEndOfFrame();
		Debug.Log(Time.deltaTime + "+-+");
		while(true) {
			Debug.Log(Time.deltaTime + "---");
			yield return new WaitForSeconds(1);
			Debug.Log(Time.deltaTime);
		}*/
