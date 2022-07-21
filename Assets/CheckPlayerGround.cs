using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerGround : MonoBehaviour
{
    public LayerMask WaterLayer;
    Swimming swimming;
    public GameObject Head;
    private void Start()
    {
        swimming = GetComponent<Swimming>();

    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = GroundHitter.instance.HitGround();
        if (hit.point.y < 16)
        {
            Debug.Log(" water Level");
            if (swimming.enabled == false)
            {
                if (transform.position.y < 14.45f)
                {

                    GameManager.instance.ChangeActionMap(ActionMapManager.ActionMap.Swimming);
                    Debug.Log("Switching to Swimming");
                }
                else
                {

                }

            }
            else
            {
                if (swimming.enabled == true)
                {
                    if (transform.position.y > 14.45)
                    {
                        GameManager.instance.ChangeActionMap(ActionMapManager.ActionMap.Land);
                    }
                }
            }
        }
    }
}
