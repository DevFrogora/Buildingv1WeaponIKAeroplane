using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DropzoneSlot2UI : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped to " + gameObject.name);
        if(eventData.pointerDrag.GetComponent<DraggableSlot1AssultUI>())
        {
            GameObject tempGameObject = BagInventory.instance.slot2.assultPrefab;
            BagInventory.instance.SetSlot2Assult(BagInventory.instance.slot1.assultPrefab);
            BagInventory.instance.SetSlot1Assult(tempGameObject);

        }
    }
}
