using UnityEngine;
using UnityEngine.InputSystem;

public class BatMovement : MonoBehaviour
{
    [Header("Movment")]
    [SerializeField] private float _flySpeed = 6.0f;
    [SerializeField] private float _acceleration = 10.0f;

    private Vector3 _velocity = Vector3.zero;
    private CharacterController _controller;

    [Header("Camera")]
    [SerializeField] private float _cameraRotateSpeed = 3f;

    private Camera _batCamera;
    private float _pitch = 0f;

    #region INPUT

    private InputActionAsset _batInputActions;
    private InputAction _flyAction;
    private InputAction _cameraAction;

    private Vector2 _cameraInput;
    #endregion

    private void Awake()
    {
        _batCamera = GameObject.FindGameObjectWithTag("BatCamera").GetComponent<Camera>();

        _batInputActions = GetComponent<PlayerInput>().actions;
        _flyAction = _batInputActions.FindAction("Fly");
        _cameraAction = _batInputActions.FindAction("Camera");
    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _flyAction.Enable();
        _cameraAction.Enable();

        _cameraAction.performed += ctx => _cameraInput = ctx.ReadValue<Vector2>();
        _cameraAction.canceled += ctx => _cameraInput = Vector2.zero;
    }

    private void OnDisable()
    {      
        _flyAction.Disable();
        _cameraAction.Disable();
    }

    private void Update()
    {
        UpdateFlying();
       
        RotateCamera();
    }

    private Vector3 GetMovementInput()
    {
        var movementInput = _flyAction.ReadValue<Vector2>();

        var input = new Vector3();

        input += _batCamera.transform.forward * movementInput.y;
        input += _batCamera.transform.right * movementInput.x;
        input = Vector3.ClampMagnitude(input, 1.0f);
        input *= _flySpeed;

        return input;
    }

    private void UpdateFlying()
    {
        var input = GetMovementInput();

        var factor = _acceleration * Time.deltaTime;

        _velocity = Vector3.Lerp(_velocity, input, factor);

        _controller.Move(_velocity * Time.deltaTime);
    }

    private void RotateCamera()
    {
        if (_cameraInput != Vector2.zero)
        {
            // Rotate around the Y axis for yaw
            float yaw = _cameraInput.x * _cameraRotateSpeed;
            transform.Rotate(0, yaw, 0);

            // Adjust pitch
            _pitch -= _cameraInput.y * _cameraRotateSpeed;
            _pitch = Mathf.Clamp(_pitch, -30, 30); // Limit pitch

            // Apply the pitch to the camera
            _batCamera.transform.localEulerAngles = new Vector3(_pitch, 0, 0);
        }
    }
}





