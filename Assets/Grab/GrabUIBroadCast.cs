using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GrabUIBroadCast : MonoBehaviour
{
    public static GrabUIBroadCast instance;
    private void Awake()
    {
        instance = this;
    }

    public event Action<GameObject> grabItemAdded;

    public void GrabItemAdded(GameObject item)
    {
        grabItemAdded?.Invoke(item);
    }

}
