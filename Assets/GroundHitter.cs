using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GroundHitter : MonoBehaviour
{
    public static GroundHitter instance;
    private void Awake()
    {
        instance = this;
    }
    //public event Action<Vector3?> groundPosition;


    //public void GetGroundPosition()
    //{
    //    groundPosition?.Invoke(HitGround());
    //}

    public float distance = 100f;
    public Vector3? HitGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
        {
            /*
             * Set the target location to the location of the hit.
             */
            return hit.point;
        }
        return null;
    }
}
