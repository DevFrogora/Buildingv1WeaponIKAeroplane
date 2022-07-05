using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAllItemUIAdder : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject InventoryItemUI;
    public Transform canvasTrasform;

    void Start()
    {
        InventoryItemUI.GetComponent<InventoryItemUI>().canvasTransform = canvasTrasform;
        BagUIBroadcast.instance.slotMixItemAdded += AddItemInSlot;
    }

    void AddItemInSlot(GameObject item)
    {
        GameObject ui = Instantiate(InventoryItemUI, transform);
        ui.GetComponent<InventoryItemUI>().itemPrefab = item;
        ui.GetComponent<InventoryItemUI>().itemName.text = item.GetComponent<IInventoryItem>().Name;
        ui.GetComponent<InventoryItemUI>().image.sprite = item.GetComponent<IInventoryItem>().spriteImage;
        ui.GetComponent<InventoryItemUI>().draggableImage.sprite = item.GetComponent<IInventoryItem>().spriteImage;
        ui.GetComponent<InventoryItemUI>().draggableImage.GetComponent<DraggableItemUI>().itemPrefab = item;

    }

}
