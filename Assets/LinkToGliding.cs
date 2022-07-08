using UnityEngine;
using UnityEngine.InputSystem;

public class LinkToGliding : MonoBehaviour
{
    private InputActionMap glidingActionMap;
    InputAction move,open;

    GlidingUiControl glidingUiControl;
    Vector2 input;
    Animator animator;
    bool isGliding;
    public GameObject parachutePrefab;

    PlayerUtils playerUtils;

    private void Start()
    {
        glidingUiControl = GetComponentInChildren<GlidingUiControl>();
        playerUtils = GetComponent<PlayerUtils>();
        animator = GetComponent<Animator>();
        glidingActionMap = ActionMapManager.playerInput.actions.FindActionMap(ActionMapManager.ActionMap.Gliding);
        GameManager.instance.changeActionMap += OnChangeActionMap;
        RegisterAction();
    }

    private void OnChangeActionMap(string actionMap)
    {
        if (actionMap == ActionMapManager.ActionMap.Gliding)
        {
            animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.Gliding, 1);
            RegisterAction();
            //Debug.Log("Player AeroPlayer Activate");
            glidingUiControl.enabled = true;
        }
        else
        {
            animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.Gliding, 0);
            UnRegisterActionMap();
            glidingUiControl.enabled = false;
        }
    }

    private void UnRegisterActionMap()
    {
        glidingActionMap.Disable();
    }

    void RegisterAction()
    {
        move = glidingActionMap["Move"];
        open = glidingActionMap["Open"];
        open.performed += GlidingOpenParachute;
        move.canceled += Move_canceled;
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        FlyRightSpeed = 0;
    }

    private void GlidingOpenParachute(InputAction.CallbackContext context)
    {
        //Debug.Log("Transform rotation of player : " + transform.rotation);
        parachutePrefab = Instantiate(parachutePrefab, transform.position, transform.localRotation);
    }

    void Gliding_animation(Vector2 input)
    {
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
    }

    public float FlyForwardSpeed = 5;
    public float FlyRightSpeed = 3;
    public float defaultMaxDragSpeed = 1.5f;
    public float defaultMinDragSpeed = 0.8f;

    float dragSpeed = 5;
    public float YawAmount = 120;
    public float Yaw;
    public float pitch = 80;

    private void Update()
    {
        if (!playerUtils.isGliding) return;
        input = move.ReadValue<Vector2>();
        Gliding_animation(input);
        if (input.y > 0)
        {
            FlyForwardSpeed += 10 * Time.deltaTime;
            FlyForwardSpeed = FlyForwardSpeed > 30 ? 30 : FlyForwardSpeed;

            dragSpeed -= 2 * Time.deltaTime;
            dragSpeed = dragSpeed < defaultMinDragSpeed ? defaultMinDragSpeed : dragSpeed;
            pitch = 110;
        }

        if (input.x > 0)
        {
            
            FlyRightSpeed += 3 * Time.deltaTime;
            FlyRightSpeed = FlyRightSpeed > 3 ? 3 : FlyRightSpeed;
        }


        if (input.x < 0)
        {
            FlyRightSpeed -= 3 * Time.deltaTime;
            FlyRightSpeed = FlyRightSpeed > -3 ? -3 : FlyRightSpeed;
        }

        if (input == Vector2.zero)
        {
            FlyForwardSpeed -= 5f * Time.deltaTime;
            FlyForwardSpeed = FlyForwardSpeed < 0 ? 0 : FlyForwardSpeed;

            dragSpeed += 5f * Time.deltaTime;
            dragSpeed = dragSpeed > defaultMaxDragSpeed ? defaultMaxDragSpeed : dragSpeed;

            pitch = 80;
        }

        //Move Forward
        transform.position += ((transform.up * FlyForwardSpeed)  + (transform.right * FlyRightSpeed)) * Time.deltaTime;
        //transform.position += (transform.forward * FlyForwardSpeed + transform.up * (-FlyDownwardSpeed)) * Time.deltaTime;
        playerUtils.SetDrag(dragSpeed);
        //transform.rotation = Quaternion.Euler(70, 0,0);
        float yawCamera = Camera.main.transform.rotation.eulerAngles.y;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(pitch, yawCamera, 0), turnSpeed * Time.deltaTime);
        float altitude = playerUtils.DownRaycast();
        float speed = playerUtils.RigidBodySpeed();
        glidingUiControl.SetAltitude((int) altitude, altitude / 1000);
        glidingUiControl.SetSpeed((int)speed, speed / 13);
    }
    public float turnSpeed = 15;
    private void OnDisable()
    {
        UnRegisterActionMap();
    }
}
