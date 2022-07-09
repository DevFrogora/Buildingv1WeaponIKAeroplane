using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot1UIUpdater : MonoBehaviour
{
    public Image assult;
    public Image ExtendedMag;
    public Image Sight;
    public Image draggableImage;

    private void Start()
    {
        BagUIBroadcast.instance.slot1AssultAdded += SetAssult;
    }

    void SetAssult(GameObject item)
    {
        if(item == null)
        {
            assult.sprite = null;
            draggableImage.sprite = null;

        }
        else
        {
            draggableImage.GetComponent<DraggableItemUI>().itemPrefab = item;
            draggableImage.sprite = item.GetComponent<IInventoryItem>().spriteImage;
            assult.sprite = item.GetComponent<IInventoryItem>().spriteImage;
        }
    }
}
