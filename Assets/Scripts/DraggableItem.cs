using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private RawImage img;
    private Text textchild;
    [HideInInspector] public Transform parentAfterDrag;

    private void Awake()
    {
        img = GetComponent<RawImage>();
        textchild = GetComponentInChildren<Text>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        img.raycastTarget = false;
        textchild.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        img.raycastTarget = true;
        textchild.raycastTarget = true;
    }
 
}
