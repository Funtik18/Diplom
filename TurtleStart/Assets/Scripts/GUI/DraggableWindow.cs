using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IAllEvents{

    private Vector2 pointerOffset;//для того чтобы не перетаскивать с середины
    public RectTransform container;//ограничение
    public RectTransform root;//что двигаем


    private bool clampedToLeft;
    private bool clampedToRight;
    private bool clampedToTop;
    private bool clampedToBottom;

    public void Start() {
        if(container == null)
            container = transform.parent as RectTransform;
        if(root == null)
            root = transform as RectTransform;

        clampedToLeft = false;
        clampedToRight = false;
        clampedToTop = false;
        clampedToBottom = false;
    }
    List<Vector2> pointersOffsets;
    public void OnBeginDrag(PointerEventData eventData) {
        //child.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(root, eventData.position, eventData.pressEventCamera, out pointerOffset);
    }
    public void OnDrag(PointerEventData eventData) {

        Vector2 localPointerPosition;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(container, eventData.position, eventData.pressEventCamera, out localPointerPosition)) {

            root.localPosition = localPointerPosition - pointerOffset;//движение
            ClampToWindow();//определение перешёл ли границу экрана
            Vector2 clampedPosition = root.localPosition;
            OffsetBorders(ref clampedPosition);
            root.localPosition = clampedPosition;

        }

    }
    public void OnEndDrag(PointerEventData eventData) {

    }


    void ClampToWindow() {
        Vector3[] canvasCorners = new Vector3[4];
        Vector3[] panelRectCorners = new Vector3[4];

        container.GetWorldCorners(canvasCorners);
        root.GetWorldCorners(panelRectCorners);

        if(panelRectCorners[2].x > canvasCorners[2].x) {//Debug.Log("Panel is to the right of canvas limits");
            if(!clampedToRight) {
                clampedToRight = true;
            }
        } else if(clampedToRight) {
            clampedToRight = false;
        } 
        
        if(panelRectCorners[0].x < canvasCorners[0].x) {//Debug.Log("Panel is to the left of canvas limits");
            if(!clampedToLeft) {
                clampedToLeft = true;
            }
        } else if(clampedToLeft) {
            clampedToLeft = false;
        }

        if(panelRectCorners[2].y > canvasCorners[2].y) {//Debug.Log("Panel is to the top of canvas limits");
            if(!clampedToTop) {
                clampedToTop = true;
            }
        } else if(clampedToTop) {
            clampedToTop = false;
        }
        
        if(panelRectCorners[0].y < canvasCorners[0].y) {//Debug.Log("Panel is to the bottom of canvas limits");
            if(!clampedToBottom) {
                clampedToBottom = true;
            }
        } else if(clampedToBottom) {
            clampedToBottom = false;
        }
    }

    void OffsetBorders(ref Vector2 _clampedPosition) {
        if(clampedToRight) {
            _clampedPosition.x = ( container.rect.width / 2 ) - ( root.rect.width * ( 1 - root.pivot.x ) );
        }

        if(clampedToLeft) {
            _clampedPosition.x = ( -container.rect.width / 2 ) + ( root.rect.width * root.pivot.x );
        }

        if(clampedToTop) {
            _clampedPosition.y = ( container.rect.height / 2 ) - ( root.rect.height * ( 1 - root.pivot.y ) );
        }

        if(clampedToBottom) {
            _clampedPosition.y = ( -container.rect.height / 2 ) + ( root.rect.height * root.pivot.y );
        }
    }


    /* void CheckBorders(ref Vector2 _clampedPosition) {
        if(clampedToRight) {
            _clampedPosition.x = ( panelRectTransform.sizeDelta / 2 ).x;
        }
        
        if(clampedToLeft) {
            _clampedPosition.x = ( -panelRectTransform.sizeDelta / 2 ).x;
        }

        if(clampedToTop) {
            _clampedPosition.y = ( panelRectTransform.sizeDelta / 2 ).y;
        }
        
        if(clampedToBottom) {
            _clampedPosition.y = ( -panelRectTransform.sizeDelta / 2 ).y;
        }
    }*/

    /* public RectTransform root;

     public float speed;

     /// <summary>
     /// Пространство между якорями rectTransform'а
     /// </summary>
     Vector2 anchorPadding { get { return root.anchorMax - root.anchorMin; } }

     Vector2 widthPadding;
     Vector2 heightPadding;

     private void Awake() {
         widthPadding = new Vector2((root.sizeDelta.x / 2) * -1, root.sizeDelta.x / 2);
         heightPadding = new Vector2(( root.sizeDelta.y / 2 ) * -1, root.sizeDelta.y / 2);
     }


     public void SetRect(RectTransform _rectTransform) {
         root = _rectTransform;
     }



     public void OnBeginDrag(PointerEventData eventData) {

     }
     public void OnDrag(PointerEventData eventData) {
         Vector2 pos = root.anchoredPosition;
         Vector2 targetPos = eventData.delta;

         root.anchoredPosition += targetPos;

         if(pos.x < widthPadding.x) {
             root.anchoredPosition  = new Vector2(widthPadding.x, pos.y);
         }
         if(pos.x > widthPadding.y) {
             root.anchoredPosition = new Vector2(widthPadding.y, pos.y);
         }
         if(pos.y < heightPadding.x) {
             root.anchoredPosition = new Vector2(pos.x, heightPadding.x);
         }
         if(pos.y > heightPadding.y) {
             root.anchoredPosition = new Vector2(pos.x, heightPadding.y);
         }
     }
     public void OnEndDrag(PointerEventData eventData) {

     }
     */
    public void OnPointerClick(PointerEventData eventData) {
    }
    public void OnPointerDown(PointerEventData eventData) {
    }
    public void OnPointerEnter(PointerEventData eventData) {
    }
    public void OnPointerExit(PointerEventData eventData) {
    }
    public void OnPointerUp(PointerEventData eventData) {
    }

}
