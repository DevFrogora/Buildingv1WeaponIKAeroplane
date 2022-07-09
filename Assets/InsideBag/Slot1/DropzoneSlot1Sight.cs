using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropzoneSlot1Sight : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped to " + gameObject.name);
        if (BagInventory.instance.slot1.assultPrefab == null) return;

        InventoryItemUI tempinventoryItemUI = eventData.pointerDrag.GetComponent<InventoryItemUI>();
        if (BagInventory.instance.slot1.assultPrefab.GetComponent<m416>() != null)
        {
            if (BagInventory.instance.slot1.sight == null)
            {
                Debug.Log("We are attaching red Dot");
                BagInventory.instance.SetSlot1Sight(tempinventoryItemUI.itemPrefab);
                Destroy(eventData.pointerDrag);
            }
        }
    }
}
