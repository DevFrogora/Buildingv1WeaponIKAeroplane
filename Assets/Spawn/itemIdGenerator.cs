using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemIdGenerator : MonoBehaviour
{
    public static itemIdGenerator instance;
    private void Awake()
    {
        instance = this;
    }
    int i;
    public int GetId()
    {
        i = i + 1;
        return i;
    }
}
