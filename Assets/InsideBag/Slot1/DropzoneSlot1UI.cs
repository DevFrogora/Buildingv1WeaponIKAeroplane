using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DropzoneSlot1UI : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped to " + gameObject.name);
        if (eventData.pointerDrag.GetComponent<DraggableSlot2AssultUI>())
        {
            Debug.Log("We are changing");
            GameObject tempGameObject = BagInventory.instance.slot1.assultPrefab;
            BagInventory.instance.SetSlot1Assult(BagInventory.instance.slot2.assultPrefab);
            BagInventory.instance.SetSlot2Assult(tempGameObject);

        }
    }
}
