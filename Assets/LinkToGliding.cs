using UnityEngine;
using UnityEngine.InputSystem;

public class LinkToGliding : MonoBehaviour
{
    public GameObject parachuteBag;
    private InputActionMap glidingActionMap;
    InputAction move, openParachute;

    public GlidingUiControl glidingUiControl;
    Vector2 input;
    Animator animator;
    bool isGliding;
    public GameObject parachutePrefab;
    public GameObject paraChuteParent;

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
            parachuteBag = Instantiate(parachuteBag);
            parachuteBag.transform.localPosition = Vector3.zero;

            playerUtils.EnterGlidingSetting(parachuteBag);
            glidingUiControl.enabled = true;
        }
        else
        {
            animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.Gliding, 0);
            if(glidingUiControl != null) // this check is not needed , it is just because some Ui is disable for testing 
            {
                glidingUiControl.enabled = false;

            }
            UnRegisterActionMap();
        }
    }

    private void UnRegisterActionMap()
    {
        glidingActionMap.Disable();
    }

    void RegisterAction()
    {
        move = glidingActionMap["Move"];
        openParachute = glidingActionMap["F"];
        openParachute.performed += GlidingOpenParachute;
        move.canceled += Move_canceled;
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        FlyRightSpeed = 0;
    }

    bool isParachuteOpen = false;
    private void GlidingOpenParachute(InputAction.CallbackContext context)
    {
        //Debug.Log("Transform rotation of player : " + transform.rotation);
        OpenParachuteOnce();
    }

    public void OpenParachuteOnce()
    {
        if (!isParachuteOpen)
        {
            parachutePrefab = Instantiate(parachutePrefab, paraChuteParent.transform);
            parachutePrefab.transform.localPosition = Vector3.zero;
            animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.Gliding, 0);
            animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.ParaGliding, 1);
            isParachuteOpen = true;
            yaw = Camera.main.transform.rotation.eulerAngles.y;
            glidingUiControl.OpenParachute(false);
        }
    }

    void CloseParachuteOnce()
    {
        Destroy(parachuteBag.gameObject);
        Destroy(parachutePrefab.gameObject);
        animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.ParaGliding, 0);
        GameManager.instance.ChangeActionMap("Land");
        transform.rotation =  Quaternion.Euler(0, yaw, 0);
        playerUtils.ExitingGlidingSetting();
    }

    void Gliding_animation(Vector2 input)
    {
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
    }

    void Parachute_animation(Vector2 input)
    {
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
    }

    public float FlyForwardSpeed = 5;
    public float FlyRightSpeed = 3;
    public float defaultMaxDragSpeed = 1.5f;
    public float defaultMinDragSpeed = 0.8f;

    float dragSpeed = 5;
    //public float YawAmount = 120;
    public float pitch = 80;
    float yaw = 0;

    private void Update()
    {
        if (!playerUtils.isGliding) return;
        input = move.ReadValue<Vector2>();
        float altitude = playerUtils.DownRaycast();
        float speed = playerUtils.RigidBodySpeed();
        if (altitude != -1)
        {
            if (altitude <= 200 || isParachuteOpen)
            {
                // parachute
                OpenParachuteOnce();
                ParachuteFunction(input);
                if(altitude < 1.5)
                {
                    CloseParachuteOnce();

                }
            }
            else
            {
                //gliding
                GlidingFunction(input);
            }

            glidingUiControl.SetAltitude((int)altitude, altitude / 1000);
            glidingUiControl.SetSpeed((int)speed, speed / 13);
        }



    }


    void GlidingFunction(Vector2 input)
    {
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
        transform.position += ((transform.up * FlyForwardSpeed) + (transform.right * FlyRightSpeed)) * Time.deltaTime;
        //transform.position += (transform.forward * FlyForwardSpeed + transform.up * (-FlyDownwardSpeed)) * Time.deltaTime;
        playerUtils.SetDrag(dragSpeed);

        float yawCamera = Camera.main.transform.rotation.eulerAngles.y;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(pitch, yawCamera, 0), turnSpeed * Time.deltaTime);
    }

    void ParachuteFunction(Vector2 input)
    {
        Parachute_animation(input);
        if (input.y > 0)
        {
            FlyForwardSpeed += 10 * Time.deltaTime;
            FlyForwardSpeed = FlyForwardSpeed > 30 ? 30 : FlyForwardSpeed;

            dragSpeed -= 2 * Time.deltaTime;
            dragSpeed = dragSpeed < defaultMinDragSpeed ? defaultMinDragSpeed : dragSpeed;
            pitch = 70;
        }

        if(input.y == 0)
        {
            FlyForwardSpeed -= 5f * Time.deltaTime;
            FlyForwardSpeed = FlyForwardSpeed < 0 ? 0 : FlyForwardSpeed;

            dragSpeed += 5f * Time.deltaTime;
            dragSpeed = dragSpeed > defaultMaxDragSpeed ? defaultMaxDragSpeed : dragSpeed;
            pitch = 0;

        }

        if (input.y < 0)
        {
            FlyForwardSpeed -= 10f * Time.deltaTime;
            FlyForwardSpeed = FlyForwardSpeed < 0 ? 0 : FlyForwardSpeed;

            dragSpeed += 5f * Time.deltaTime;
            dragSpeed = dragSpeed > defaultMaxDragSpeed ? defaultMaxDragSpeed : dragSpeed;
            pitch = 0;

        }

        if (input.x > 0)
        {

            FlyRightSpeed += 3 * Time.deltaTime;
            FlyRightSpeed = FlyRightSpeed > 3 ? 3 : FlyRightSpeed;
            yaw += 40 * Time.deltaTime;
        }


        if (input.x < 0)
        {
            FlyRightSpeed -= 3 * Time.deltaTime;
            FlyRightSpeed = FlyRightSpeed > -3 ? -3 : FlyRightSpeed;
            yaw -= 40 * Time.deltaTime;
        }

        //if (input == Vector2.zero)
        //{
        //    FlyForwardSpeed -= 5f * Time.deltaTime;
        //    FlyForwardSpeed = FlyForwardSpeed < 0 ? 0 : FlyForwardSpeed;

        //    dragSpeed += 5f * Time.deltaTime;
        //    dragSpeed = dragSpeed > defaultMaxDragSpeed ? defaultMaxDragSpeed : dragSpeed;
        //    //yaw = 0;
        //    pitch = 0;
        //}

        //Move Forward
        transform.position += ((transform.forward * FlyForwardSpeed) + (transform.right * FlyRightSpeed)) * Time.deltaTime;
        //transform.position += (transform.forward * FlyForwardSpeed + transform.up * (-FlyDownwardSpeed)) * Time.deltaTime;
        playerUtils.SetDrag(dragSpeed);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(pitch, yaw, 0), turnSpeed * Time.deltaTime);
    }
    public float turnSpeed = 15;
    private void OnDisable()
    {
        UnRegisterActionMap();
    }
}
