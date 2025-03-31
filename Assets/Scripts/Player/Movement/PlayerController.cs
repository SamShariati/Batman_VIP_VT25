using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float playerSpeed = 6.0f;
    [SerializeField] private float acceleration = 10.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float jumpHeight = 1.0f;

    private Vector3 playerVelocity;
    private Vector3 velocity = Vector3.zero;
    private CharacterController controller;
    private bool groundedPlayer;

    [Header("Camera")]
    [SerializeField] private float cameraRotateSpeed = 3f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private Camera mainCamera;
    private float cameraPitch = 0.0f;

    private Vector2 movementInput;
    private Vector2 lookInput;

    private InputActionAsset playerInputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerInputActions = GetComponent<PlayerInput>().actions;
        moveAction = playerInputActions.FindAction("Movement");
        lookAction = playerInputActions.FindAction("Look");
        jumpAction = playerInputActions.FindAction("Jump");
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();

        moveAction.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => movementInput = Vector2.zero;
        lookAction.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        lookAction.canceled += ctx => lookInput = Vector2.zero;
        jumpAction.performed += OnJump;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
    }

    private void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        UpdateMovement();
        RotateCamera();

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        UpdateAnimation();
    }

    private void UpdateMovement()
    {
         
        Vector3 input = new Vector3(movementInput.x, 0, movementInput.y);
        input = mainCamera.transform.TransformDirection(input);
        input.y = 0f;
        input *= playerSpeed;
        
        float factor = acceleration * Time.deltaTime;

        velocity = Vector3.Lerp(velocity, input, factor);

        controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateAnimation()
    {
        float speed = new Vector3(velocity.x, 0, velocity.z).magnitude;
        if (speed < 0.01f) // Idle
        {
            animator.SetFloat("Speed", 0f, 0.01f, Time.deltaTime);
            AudioManager.instance.StopWalkingSound();
        }
        else if (speed >= 0.01f && speed < playerSpeed * 0.5f) // Walking
        {
            animator.SetFloat("Speed", 0.5f, 0.01f, Time.deltaTime);
            AudioManager.instance.PlayWalkingSound();
        }
        else if (speed >= playerSpeed * 0.5f) // Running
        {
            AudioManager.instance.PlayWalkingSound();
            animator.SetFloat("Speed", 1f, 0.01f, Time.deltaTime);
        }
    }

    private void RotateCamera()
    {
        if (lookInput != Vector2.zero)
        {
            float yaw = lookInput.x * cameraRotateSpeed * Time.deltaTime;
            transform.Rotate(0, yaw, 0);

            cameraPitch -= lookInput.y * cameraRotateSpeed * Time.deltaTime;
            cameraPitch = Mathf.Clamp(cameraPitch, -90, 90);
            mainCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0, 0);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }
}