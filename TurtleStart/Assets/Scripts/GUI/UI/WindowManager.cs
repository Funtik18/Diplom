using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour{
	public static WindowManager _instance;

	public Transform root;

	public GameObject winWindow;

	public GameObject preferenceWindow;

	private void Awake() {
		_instance = this;
	}

	public void ShowWinWindow() {
		FadePreference(false);

		FadeBackground(true);
		FadeWin(true);
	}
	public void ShowPreferenceWindow() {
		FadeWin(false);

		FadeBackground(true);
		FadePreference(true);
	}
	private void FadeBackground(bool _triger) {
		UIFader background = root.GetComponent<UIFader>();
		background.OnOffRayCast(_triger);
		if(_triger) background.FadeIn();
		else background.FadeOut();
	}
	private void FadeWin( bool _triger ) {
		UIFader fader = winWindow.GetComponent<UIFader>();
		fader.OnOffRayCast(_triger);
		if (_triger) fader.FadeIn();
		else fader.FadeOut();
	}
	private void FadePreference( bool _triger ) {
		UIFader fader = preferenceWindow.GetComponent<UIFader>();
		fader.OnOffRayCast(_triger);
		if (_triger) fader.FadeIn();
		else fader.FadeOut();
	}
}
