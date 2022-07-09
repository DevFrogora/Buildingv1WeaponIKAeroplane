using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabInventory : MonoBehaviour
{
    public static GrabInventory instance;

    private void Awake()
    {
        instance = this;
    }

    public Transform itemParent;
    public Transform activeWeaponParent;

    public GameObject grab;

    public void SetGrab(GameObject itemPrefab)
    {
        itemPrefab.gameObject.transform.SetParent(itemParent, false);
        itemPrefab.gameObject.transform.localPosition = new Vector3(0, 0, 0.303f); //was  vecto3.zero 
        grab = itemPrefab;
        GrabUIBroadCast.instance.GrabItemAdded(itemPrefab);
    }
}