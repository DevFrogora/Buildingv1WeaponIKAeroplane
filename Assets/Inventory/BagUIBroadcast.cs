using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BagUIBroadcast : MonoBehaviour
{
    public static BagUIBroadcast instance;
    private void Awake()
    {
        instance = this;
    }

    public event Action<GameObject> slot1AssultAdded,
    slot2AssultAdded,slot1SightAdded,slotMixItemAdded;

    public event Action<int,bool> activeSlot, droppedSlot;

    public event Action<WeaponShotType.ShotType> slot1ShotType, slot2ShotType;

    public event Action<string> slot1AmmoTextUpdate;

    public event Action<float, String , bool> reloadAmmo;

    public void ReloadAammoUIUpdater(float fillAmount,string text, bool visibility)
    {
        reloadAmmo?.Invoke(fillAmount, text, visibility);
    }

    public void Slot1AmmoTextUpdate(string text)
    {
        slot1AmmoTextUpdate?.Invoke(text);
    }

    public void Slot1AssultAdded(GameObject item)
    {
        slot1AssultAdded?.Invoke(item);
    }

    public void Slot1SightAdded(GameObject item)
    {
        slot1SightAdded?.Invoke(item);
    }

    public void Slot2AssultAdded(GameObject item)
    {
        slot2AssultAdded?.Invoke(item);
    }

    public void SlotMixItemAdded(GameObject item)
    {
        slotMixItemAdded?.Invoke(item);
    }

    public void ActiveSlot(int slotNumber,bool activeState)
    {
        activeSlot?.Invoke(slotNumber, activeState);
    }

    public void DroppedSlot(int slotNumber,bool activeState)
    {
        droppedSlot?.Invoke(slotNumber, activeState);
    }

    public void Slot1ShootType(WeaponShotType.ShotType shotType)
    {
        slot1ShotType?.Invoke(shotType);
    }
}
