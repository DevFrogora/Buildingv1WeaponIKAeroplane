using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour , IInventoryItem
{
    public int level;
    public int extendSize;

    public int id;
    public ItemType itemtype;
    public string itemName;
    public Sprite itemImage;


    public string Name => itemName;

    public Sprite spriteImage => itemImage;

    public int ItemId { get => id; set => id = value; }

    public ItemType itemType { get => itemtype; set => itemtype = value; }

    public void OnPickup()
    {
        Debug.Log("Its said you are picking up "+ itemName);
    }
}
