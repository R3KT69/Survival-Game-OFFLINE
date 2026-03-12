using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    private Transform cameraTransform;
    private Vector3 moveDirection;
    public Crouching crouchstate;
    private float moveX, moveZ;
    private float currentSpeed = 0f;

    [Header("Movement Settings")]
    public float walkSpeed = 2.5f;
    public float sprintSpeed = 6f;
    public float jogSpeed = 4.5f;
    public float speedReduceFactor = 0.5f;
    public float jumpForce = 5f;
    //public float rotationSpeed = 8f;
    bool isSprinting = false;
    
    

    [Header("Ground Check Settings")]
    private readonly Collider[] groundHits = new Collider[4];
    public Transform feetTransformR;
    public Transform feetTransformL;
    public float sphereRadius = 0.3f;
    public LayerMask groundMask;
    //private static Collider[] groundHits = new Collider[10];

    [Header("Physics Settings")]
    public Vector3 velocity;
    public float gravity = -9.81f;
    private bool isGrounded;



    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        cameraTransform = gameObject.GetComponentInChildren<Camera>().transform;
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovementInput();
        CheckGroundStatus();
        Jump();
        ApplyGravity();
        MovePlayer();
        UpdateAnimations();
        //RotatePlayerTowardCamera(); (This is deprecated, used to work in third person view)
        //Debug.Log(currentSpeed);
    }

    public bool isMoving()
    {
        return moveDirection.sqrMagnitude > 0.01f;
    }

    

    void HandleMovementInput()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        moveDirection = transform.right * moveX + transform.forward * moveZ;

        if (moveDirection.magnitude > 1) moveDirection.Normalize();

        float targetSpeed = jogSpeed;

        if (animator.GetBool("isAiming"))
        {
            targetSpeed = walkSpeed;
            isSprinting = false;
        }
        else if (!crouchstate.isCrouching && Input.GetKey(KeyCode.LeftShift) && moveZ > 0f && Mathf.Abs(moveX) < 0.01f)
        {
            targetSpeed = sprintSpeed;
            isSprinting = true;
        }
        else if (crouchstate.isCrouching)
        {
            targetSpeed = walkSpeed;
            isSprinting = false;
        }
        else
        {
            targetSpeed = jogSpeed;
            isSprinting = false;
        }

         
        //float targetSpeed = animator.GetBool("isAiming") /*|| Input.GetKey(KeyCode.LeftShift)*/ ? walkSpeed : sprintSpeed; // Toggle Walk/Sprint
        //float targetSpeed = sprintSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 8f);
    }

    void MovePlayer()
    {
        Vector3 moveVelocity = moveDirection * currentSpeed;
        moveVelocity.y = velocity.y;
        characterController.Move(moveVelocity * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetTrigger("Jump");
        }
    }

    void ApplyGravity()
    {
        bool isCollidingBelow = (characterController.collisionFlags & CollisionFlags.Below) != 0;

        if (isCollidingBelow && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
    }



    void CheckGroundStatus()
    {
        isGrounded = GroundCheck(feetTransformR, feetTransformL, sphereRadius, groundMask);
        animator.SetBool("isGrounded", isGrounded);
    }
 

    bool GroundCheck(Transform feetTransformR, Transform feetTransformL, float sphereRadius, LayerMask groundMask)
    {
        int hitCountR = Physics.OverlapSphereNonAlloc(feetTransformR.position, sphereRadius, groundHits, groundMask);
        bool rightGrounded = hitCountR > 0;

        int hitCountL = Physics.OverlapSphereNonAlloc(feetTransformL.position, sphereRadius, groundHits, groundMask);
        bool leftGrounded = hitCountL > 0;

        return rightGrounded || leftGrounded;
    }


    void UpdateAnimations()
    {
        // --- Base input ---
        float targetAccel = moveZ;
        float targetHorizontal = moveX;

        // --- Normalize for diagonal movement ---
        if (Mathf.Abs(moveX) > 0.01f && Mathf.Abs(moveZ) > 0.01f)
        {
            float diagonalMag = Mathf.Sqrt(moveX * moveX + moveZ * moveZ);
            targetAccel = moveZ / diagonalMag;
            targetHorizontal = moveX / diagonalMag;
        }

        // --- Apply aiming speed reduction ---
        if (animator.GetBool("isAiming"))
        {
            targetAccel *= speedReduceFactor;
            targetHorizontal *= speedReduceFactor;
        }

        // --- Sprint override ---
        if (isSprinting)
            targetAccel = 1.5f;

        // --- Smooth interpolation ---
        float smoothAccel = Mathf.Lerp(animator.GetFloat("Acceleration"), targetAccel, Time.deltaTime * 10f);
        float smoothHorizontal = Mathf.Lerp(animator.GetFloat("Horizontal"), targetHorizontal, Time.deltaTime * 10f);

        animator.SetFloat("Acceleration", smoothAccel, 0.01f, Time.deltaTime);
        animator.SetFloat("Horizontal", smoothHorizontal, 0.01f, Time.deltaTime);
    }


    void OnDrawGizmos()
    {
        bool grounded = GroundCheck(feetTransformR, feetTransformL, sphereRadius, groundMask);

        if (feetTransformR == null) return;
        Gizmos.color = grounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(feetTransformR.position, sphereRadius);

        if (feetTransformL == null) return;
        Gizmos.color = grounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(feetTransformL.position, sphereRadius);
    }

    /* TpsController Deprecated Code
        void RotatePlayerTowardCamera()
        {
            Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (inputDir.sqrMagnitude > 0.01f)
            {
                // Camera-relative direction
                Vector3 camForward = cameraTransform.forward;
                camForward.y = 0;
                camForward.Normalize();

                Quaternion targetRotation = Quaternion.LookRotation(camForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    */

}
