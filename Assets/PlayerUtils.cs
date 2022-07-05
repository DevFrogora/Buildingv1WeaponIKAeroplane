using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtils : MonoBehaviour
{
    Rigidbody rigidBody;
    CapsuleCollider collider;
    CharacterAiming characterAiming;
    CinemachineCameraOffset cameraOffset;


    private void Start()
    {
        rigidBody =  GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        cameraOffset = GetComponentInChildren<CinemachineCameraOffset>();
        characterAiming = GetComponent<CharacterAiming>();
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

    
    public void JoinedPlaneSetting()
    {
        Coupling();
        Vector3 cameraOffsetForPlayer = new Vector3(-2.73f, 1.4f, -14.7f);
        if(characterAiming.toggleMouseLock == false)
        {
            characterAiming.ToggleMouseLock();
        }
        characterAiming.xAxis.Value = 175f;
        characterAiming.yAxis.Value = 14.2f;
        SetCameraOffset(cameraOffsetForPlayer );
    }





}
