using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLocomotion : MonoBehaviour
{
    public float jumpHeight;
    public float gravity;

    InputActionMap landActionMap;
    InputAction move,jump,sprint,peekLeft,peekRight;

    Animator animator;
    Vector2 input;

    Vector3 rootMotion;
    Vector3 velocity;
    bool isJumping;

    bool isGrounded;

    Rigidbody rigidBody;
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    //[SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 2f;

    [SerializeField] GameObject windowRayChecker;
    [SerializeField] GameObject windowLeftRayChecker;
    [SerializeField] GameObject windowRightRayChecker;


    //Camera Position on Car and plane or land or water
    //public CinemachineCameraOffset camera;
    //default Vector3(0.38,-0.15,-0.84)
    public Vector3 defaultCameraPos = new Vector3(0.38f, -0.15f, -0.84f);


    CharacterAiming characterAiming;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        characterAiming = GetComponent<CharacterAiming>();
        //stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);
    }


    void Start()
    {
        animator = GetComponent<Animator>();
        landActionMap = ActionMapManager.playerInput.actions.FindActionMap(ActionMapManager.ActionMap.Land);

        GameManager.instance.changeActionMap += ChangeActionMap;
        RegisterAction();
        
    }

    void ChangeActionMap(string actionMap)
    {
        if(actionMap == ActionMapManager.ActionMap.Land)
        {
            animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.Land, 1);
            animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.Land2, 1);
            RegisterAction();
            Debug.Log("Player Land Activate");
        }
        else
        {
            animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.Land, 0);
            animator.SetLayerWeight((int)AnimatorManager.AnimatorLayer.Land2, 0);

            UnRegisterActionMap();
        }
    }

    void RegisterAction()
    {
        move = landActionMap["Move"];
        jump = landActionMap["Jump"];
        sprint = landActionMap["Sprint"];

        peekLeft = landActionMap["Q"];
        peekRight = landActionMap["E"];
        peekLeft.performed += PeekLeft_performed;
        peekRight.performed += PeekRight_performed;
        sprint.performed += sprintingPerformed;
        sprint.canceled += sprintingCanceled;
    }

    private void PeekRight_performed(InputAction.CallbackContext obj)
    {
        peekLeftBool = false;
        peekRighttBool = !peekRighttBool;  //toggle
        animator.SetBool("Q", peekLeftBool);
        animator.SetBool("E", peekRighttBool);
    }

    bool peekLeftBool = false;
    bool peekRighttBool = false;

    private void PeekLeft_performed(InputAction.CallbackContext obj)
    {
        peekRighttBool = false;
        peekLeftBool = !peekLeftBool;  //toggle
        animator.SetBool("E", peekRighttBool);
        animator.SetBool("Q", peekLeftBool);
    }

    bool isSprinting;
    private void sprintingPerformed(InputAction.CallbackContext obj)
    {
        isSprinting = true;
    }

    private void sprintingCanceled(InputAction.CallbackContext obj)
    {
        isSprinting = false;

    }
    #region Animation
    void move_animation(Vector2 input)
    {
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
    }
    #endregion

    private void Update()
    {
        input = move.ReadValue<Vector2>();
        move_animation(input*2);

        GroundCheck();

        if (jump.triggered)
        {
            JumpDecider();
        }

        UpdateSprinting();


        animatorStateHandler();
    }

    void UpdateSprinting()
    {
        animator.SetBool("BtnShift", isSprinting);
    }

    void animatorStateHandler()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("wallJump"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.98f)
            {
                GetComponent<CapsuleCollider>().enabled = true;
                rigidBody.useGravity = true;
                characterAiming.enabled = true;


            }
        }
    }

    void JumpDecider()
    {
        //// Wall Jump Code
        //animator.SetTrigger("IsWallJump");
        //GetComponent<CapsuleCollider>().enabled = false;

        // normal Jump
        //Jump();
        PlayerRayCastChecker checker = checkForWindows();
        if (checker == PlayerRayCastChecker.none || checker == PlayerRayCastChecker.wall)
        {
            //normal Jump
            Jump();
            Debug.Log("normal Jump");

        }
        else if (checker == PlayerRayCastChecker.window)
        {
            characterAiming.enabled = false;
            Debug.Log("wall Jump");
            rigidBody.useGravity = false;
            animator.Play("wallJump");
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    enum PlayerRayCastChecker
    {
        none,
        wall,
        window
    };

    PlayerRayCastChecker checkForWindows()
    {

        // future work use overlay Sphere cast to detect window;
        RaycastHit hitLower;

        if (Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.2f))
        {

            if (hitLower.collider.tag == "wall")
            {
                Debug.Log("It is wall");
                RaycastHit hitUpper;
                RaycastHit hitLUpper;
                RaycastHit hitRUpper;


                if (Physics.Raycast(windowRayChecker.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f)
                    || Physics.Raycast(windowLeftRayChecker.transform.position, transform.TransformDirection(Vector3.forward), out hitLUpper, 0.2f)
                    || Physics.Raycast(windowRightRayChecker.transform.position, transform.TransformDirection(Vector3.forward), out hitRUpper, 0.2f)
                    )
                {
                    if (hitUpper.collider.tag == "wall" )
                    {
                        //|| hitLUpper.collider.tag == "wall" || hitRUpper.collider.tag == "wall"
                        Debug.Log("We don't have to windows Jump");
                    }
                    return PlayerRayCastChecker.wall;

                }
                else
                {
                    Debug.Log("We have to windows Jump");
                    return PlayerRayCastChecker.window;
                }

            }

        }
        
            return PlayerRayCastChecker.none;

        
    }


    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
        
    }

    bool oneJump;
    private void FixedUpdate()
    {
        if(isJumping)
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
            Move(velocity * Time.fixedDeltaTime);
            rootMotion = Vector3.zero;
            if (oneJump)
            {
                StartCoroutine(GroundCheckForJump());
            }
            else
            {
                
                isJumping = !isGrounded;
            }
            animator.SetBool("BtnSpace", isJumping);


        }
        else
        { //on ground

                Move(new Vector3(rootMotion.x, 0, rootMotion.z));
                rootMotion = Vector3.zero;

                stepClimb();
        }

    }

    IEnumerator GroundCheckForJump()
    {
        yield return new WaitForSeconds(0.17f);
        isJumping = !isGrounded;
        animator.SetBool("BtnSpace", isJumping);

        oneJump = false;

    }

    void Jump()
    {
        if(!isJumping)
        {
            isJumping = true;
            velocity = animator.velocity;
            velocity.y = Mathf.Sqrt(2 * gravity * jumpHeight);
            oneJump = true;
        }
    }


    void Move(Vector3 move)
    {
        rigidBody.MovePosition(transform.position + move);

    }
    

    void CheckJumpAnimationDoneOrNot()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("IsJumping"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
            {
                Debug.Log("jumping Finished");
            }

        }
    }

    public Vector3 stepOffset = new Vector3(0, 0.1f,0);
    void GroundCheck()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position  + stepOffset;
        if (Physics.Raycast(rayCastOrigin, -transform.up,out hit))
        {
            //Debug.DrawLine(rayCastOrigin, hit.point, Color.green);
            if(hit.distance < (stepOffset.y + 0.2f))
            {
                //transform.position = hit.point + stepOffset;
                //rigidBody.velocity = Vector3.zero;
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }

    void stepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
            {
                if(hitLower.collider.tag == "stair")
                {
                    Debug.Log("i am trying to climb up");
                    rigidBody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
                }

            }
        }

        //RaycastHit hitLower45;
        //if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f))
        //{

        //    RaycastHit hitUpper45;
        //    if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.2f))
        //    {
        //        if (hitLower45.collider.tag == "stair")
        //        {
        //            rigidBody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
        //        }
        //    }
        //}

        //RaycastHit hitLowerMinus45;
        //if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.1f))
        //{

        //    RaycastHit hitUpperMinus45;
        //    if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 0.2f))
        //    {
        //        if (hitLowerMinus45.collider.tag == "stair")
        //        {
        //            rigidBody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
        //        }
        //    }
        //}
    }



    #region DisableOrUnregister
    private void OnDisable()
    {
        UnRegisterActionMap();
    }

    void UnRegisterActionMap()
    {
        landActionMap.Disable();
    }
    #endregion

}
