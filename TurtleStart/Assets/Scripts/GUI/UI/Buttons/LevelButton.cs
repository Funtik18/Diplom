using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelButton : BasicButton {
	[SerializeField] TMPro.TextMeshProUGUI txt;
	public int Count {
		get {
			return Convert.ToInt32(txt.text);
		}
		set {
			txt.text = value.ToString();
		}
	}
	private void Awake() {
		main = GetComponent<Button>();
	}
	public void SetCall( string _str ) {
		main.onClick.AddListener(delegate { SaveLoadManager.LoadScene("Game", _str); });
	}

}
