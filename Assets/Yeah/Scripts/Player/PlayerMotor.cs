using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] public float walkSpeed = 0f;
    [SerializeField] public float sprintSpeed = 0f;
    [SerializeField] public float jumpForce = 0f;
    [SerializeField] public bool isGrounded = false;
    [SerializeField] public bool isWalking = false;
    [SerializeField] public bool isSprinting = false;
    
    private PlayerMovement playerInput = null;
    public CharacterController characterController { get; private set; }
    private float currentSpeed = 0f;

    //movement parameters
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float rayLength = 0.5f;
    private Vector2 moveVector;
    private Vector3 playerVelocity;

    public Action OnJump;

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, new Vector3(0, -1, 0) * rayLength);
    }

    #endregion

    void Awake()
    {
        playerInput = new PlayerMovement();
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        CheckIsGrounded();

        Move(moveVector);
    }

    private void OnEnable()
    {
        playerInput.OnFoot.Enable();
        playerInput.OnFoot.Movement.performed += OnMovementPerformed;
        playerInput.OnFoot.Movement.canceled += OnMovementCanceled;
        playerInput.OnFoot.Jump.performed += Jump;
        playerInput.OnFoot.Sprint.started += SprintStarted;
        playerInput.OnFoot.Sprint.canceled += SprintCanceled;
    }

    private void OnDisable()
    {
        playerInput.OnFoot.Disable();
        playerInput.OnFoot.Movement.performed -= OnMovementPerformed;
        playerInput.OnFoot.Movement.canceled -= OnMovementCanceled;
        playerInput.OnFoot.Jump.performed -= Jump;
        playerInput.OnFoot.Sprint.started -= SprintStarted;
        playerInput.OnFoot.Sprint.canceled -= SprintCanceled;
    }

    private void CheckIsGrounded()
    {
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out RaycastHit hitTheFloor, rayLength))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }

    public void Move(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;

        moveDirection.x = input.x;
        moveDirection.z = input.y;

        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            if (isGrounded)
                isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (!isGrounded) 
            playerVelocity.y += gravity * Time.deltaTime;

        characterController.Move(playerVelocity * Time.deltaTime + currentSpeed * Time.deltaTime * transform.TransformDirection(moveDirection));
    }

    public void Jump(InputAction.CallbackContext value)
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * -3.0f * gravity);
        }

        OnJump.Invoke();
    }

    public void SprintStarted(InputAction.CallbackContext value)
    {
        currentSpeed = sprintSpeed;
        isSprinting = true;
    }

    public void SprintCanceled(InputAction.CallbackContext value)
    {
        currentSpeed = walkSpeed;
        isSprinting = false;
    }
}