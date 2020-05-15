using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCloseFunction : MonoBehaviour {

	public static OpenCloseFunction _instance;

	public Transform content;
	public Program prefabProgram;

	private Button addBtn;

	private void OnEnable() {

		_instance = this;

		addBtn = content.GetChild(0).GetComponentInChildren<Button>();

		addBtn.onClick.AddListener(delegate { AddFunction(); });
	}

	private void AddFunction() {

		//transform.parent.parent.GetChild(1).gameObject.GetComponent<Scrollbar>().value = 1;
		Program newProgram = Instantiate<Program>(prefabProgram);

		newProgram.transform.SetParent(content, false);

		//transform.parent.parent.GetChild(1).gameObject.GetComponent<Scrollbar>().value = 1;
	}
	public void DeleteDisableFunctions() {
		int count = content.childCount;
		for(int i = 2; i < count; i++) {
			Transform temp = content.GetChild(i);
			if(!temp.gameObject.activeSelf) {
				Destroy(temp.gameObject);
			}
		}
	}
}
