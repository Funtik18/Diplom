using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartGameButton : BasicButton {
    private void Awake() {
        main = GetComponent<Button>();
        main.onClick.AddListener(() => RestartClick());
    }
    private void RestartClick() {
        SaveLoadManager.LoadScene("Game", MapConstants.MapName, MapConstants.MapOptions);
    }
}
