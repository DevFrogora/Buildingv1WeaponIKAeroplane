using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem 
{
    public int ItemId { get; set; }
    string Name { get; }
    Sprite spriteImage { get;  }

    ItemType itemType { get; }

    void OnPickup();
}

public enum ItemType
{
    Assult,
    Pistol,
    Explosive,
    Ammo,
};