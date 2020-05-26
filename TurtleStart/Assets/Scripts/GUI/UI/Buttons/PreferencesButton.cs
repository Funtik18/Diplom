using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreferencesButton : BasicButton {

    public Transform showPanel;

    private void Awake() {
        if (showPanel == null) {
            return;
        }

        main = GetComponent<Button>();
        main.onClick.AddListener(() => OpenCloseClick());
    }
    private void OpenCloseClick() {
        WindowManager._instance.ShowPreferenceWindow();
    }
}
