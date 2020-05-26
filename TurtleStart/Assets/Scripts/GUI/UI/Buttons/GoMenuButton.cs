using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoMenuButton : BasicButton
{
    private void Awake() {
        main = GetComponent<Button>();
        main.onClick.AddListener(() => GoHomeClick());
    }
    private void GoHomeClick() {
        SaveLoadManager.LoadScene("Menu");
    }
}
