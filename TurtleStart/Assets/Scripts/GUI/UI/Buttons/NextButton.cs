using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NextButton : BasicButton {

	private void Awake() {
		main = GetComponent<Button>();
		TextAsset[] assets = SaveLoadManager.GetMaps();
		int rnd = Random.Range(0, assets.Length);
		SetCall(assets[rnd].name);
	}
	public void SetCall( string _str ) {
		main.onClick.AddListener(delegate { SaveLoadManager.LoadScene("Game", _str); });
	}

}
