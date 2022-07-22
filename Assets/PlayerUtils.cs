using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtils : MonoBehaviour
{
    Rigidbody rigidBody;
    CapsuleCollider collider;
    CharacterAiming characterAiming;
    CinemachineCameraOffset cameraOffset;

    public bool isGliding;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        cameraOffset = GetComponentInChildren<CinemachineCameraOffset>();
        characterAiming = GetComponent<CharacterAiming>();
    }

    public void SetDrag(float drag)
    {
        rigidBody.drag = drag;
    }


    void setGravity(bool condition)
    {
        rigidBody.useGravity = condition;
    }

    void setKinematic(bool condition)
    {
        rigidBody.isKinematic = condition;
    }

    void setCollider(bool condition)
    {
        collider.enabled = condition;
    }

    public void Coupling()
    {
        setCollider(false);
        setGravity(false);
        setKinematic(true);
    }

    public void DeCoupling()
    {
        setCollider(true);
        setGravity(true);
        setKinematic(false);
    }

    public void SetCameraOffset(Vector3 offset)
    {
        cameraOffset.m_Offset = offset;
    }


    public void JoinedPlaneSetting(GameObject bag)
    {
        Coupling();
        Vector3 cameraOffsetForPlayer = new Vector3(-2.73f, 1.4f, -14.7f);
        if (characterAiming.toggleMouseLock == false)
        {
            characterAiming.ToggleMouseLock();
        }
        characterAiming.xAxis.Value = 175f;
        characterAiming.yAxis.Value = 14.2f;
        //BagInventory.instance.SetBag(bag);
        SetCameraOffset(cameraOffsetForPlayer);
    }



    public void ExitPlaneSetting()
    {
        DeCoupling();
        //Vector3 cameraOffsetForPlayer = Vector3.zero;
        Vector3 cameraOffsetForPlayer = new Vector3(0, 0, -1.32f);
        if (characterAiming.toggleMouseLock == false)
        {
            characterAiming.ToggleMouseLock();
        }
        SetCameraOffset(cameraOffsetForPlayer);
        //EnterGlidingSetting();
    }


    public void EnterGlidingSetting(GameObject bag)
    {
        DeCoupling();
        Vector3 cameraOffsetForPlayer = new Vector3(0, 0, -2.32f);
        if (characterAiming.toggleMouseLock == false)
        {
            characterAiming.ToggleMouseLock();
        }
        SetCameraOffset(cameraOffsetForPlayer);
        BagInventory.instance.SetBag(bag);
        characterAiming.lockCharacterRotationViaCamera = true;
        isGliding = true;
    }

    public void ExitingGlidingSetting()
    {
        characterAiming.lockCharacterRotationViaCamera = false;
        Vector3 cameraOffsetForPlayer = Vector3.zero;
        if (characterAiming.toggleMouseLock == false)
        {
            characterAiming.ToggleMouseLock();
        }
        SetCameraOffset(cameraOffsetForPlayer);
    }


    public LayerMask GroundLayer;
    RaycastHit hit;
    public float DownRaycast()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000, GroundLayer))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            float altitude = Vector3.Distance(transform.position, hit.point);
            return altitude;
        }
        return -1;
    }

    public float RigidBodySpeed()
    {
        return rigidBody.velocity.magnitude;
    }

    public void EnterSwimming()
    {
        characterAiming.lockCharacterRotationViaCamera = true;
    }

    public void ExitSwimming()
    {
        characterAiming.lockCharacterRotationViaCamera = false;
    }

    public void EnterSleeping()
    {
        characterAiming.lockCharacterRotationViaCamera = true;

        Vector3 cameraOffsetForPlayer = new Vector3(0, 0.7f, -3f);
        if (characterAiming.toggleMouseLock == false)
        {
            characterAiming.ToggleMouseLock();
        }
        SetCameraOffset(cameraOffsetForPlayer);
    }

    public void ExitSleeping()
    {
        characterAiming.lockCharacterRotationViaCamera = false;
        Vector3 cameraOffsetForPlayer = Vector3.zero;
        if (characterAiming.toggleMouseLock == false)
        {
            characterAiming.ToggleMouseLock();
        }
        SetCameraOffset(cameraOffsetForPlayer);
    }
}