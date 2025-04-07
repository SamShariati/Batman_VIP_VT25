using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 5.5f;
    [SerializeField] private float crouchSpeed = 1.8f;
    [SerializeField] private float acceleration = 10.0f;
    [SerializeField] private float gravityValue = -9.81f;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchHeight = 0.8f;
    [SerializeField] private float standHeight = 1.8f;
    [SerializeField] private float crouchTransitionSpeed = 8f;
    [SerializeField] private float crouchCameraOffset = -0.5f;

    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDepletionRate = 25f;
    [SerializeField] private float staminaRecoveryRate = 18f;
    [SerializeField] private float staminaRecoveryDelay = 1.5f;
    [SerializeField] private float minStaminaToSprint = 15f;

    [Header("Camera")]
    [SerializeField] private float cameraRotateSpeed = 3f;
    [SerializeField] private float headBobSpeed = 14f;
    [SerializeField] private float headBobAmount = 0.05f;

    [Header("Audio")]
    [SerializeField] private float footstepIntervalWalk = 0.5f;
    [SerializeField] private float footstepIntervalSprint = 0.3f;
    [SerializeField] private float footstepIntervalCrouch = 0.7f;

    private Camera mainCamera;
    private CharacterController controller;
    private float cameraPitch = 0.0f;
    private float originalCameraY;
    private float currentSpeed;
    private float targetHeight;
    private float currentStamina;
    private float timeSinceLastSprint;
    private float footstepTimer;
    private float headBobTimer;
    private Vector3 headBobOriginalPosition;

    private bool isCrouching;
    private bool isSprinting;
    private bool isExhausted;
    private bool groundedPlayer;

    private Vector3 playerVelocity;
    private Vector3 velocity = Vector3.zero;

    private Vector2 movementInput;
    private Vector2 lookInput;

    private InputActionAsset playerInputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;
    private InputAction crouchAction;

    private void Awake()
    {
        mainCamera = Camera.main;
        originalCameraY = mainCamera.transform.localPosition.y;
        headBobOriginalPosition = mainCamera.transform.localPosition;
        playerInputActions = GetComponent<PlayerInput>().actions;

        moveAction = playerInputActions.FindAction("Movement");
        lookAction = playerInputActions.FindAction("Look");
        sprintAction = playerInputActions.FindAction("Sprint");
        crouchAction = playerInputActions.FindAction("Crouch");
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = walkSpeed;
        targetHeight = standHeight;
        currentStamina = maxStamina;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        sprintAction.Enable();
        crouchAction.Enable();

        moveAction.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => movementInput = Vector2.zero;
        lookAction.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        lookAction.canceled += ctx => lookInput = Vector2.zero;
        sprintAction.performed += OnStartSprint;
        sprintAction.canceled += OnEndSprint;
        crouchAction.performed += OnCrouch;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        sprintAction.Disable();
        crouchAction.Disable();
    }

    private void Update()
    {
        HandleGravity();
        HandleMovementStates();
        UpdateMovement();
        RotateCamera();
        UpdateCrouch();
        UpdateStamina();
        HandleHeadBob();
        HandleFootsteps();
    }

    private void HandleGravity()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -0.5f; // Small value to keep player grounded
        }
        else
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void HandleMovementStates()
    {
        // Can't sprint while crouching
        if (isCrouching && isSprinting)
        {
            isSprinting = false;
        }

        // Determine current movement speed
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else if (isSprinting && movementInput.y > 0 && !isExhausted && currentStamina > minStaminaToSprint)
        {
            currentSpeed = sprintSpeed;
            currentStamina -= staminaDepletionRate * Time.deltaTime;
            timeSinceLastSprint = 0f;
        }
        else
        {
            currentSpeed = walkSpeed;
            isSprinting = false;
            timeSinceLastSprint += Time.deltaTime;
        }
    }

    private void UpdateMovement()
    {
        Vector3 input = new Vector3(movementInput.x, 0, movementInput.y);
        input = mainCamera.transform.TransformDirection(input);
        input.y = 0f;
        input.Normalize();
        input *= currentSpeed;

        velocity = Vector3.Lerp(velocity, input, acceleration * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    private void RotateCamera()
    {
        if (lookInput != Vector2.zero)
        {
            // Horizontal rotation (player body)
            float yaw = lookInput.x * cameraRotateSpeed * Time.deltaTime;
            transform.Rotate(0, yaw, 0);

            // Vertical rotation (camera only)
            cameraPitch -= lookInput.y * cameraRotateSpeed * Time.deltaTime;
            cameraPitch = Mathf.Clamp(cameraPitch, -90, 90);
            mainCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0, 0);
        }
    }

    private void UpdateCrouch()
    {
        // Smoothly transition between crouch/stand heights
        controller.height = Mathf.Lerp(controller.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);

        // Adjust camera position for crouching
        float targetCameraY = isCrouching ? originalCameraY + crouchCameraOffset : originalCameraY;
        Vector3 cameraPos = mainCamera.transform.localPosition;
        cameraPos.y = Mathf.Lerp(cameraPos.y, targetCameraY, crouchTransitionSpeed * Time.deltaTime);
        mainCamera.transform.localPosition = cameraPos;
    }

    private void UpdateStamina()
    {
        if (!isSprinting && timeSinceLastSprint > staminaRecoveryDelay)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }

        isExhausted = currentStamina <= 0;
    }

    private void HandleHeadBob()
    {
        if (velocity.magnitude > 0.1f && groundedPlayer)
        {
            // Calculate head bob based on movement speed
            float bobSpeed = isSprinting ? headBobSpeed * 1.5f : isCrouching ? headBobSpeed * 0.7f : headBobSpeed;
            float bobAmount = isSprinting ? headBobAmount * 1.3f : isCrouching ? headBobAmount * 0.6f : headBobAmount;

            headBobTimer += Time.deltaTime * bobSpeed;
            Vector3 newPosition = headBobOriginalPosition;
            newPosition.y += Mathf.Sin(headBobTimer) * bobAmount;
            newPosition.x += Mathf.Cos(headBobTimer * 0.5f) * bobAmount * 0.5f;

            if (isCrouching) newPosition.y += crouchCameraOffset;

            mainCamera.transform.localPosition = Vector3.Lerp(
                mainCamera.transform.localPosition,
                newPosition,
                Time.deltaTime * 10f
            );
        }
        else
        {
            // Return to original position
            Vector3 targetPosition = headBobOriginalPosition;
            if (isCrouching) targetPosition.y += crouchCameraOffset;

            mainCamera.transform.localPosition = Vector3.Lerp(
                mainCamera.transform.localPosition,
                targetPosition,
                Time.deltaTime * 10f
            );
            headBobTimer = 0;
        }
    }

    private void HandleFootsteps()
    {
        if (velocity.magnitude > 0.1f && groundedPlayer)
        {
            float interval = isSprinting ? footstepIntervalSprint :
                           isCrouching ? footstepIntervalCrouch :
                           footstepIntervalWalk;

            footstepTimer += Time.deltaTime;

            if (footstepTimer >= interval)
            {
                footstepTimer = 0;
                PlayFootstepSound();
            }
        }
        else
        {
            footstepTimer = 0;
        }
    }

    private void PlayFootstepSound()
    {
        if (isSprinting)
        {
            //AudioManager.instance.PlayRunningSound();
        }
        else if (isCrouching)
        {
            //AudioManager.instance.PlayCrouchingSound();
        }
        else
        {
            AudioManager.instance.PlayWalkingSound();
        }
    }

    private void OnStartSprint(InputAction.CallbackContext context)
    {
        if (!isCrouching && currentStamina > minStaminaToSprint && movementInput.y > 0)
        {
            isSprinting = true;
        }
    }

    private void OnEndSprint(InputAction.CallbackContext context)
    {
        isSprinting = false;
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = !isCrouching;
            targetHeight = isCrouching ? crouchHeight : standHeight;

            // Can't sprint while crouching
            if (isCrouching) isSprinting = false;
        }
    }
}