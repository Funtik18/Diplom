using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : BasicButton {
    public Transform closePanel;

    private void Awake() {
        if (closePanel == null) {
            closePanel = transform.parent;
        }

        main = GetComponent<Button>();
        main.onClick.AddListener(() => OpenCloseClick());
    }
    private void OpenCloseClick() {
        /*UIFader fader = closePanel.GetComponent<UIFader>();
        fader.FadeOut();


        UIFader background = closePanel.parent.GetComponent<UIFader>();
        background.FadeOut();
        background.OnOffRayCast(false);*/

        WindowManager._instance.HidePreferenceWindow();
    }
}
