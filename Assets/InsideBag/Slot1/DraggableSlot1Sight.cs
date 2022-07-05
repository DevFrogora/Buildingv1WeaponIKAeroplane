using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DraggableSlot1Sight : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform canvasTransform;
    public Image draggableSightUI;
    Transform parentToReturnTo = null;
    CanvasGroup draggableUIcanvasGroup;
    public GameObject itemPrefab;

    private void Start()
    {
        draggableUIcanvasGroup = draggableSightUI.GetComponent<CanvasGroup>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        parentToReturnTo = draggableSightUI.transform.parent;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        draggableSightUI.transform.SetParent(canvasTransform);
        draggableUIcanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggableSightUI.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggableSightUI.transform.SetParent(parentToReturnTo);
        draggableUIcanvasGroup.blocksRaycasts = true;
    }
}
