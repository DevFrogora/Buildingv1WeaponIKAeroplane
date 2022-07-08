using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PickerItemUI : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public Image image;

    public GameObject itemPrefab;
    public int itemId;

    public void onClick()
    {
        //its UI For PickupItem

        if (itemPrefab.GetComponent<m416>())
        {
            if(!BagInventory.instance.activeSlot1 && !BagInventory.instance.activeSlot2)
            {
                if (BagInventory.instance.slot1.assultPrefab == null && BagInventory.instance.slot1.assultPrefab == null)
                {
                    BagInventory.instance.SetSlot1Assult(itemPrefab);

                }else if (BagInventory.instance.slot1.assultPrefab != null && BagInventory.instance.slot2.assultPrefab == null)
                {
                    BagInventory.instance.SetSlot2Assult(itemPrefab); // equip the item
                                                           
                }
                else if (BagInventory.instance.slot1.assultPrefab != null && BagInventory.instance.slot2.assultPrefab != null)
                {
                    BagInventory.instance.SetSlot1Assult(itemPrefab); // equip the item
                                                                      // drop the item of slot 1
                }
            }
            else
            {
                if (BagInventory.instance.activeSlot1)
                {
                    // Cloned it and Destory it or pickup the this prefab;
                    BagInventory.instance.SetSlot1Assult(itemPrefab);
                }
                if (BagInventory.instance.activeSlot2)
                {
                    BagInventory.instance.SetSlot2Assult(itemPrefab);
                }
            }
        }
        else if (itemPrefab.GetComponent<RedDotSight>())
        {
            BagInventory.instance.AddInMixItem(itemPrefab);
        }else if(itemPrefab.GetComponent<Bag>())
        {
            BagInventory.instance.SetBag(itemPrefab);
        }

    }
}
