using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropzoneDraggableItem : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped to " + gameObject.name);
        if (eventData.pointerDrag.GetComponent<DraggableSlot1AssultUI>())
        {
            Debug.Log("We are Dropping Slot1 Assult");
            if(BagInventory.instance.slot1.assultPrefab != null)
            {
                GameObject item = (BagInventory.instance.slot1.assultPrefab);
                item.transform.SetParent(transform.root.parent);
                RaycastHit hitInfo = GroundHitter.instance.HitGround();
                item.transform.position =(Vector3) hitInfo.point;
                item.transform.rotation = Quaternion.identity;

                BagInventory.instance.SetSlot1Assult(null);
                BagUIBroadcast.instance.DroppedSlot(1,true);
            }
        }

        if (eventData.pointerDrag.GetComponent<InventoryItemUI>())
        {
            Debug.Log("Dropping Item From All Slot : Name: "+eventData.pointerDrag.GetComponent<InventoryItemUI>().itemPrefab.name);
            if (BagInventory.instance.mixItem != null)
            {
                //GameObject item = (BagInventory.instance.mixItem.);
                //item.transform.SetParent(transform.root.parent);
                //BagInventory.instance.SetSlot1Assult(null);
                BagInventory.instance.mixItem.Remove(eventData.pointerDrag.GetComponent<InventoryItemUI>().itemPrefab);
                eventData.pointerDrag.GetComponent<InventoryItemUI>().itemPrefab.transform.SetParent(transform.root.parent);
                Destroy(eventData.pointerDrag);
            }
        }
    }
}

