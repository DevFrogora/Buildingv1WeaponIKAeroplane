using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class WeaponInHand : MonoBehaviour
{
    InputActionMap landActionMap;
    InputAction leftMouse,rightMouse,reload;
    public float aimDuration = 0.3f;

    public UnityEngine.Animations.Rigging.Rig handIK;
    public TwoBoneIKConstraint LeftHandIk;
    public TwoBoneIKConstraint RightHandIk;
    public Transform weaponPivot;


    public Rig aimLayer;
    // Start is called before the first frame update

    public static WeaponInHand instance;
    private void Awake()
    {
        instance = this;
        handIK.weight = 0f;
        aimLayer.weight = 1f;
        
    }

    private void Start()
    {
        landActionMap = ActionMapManager.playerInput.actions.FindActionMap(ActionMapManager.ActionMap.Land);
        GameManager.instance.changeActionMap += ChangeActionMap;
        BagUIBroadcast.instance.activeSlot += ActiveSlot;
        BagUIBroadcast.instance.droppedSlot += DroppedSlot;

        RegisterAction();
    }


    public GameObject activeWeapon;
    int activeSlotNumber;
    public m416 weaponScriptRef; // we have to make interface so same method work on different type of weapon

    private void LateUpdate()
    {
        if(activeWeapon != null)
        {
            if (weaponScriptRef.isFiring)
            {
                aimLayer.weight += Time.deltaTime / aimDuration;
                weaponScriptRef.shoot();
            }else if (reload.triggered)
            {
                weaponScriptRef.Reloading();
            }
            else
            {
                if(rightMouse.IsPressed())
                {

                //    aimLayer.weight += Time.deltaTime / aimDuration;
                //}
                //else
                //{
                //    aimLayer.weight -= Time.deltaTime / aimDuration;

                    // use it for zoom in to weapon sight
                }
            }
        }
        else
        {
            //handIK.weight = 0.0f;
        }
    }

    void ActiveSlot( int slotNumber,bool activeState)
    {
        if(slotNumber == 1  )
        {
            if(activeState)
            {
                activeSlotNumber = slotNumber;
                activeWeapon = BagInventory.instance.slot1.assultPrefab;
                weaponScriptRef = activeWeapon.GetComponent<m416>();
                weaponScriptRef.mouse = leftMouse;
                weaponScriptRef.reload = reload;
                weaponScriptRef.uiAmmoUpdater += WeaponScriptRef_uiUpdater;
                weaponScriptRef.shotTypeUpdater += WeaponScriptRef_shotTypeUpdater;
                weaponScriptRef.uiReloadUpdater += WeaponScriptRef_uiReloadUpdater;
                weaponScriptRef.OnPickup();
                handIK.weight = 1f;

                StartCoroutine(WeaponPosition());

            }
            else
            {
                activeWeapon = null;
                weaponScriptRef = null;
                handIK.weight = 0f;
            }



        }
    }


    IEnumerator WeaponPosition()
    {
        yield return new WaitForEndOfFrame();
        RightHandIk.data.target.position = (weaponScriptRef.rightHandGrip.position);
        RightHandIk.data.target.rotation = weaponScriptRef.rightHandGrip.rotation;

        LeftHandIk.data.target.position = weaponScriptRef.leftHandGrip.position;
        LeftHandIk.data.target.rotation = weaponScriptRef.leftHandGrip.rotation;
    }

    private void WeaponScriptRef_uiReloadUpdater(float fillAmount, string text, bool visibility)
    {
        BagUIBroadcast.instance.ReloadAammoUIUpdater(fillAmount, text , visibility);
    }

    private void WeaponScriptRef_shotTypeUpdater(WeaponShotType.ShotType shotType)
    {
        BagUIBroadcast.instance.Slot1ShootType(shotType);
    }

    private void WeaponScriptRef_uiUpdater(string text)
    {
        BagUIBroadcast.instance.Slot1AmmoTextUpdate(text);
    }

    void DroppedSlot(int slotNumber, bool activeState)
    {
        if (slotNumber == activeSlotNumber)
        {
            activeWeapon = null;
            handIK.weight = 0f;
        }
    }

    public void SetNextShotTYpe(WeaponShotType.ShotType _shotType)
    {
        if(activeWeapon != null)
        {
            weaponScriptRef.shotType = _shotType.Next();
            weaponScriptRef.ShotTypeUpdater(weaponScriptRef.shotType);
        }
    }


    void ChangeActionMap(string actionMap)
    {
        if (actionMap == ActionMapManager.ActionMap.Land)
        {
            RegisterAction();
            Debug.Log("Player Mouse Activate");
        }
        else
        {
            UnRegisterActionMap();
        }
    }

    void RegisterAction()
    {
        leftMouse = landActionMap["LeftMouse"];
        rightMouse = landActionMap["RightMouse"];
        reload = landActionMap["R"];

        leftMouse.performed += LeftMouse_performed;
        leftMouse.canceled += LeftMouse_canceled;
    }


    private void LeftMouse_canceled(InputAction.CallbackContext obj)
    {
        if (activeWeapon != null)
        {
            weaponScriptRef.StopFiring();
        }
    }

    private void LeftMouse_performed(InputAction.CallbackContext obj)
    {
        if(activeWeapon != null)
        {
            weaponScriptRef.StartFiring();

        }
    }

    private void OnDisable()
    {
        UnRegisterActionMap();
    }

    void UnRegisterActionMap()
    {
        landActionMap.Disable();
    }
}
