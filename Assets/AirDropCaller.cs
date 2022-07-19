using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDropCaller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Frame(1));
        StartCoroutine(Frame(3));

    }
    IEnumerator Frame(int second)
    {
        yield return new WaitForSeconds(second);
        AirDropManager.instance.CallDropLocation(transform);
    }


}
