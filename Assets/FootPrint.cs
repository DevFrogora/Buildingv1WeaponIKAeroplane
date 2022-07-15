using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrint : MonoBehaviour
{

    public void CallWhenGameobjectaActive(Transform playerpos)
    {
        transform.position = playerpos.position + Vector3.up * 3;
        transform.rotation = playerpos.rotation;
        StartCoroutine(FootPrintVisible());
    }
    

    IEnumerator FootPrintVisible()
    {
        Debug.Log(" is detected!");
        yield return new WaitForSeconds(2);
        // make foot disable;
        gameObject.SetActive(false);
    }
}
