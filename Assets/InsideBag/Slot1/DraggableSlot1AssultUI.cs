using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DraggableSlot1AssultUI : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform canvasTransform;
    public Image assultImage;
    public Image draggableUI;
    Transform parentToReturnTo = null;
    CanvasGroup draggableUIcanvasGroup;

    private void Start()
    {
        draggableUIcanvasGroup = draggableUI.GetComponent<CanvasGroup>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        parentToReturnTo = draggableUI.transform.parent;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        draggableUI.transform.SetParent(canvasTransform);
        draggableUIcanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggableUI.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggableUI.transform.SetParent(parentToReturnTo);
        draggableUIcanvasGroup.blocksRaycasts = true;
    }


}