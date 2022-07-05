using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;

public class LinkToAeroPlane : MonoBehaviour
{
    public GameObject aeroPlaneCanvas;
    InputActionMap aeroplaneActionMap;
    Animator animator;

    InputAction jump;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //enabled = false;
    }

    private void OnEnable()
    {
        aeroPlaneCanvas.SetActive(true);
    }

    private void Start()
    {
        aeroplaneActionMap = ActionMapManager.playerInput.actions.FindActionMap(ActionMapManager.ActionMap.Aeroplane);
        Aeroplane.instance.totalPeople += Instance_totalPeople;
        GameManager.instance.changeActionMap += OnChangeActionMap;
    }

    private void OnChangeActionMap(string actionMap)
    {
        if (actionMap == ActionMapManager.ActionMap.Aeroplane)
        {
            animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.Aeroplane, 1);
            RegisterAction();
            Debug.Log("Player AeroPlayer Activate");
        }
        else
        {
            animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.Aeroplane, 0);
            UnRegisterActionMap();
        }
    }

    public TextMeshProUGUI RemainingPeople;
    // Start is called before the first frame update

    private void Instance_totalPeople(int countPeople)
    {
        RemainingPeople.text = "Ramaining People " + countPeople;
    }


    public void OnExitUiClick()
    {
        OnExit();
    }

    void OnExit()
    {
        Aeroplane.instance.onPlayerExit(this.gameObject);
        Debug.Log("jumping from plane");
    }


    private void OnDisable()
    {
        aeroPlaneCanvas.SetActive(false);
        UnRegisterActionMap();

    }
    void RegisterAction()
    {
        jump = aeroplaneActionMap["Jump"];
        jump.performed += Jump_performed;
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        OnExit();
    }

    void UnRegisterActionMap()
    {
        if(aeroplaneActionMap != null)
        aeroplaneActionMap.Disable();
    }
}
