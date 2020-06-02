using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ParallaxPanel : MonoBehaviour, IPointerEvents{

    [Header("X and Y max rotation")]
    [Tooltip("Set to 0 if you want don't want it to rotate along this axis")]
    public float y_maxRot;
    [Tooltip("Set to 0 if you want don't want it to rotate along this axis")]
    public float x_maxRot;

    [Tooltip("Speed for the rotation")]
    public float speed;

    RectTransform rect;
    [Tooltip("The rect we want to rotate")]
    public RectTransform rectToRotate;
    public Canvas canvas;
    private void Awake() {
        rect = GetComponent<RectTransform>();

        rect.anchoredPosition = new Vector2(Screen.width/2, Screen.height/2);

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.zero;

        
    }

    //Our target eulerangles rotation
    Vector2 targetEulerAngles = Vector3.zero;

    private void Update() {//rect.anchoredPosition3D = Input.mousePosition;

        Vector2 diff = (Vector2)rect.anchoredPosition - (Vector2)Input.mousePosition;

        if (Mathf.Abs(diff.x) <= ( rect.sizeDelta.x / 2f ) && Mathf.Abs(diff.y) <= ( rect.sizeDelta.y / 2f )) {
                targetEulerAngles = new Vector3(
                    x_maxRot * -Mathf.Clamp(diff.y / ( rect.sizeDelta.y / 2f ), -1, 1),
                    y_maxRot * Mathf.Clamp(diff.x / ( rect.sizeDelta.x / 2f ), -1, 1),
                    0);
        } else {
                targetEulerAngles = Vector3.zero;
        }

            //Lerps the rotation
            rectToRotate.eulerAngles = AngleLerp(rectToRotate.eulerAngles, targetEulerAngles, speed * Time.deltaTime);
    }
    public static Vector3 AngleLerp( Vector3 StartAngle, Vector3 FinishAngle, float t ) {
        return new Vector3(
            Mathf.LerpAngle(StartAngle.x, FinishAngle.x, t),
            Mathf.LerpAngle(StartAngle.y, FinishAngle.y, t),
            Mathf.LerpAngle(StartAngle.z, FinishAngle.z, t)
            );
    }
    bool isMove = false;
    Vector2 start;
    public void OnPointerDown( PointerEventData eventData ) => throw new System.NotImplementedException();
    public void OnPointerClick( PointerEventData eventData ) => throw new System.NotImplementedException();
    public void OnPointerUp( PointerEventData eventData ) => throw new System.NotImplementedException();
    public void OnPointerEnter( PointerEventData eventData ) {
        isMove = true;
        start = eventData.delta;
    }
    public void OnPointerExit( PointerEventData eventData ) {
        isMove = false;
        start = eventData.delta;
    }
}
