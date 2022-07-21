using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Swimming : MonoBehaviour
{
    InputActionMap swimActionMap;
    Animator animator;
    Rigidbody playerRb;
    InputAction  move,swimUp,swimDown;
    Vector2 input;

    PlayerUtils playerUtils;

    // Start is called before the first frame update
    void Start()
    {
        playerUtils = GetComponent<PlayerUtils>();
        playerRb = GetComponent<Rigidbody>();
        originalConstraint = playerRb.constraints;
        animator = GetComponent<Animator>();
        swimActionMap = ActionMapManager.playerInput.actions.FindActionMap(ActionMapManager.ActionMap.Swimming);

        GameManager.instance.changeActionMap += OnChangeActionMap;
        RegisterAction();

        _defFogMode = RenderSettings.fogMode;
        _defFogDensity = RenderSettings.fogDensity;
        _defFogColor = RenderSettings.fogColor;
        _defFogEnabled = RenderSettings.fog;

        _fogColorWater = new Color(0.2f, 0.65f, 0.75f, 0.5f);

    }
    RigidbodyConstraints originalConstraint;
    private void OnChangeActionMap(string actionMap)
    {
        if (actionMap == ActionMapManager.ActionMap.Swimming)
        {
            originalConstraint = playerRb.constraints;
            playerRb.constraints = RigidbodyConstraints.FreezePositionY;
            playerRb.constraints = ~RigidbodyConstraints.FreezeRotation;
            animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.Swimming, 1);
            playerUtils.EnterSwimming();
            RegisterAction();

        }
        else
        {
            animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.Swimming, 0);
            playerRb.constraints = originalConstraint;
            playerUtils.ExitSwimming();
            UnRegisterActionMap();
        }
    }

    private void UnRegisterActionMap()
    {
        swimActionMap.Disable();
    }

    private void RegisterAction()
    {
        move = swimActionMap["Move"];
        swimUp = swimActionMap["Space"];
        swimDown = swimActionMap["Shift"];
        animator.SetBool("BtnShift", false);
        animator.SetBool("BtnSpace", false);

    }


    float currentYPos = 14.45f;
    public float speed;


    public float turnSpeed = 1;
    float roll;
    float pitch;

    public Vector3 moveForwardZ ;
    public Vector3 moveRightX;
    public Vector3 moveUpY;



    private bool _defFogEnabled;
    private FogMode _defFogMode;
    private float _defFogDensity = 0;
    private Color _defFogColor = Color.black;

    public Color _fogColorWater = new Color(0.2f, 0.65f, 0.75f, 0.5f);
    public float fogDensity = 0.075f;

    private bool IsUnderwater()
    {
        return Camera.main.transform.position.y <= 15.75;
    }

    private void SetRenderDiving()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = _fogColorWater;
        RenderSettings.fogDensity = 0.1f;
        RenderSettings.fogMode = FogMode.Exponential;
    }

    private void SetRenderDefault()
    {
        RenderSettings.fog = _defFogEnabled;
        RenderSettings.fogColor = _defFogColor;
        RenderSettings.fogDensity = _defFogDensity;
        RenderSettings.fogMode = _defFogMode;
    }

    private void FixedUpdate()
    {
        if(IsUnderwater())
        {
            SetRenderDiving();
        }
        else
        {
            SetRenderDefault();
        }

        input = move.ReadValue<Vector2>();
        moveUpY = Vector3.zero;
        if (input.y > 0)
        {
            playerRb.constraints = RigidbodyConstraints.FreezePositionY;
            moveForwardZ =  transform.forward * speed * Time.fixedDeltaTime;
            animator.SetFloat("InputY", 1);
            pitch = 24;
        }
        if (input.y == 0)
        {
            pitch = 0;
            animator.SetFloat("InputY", 0);
            moveForwardZ = Vector3.zero;
        }
        if (input.y < 0)
        {
            playerRb.constraints = RigidbodyConstraints.FreezePositionY;
            pitch = -40;
            animator.SetFloat("InputY", -1);
            moveForwardZ = (-transform.forward) * speed * Time.fixedDeltaTime;
        }

        if (input.x > 0)
        {
            playerRb.constraints = RigidbodyConstraints.FreezePositionY;
            moveRightX =  transform.right * speed * Time.fixedDeltaTime;
            animator.SetFloat("InputX", 1);
            roll = -24;
        }

        if(input.x == 0)
        {
            roll = 0;
            animator.SetFloat("InputX", 0);
            moveRightX = Vector3.zero;
        }


        if (input.x < 0)
        {
            playerRb.constraints = RigidbodyConstraints.FreezePositionY;
            moveRightX =  (-transform.right) * speed * Time.fixedDeltaTime;
            animator.SetFloat("InputX", -1);
            roll = 24;
        }



        if (input == Vector2.zero)
        {
            pitch = 0;
            roll = 0;

            playerRb.constraints = RigidbodyConstraints.FreezePosition;

            animator.SetFloat("InputX", 0);
            animator.SetFloat("InputY", 0);
        }


        if(swimDown.IsPressed())
        {
            playerRb.constraints = ~RigidbodyConstraints.FreezePosition;

            moveUpY = (-transform.up) * speed * Time.fixedDeltaTime;
        }

        if (swimUp.IsPressed())
        {
            if(transform.position.y < 14.441)
            {
                playerRb.constraints = ~RigidbodyConstraints.FreezePosition;
                moveUpY = (transform.up) * speed * Time.fixedDeltaTime;
            }

        }

        playerRb.MovePosition(transform.position + moveForwardZ + moveRightX + moveUpY);

        float yawCamera = Camera.main.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(pitch, yawCamera, roll), turnSpeed * Time.deltaTime);
    }
}
