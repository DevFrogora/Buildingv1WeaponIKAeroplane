using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDotSight : MonoBehaviour, IInventoryItem
{
    public int id;
    public ItemType itemtype;
    public string itemName;
    public Sprite itemImage;
    public int ItemId { get => id; set => id = value; }

    public string Name => itemName;

    public Sprite spriteImage => itemImage;

    public ItemType itemType { get => itemtype; set => itemtype = value; }

    public void OnPickup()
    {
        //throw new System.NotImplementedException();
    }
}
