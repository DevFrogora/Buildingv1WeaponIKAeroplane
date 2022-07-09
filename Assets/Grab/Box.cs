using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Box : MonoBehaviour, IGrab
{

    public int id;
    public ItemType itemtype;
    public string weaponName;
    public Sprite weaponImage;

    public Transform leftHandGrip;
    public Transform rightHandGrip;


    public string Name => weaponName;

    public Sprite spriteImage => weaponImage;

    public int ItemId { get => id; set => id = value; }

    public ItemType itemType { get => itemtype; set => itemtype = value; }

    public GameObject mesh;
    public void OnPickup()
    {

    }

    public event Action<bool> collideTriggerStatus;

    public void CollideTriggerStatus(bool status)
    {
        collideTriggerStatus?.Invoke(status);
    }


    private void OnTriggerEnter(Collider other)
    {
        CollideTriggerStatus(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Camera.main.transform.rotation.x > 0)
        {
            // when camera is watching down
            mesh.transform.localRotation = Quaternion.Euler(27.14f, 0, 0);

        }
        else
        {
            // when camera is watching up
            mesh.transform.localRotation = Quaternion.Euler(-27.14f, 0, 0);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        CollideTriggerStatus(false);
    }


    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(checkPoint.transform.position, radius);
    }



}