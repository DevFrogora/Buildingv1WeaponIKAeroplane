using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrint : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void CallWhenGameobjectaActive(Transform playerpos, float distance,float maxDistanceRange)
    {
        transform.position = playerpos.position + Vector3.up * 3;
        transform.rotation = playerpos.rotation;
        StartCoroutine(FootPrintVisible(distance,maxDistanceRange));
    }
    

    IEnumerator FootPrintVisible(float distance, float maxDistanceRange)
    {
        float alphaValue = Mathf.InverseLerp(maxDistanceRange, 0, distance);
        //Debug.Log(alphaValue);
        spriteRenderer.color = new Color(1f, 0f, 0f, alphaValue);
        Debug.Log(" is detected!");
        yield return new WaitForSeconds(2);
        // make foot disable;
        gameObject.SetActive(false);
    }
}
