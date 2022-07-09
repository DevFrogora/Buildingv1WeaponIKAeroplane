using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15;
    Camera mainCamera;
    InputActionMap landActionMap,playerActionMap;
    InputAction escape;
    InputAction alt;

    public Transform cameraLookAt;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    public bool toggleMouseLock;
    public bool lockCharacterRotationViaCamera;

    private void Awake()
    {
        xAxis.SetInputAxisProvider(0, GetComponent<Cinemachine.CinemachineInputProvider>());
        yAxis.SetInputAxisProvider(1, GetComponent<Cinemachine.CinemachineInputProvider>());
    }

    void Start()
    {
        landActionMap = ActionMapManager.playerInput.actions.FindActionMap(ActionMapManager.ActionMap.Land);
        playerActionMap = ActionMapManager.playerInput.actions.FindActionMap(ActionMapManager.ActionMap.Player);
        playerActionMap.Enable();
        mainCamera = Camera.main;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        GameManager.instance.changeActionMap += ChangeActionMap;
        RegisterAction();

    }

    void ChangeActionMap(string actionMap)
    {
        if (actionMap == ActionMapManager.ActionMap.Land)
        {
            RegisterAction();
            Debug.Log("Player Land Activate");
        }
        else
        {
            UnRegisterActionMap();
        }
    }

    void RegisterAction()
    {
        escape = playerActionMap["Esc"];
        alt = landActionMap["Alt"];
        escape.performed += escapePerformed;

    }

    private void escapePerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Escape");

        ToggleMouseLock();
    }

    public void ToggleMouseLock()
    {
        toggleMouseLock = !toggleMouseLock;
        if (toggleMouseLock)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void UnRegisterActionMap()
    {
        landActionMap.Disable();
    }

    void FixedUpdate()
    {

        //if (!toggleMouseLock) return;
        //xAxis.Update(Time.fixedDeltaTime);
        //yAxis.Update(Time.fixedDeltaTime);

        //cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        //float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        //if(!alt.IsPressed())
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {

        if (!toggleMouseLock) return;
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        if (lockCharacterRotationViaCamera) return;
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        if (!alt.IsPressed())
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.deltaTime);
    }
}

