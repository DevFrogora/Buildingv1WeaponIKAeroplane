using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GrabPickUI : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public Image image;

    public GameObject itemPrefab;
    public int itemId;

    public void onClick()
    {
        GrabInventory.instance.SetGrab(itemPrefab);
    }
}
