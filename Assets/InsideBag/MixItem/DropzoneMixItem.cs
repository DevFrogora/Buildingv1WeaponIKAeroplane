using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DropzoneMixItem : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropping Item From All Slot : Name: " + eventData.pointerDrag);

        if (eventData.pointerDrag.GetComponent<DraggableSlot1Sight>() != null)
        {
            DraggableSlot1Sight draggableSlot1Sight = eventData.pointerDrag.GetComponent<DraggableSlot1Sight>();
            BagInventory.instance.AddInMixItem(draggableSlot1Sight.itemPrefab);
            BagInventory.instance.SetSlot1Sight(null);
            draggableSlot1Sight.itemPrefab = null;
            draggableSlot1Sight.draggableSightUI.sprite = null;
        }
    }
}
