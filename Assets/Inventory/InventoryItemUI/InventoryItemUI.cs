using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemUI : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TextMeshProUGUI itemName;
    public Image image;
    public Image draggableImage;

    public GameObject itemPrefab;


    public Transform canvasTransform;
    Transform parentToReturnTo = null;
    CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        parentToReturnTo = draggableImage.transform.parent;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        draggableImage.transform.SetParent(canvasTransform);
        draggableImage.GetComponent<DraggableItemUI>().GetComponent<CanvasGroup>().blocksRaycasts = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("item Dragging");
        draggableImage.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggableImage.transform.SetParent(parentToReturnTo);
        draggableImage.GetComponent<DraggableItemUI>().GetComponent<CanvasGroup>().blocksRaycasts = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void OnDestroy()
    {
        Destroy(draggableImage);
    }
}