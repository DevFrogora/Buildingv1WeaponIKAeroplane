using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot1SightUIUpdater : MonoBehaviour
{
    public Image sight;

    private void Start()
    {
        BagUIBroadcast.instance.slot1SightAdded += SetSight;
    }

    void SetSight(GameObject item)
    {
        if (item == null)
        {
            sight.sprite = null;

        }
        else
        {
            sight.GetComponent<DraggableSlot1Sight>().itemPrefab = item;
            sight.sprite = item.GetComponent<IInventoryItem>().spriteImage;
        }
    }
}
