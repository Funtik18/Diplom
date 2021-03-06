﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainStateButton : BasicButton{

	public static MainStateButton _instance;

	int expectedClick = 0;

	int state = -1;
	public int State {
		get { return state; }
		set {
			state = value;
			switch(state) {
				case 0: {//Start
					MakeUpToRestart();
					Play();
					break;
				}
				case 1: {//Restart
					MakeUpToStart();
					Restart();
					break;
				}
				case 2: {//Stop
					MakeUpToRestart();
					Stop();
					break;
				}
			}
		}
	}

	private void Awake() {

		_instance = this;

		main = GetComponent<Button>();
		main.onClick.AddListener(delegate { ChangedButton(); });

		MakeUpToStart();
	}

	void ChangedButton() {
		State = expectedClick;
	}

	public void MakeUpToStart() {
		main.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Play";
		expectedClick = 0;
	}
	public void MakeUpToRestart() {
		main.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Restart";
		expectedClick = 1;
	}
	public void MakeUpToStop() {
		main.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Stop";
		expectedClick = 2;
	}

	
	void Play() {
		Interpreter._instance.StartReadAssignments();
	}
	void Stop() {
		Interpreter._instance.StopReadAssignments();
	}
	void Restart() {
		Player._instance.RestartPosition();
		Interpreter._instance.CheckLastSlot();
		//InventoryOverseer._instance.HightLightOff();
		//Interpreter._instance.items.Clear();
		//Interpreter._instance.mainContainer.DisposeContainer();
		//Interpreter._instance.RestartReadAssignments();
	}
	public void Clear() {
		Player._instance.RestartPosition();
		Interpreter._instance.mainContainer.DisposeAll();
		InventoryOverseer._instance.DisposeContainers();
	}

}
