using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCloseFunction : MonoBehaviour {

	public static OpenCloseFunction _instance;

	public Transform content;
	public Program prefabProgram;
	public Button addBtn;

	private void OnEnable() {

		_instance = this;
		
		if(!addBtn)
			addBtn = content.GetChild(0).GetComponentInChildren<Button>();

		addBtn.onClick.AddListener(delegate { AddFunction(); });
	}

	private void AddFunction() {
		Program newProgram = Instantiate<Program>(prefabProgram);
		newProgram.transform.SetParent(content, false);
	}
	public void DeleteDisabledFunctions() {//пропускаем первые два объкта
		int count = content.childCount;
		for(int i = 2; i < count; i++) {
			GameObject obj = content.GetChild(i).gameObject;
			if (!obj.activeSelf) {
				HelpFunctions.DestroyObject(obj);
			}
		}
	}
}
