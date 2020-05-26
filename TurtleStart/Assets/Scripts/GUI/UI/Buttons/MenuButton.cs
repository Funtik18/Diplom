
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : BasicButton {

    public Transform movePanel;

    private void Awake() {
        if (movePanel == null) {
            movePanel = transform.parent;
        }

        main = GetComponent<Button>();
        main.onClick.AddListener(()=>StartClick());
    }
    private void StartClick() {
        UIFader fader = movePanel.GetComponent<UIFader>();
        fader.FadeOut();
        fader.OnOffRayCast(false);
    }
}
