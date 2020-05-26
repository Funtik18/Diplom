using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

[RequireComponent(typeof(GridLayoutGroup))]
public class LevelSelector : MonoBehaviour{

    public LevelButton prefabLevel;

    GridLayoutGroup grid;

    void Awake() {
        grid = GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(200, 200);
        grid.spacing = new Vector2(100, 100);

        HelpFunctions.DestroyChilds(transform);

        List<TextAsset> maps = SaveLoadManager.GetMaps().ToList();

        for (int i = 0; i < maps.Count; i++) {
            LevelButton level = Instantiate<LevelButton>(prefabLevel);
            level.SetCall(maps[i].name);

            level.Count = i;

            Transform temp = level.transform;

            temp.SetParent(transform);

            temp.localScale = Vector3.one;
            temp.localPosition = Vector3.zero;
        }
    }
}
