using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class ExplorerPlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 10f;
    public float gravity = -15f;
    public float jumpHeight = 1.2f;

    [Header("Ground Check")]
    public float groundedOffset = -0.14f;
    public float groundedRadius = 0.28f;
    public LayerMask groundLayers;

    [Header("Camera")]
    public GameObject cameraTarget;
    public float topClamp = 70f;
    public float bottomClamp = -30f;

    [Header("Animation")]
    public float animationBlendSpeed = 0.1f;

    // Private variables
    private CharacterController _controller;
    private Animator _animator;
    private PlayerInput _playerInput;
    private GameObject _mainCamera;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isSprinting;
    private bool _isGrounded;
    private float _verticalVelocity;
    private float _cameraYaw;
    private float _cameraPitch;
    private float _animBlendX;
    private float _animBlendY;

    // Animator parameter hashes
    private int _animMoveX;
    private int _animMoveY;
    private int _animGrounded;
    private int _animJump;

    private const float _terminalVelocity = -53f;

    void Awake()
    {
        _mainCamera = Camera.main?.gameObject;
    }

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _playerInput = GetComponent<PlayerInput>();

        // Cache animator parameters
        _animMoveX = Animator.StringToHash("MoveX");
        _animMoveY = Animator.StringToHash("MoveY");
        _animGrounded = Animator.StringToHash("Grounded");
        _animJump = Animator.StringToHash("Jump");

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        GroundCheck();
        ApplyGravity();
        Move();
        RotateCamera();
    }

    void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(
            transform.position.x,
            transform.position.y - groundedOffset,
            transform.position.z);

        _isGrounded = Physics.CheckSphere(
            spherePosition,
            groundedRadius,
            groundLayers,
            QueryTriggerInteraction.Ignore);

        if (_animator != null)
            _animator.SetBool(_animGrounded, _isGrounded);
    }

    void ApplyGravity()
    {
        if (_isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;

        if (_verticalVelocity > _terminalVelocity)
            _verticalVelocity += gravity * Time.deltaTime;
    }

    void Move()
    {
        float targetSpeed = _isSprinting ? sprintSpeed : moveSpeed;

        if (_moveInput == Vector2.zero)
            targetSpeed = 0f;

        // Get camera-relative direction
        Vector3 inputDir = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;

        Vector3 moveDir = Vector3.zero;
        if (_moveInput != Vector2.zero && _mainCamera != null)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) 
                * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Rotate player to face movement direction
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
        }

        // Apply movement
        Vector3 velocity = moveDir * targetSpeed;
        velocity.y = _verticalVelocity;
        _controller.Move(velocity * Time.deltaTime);

        // Update blend tree animations
        float targetX = _moveInput.x * (targetSpeed / moveSpeed);
        float targetY = _moveInput.y * (targetSpeed / moveSpeed);
        _animBlendX = Mathf.Lerp(_animBlendX, targetX, animationBlendSpeed);
        _animBlendY = Mathf.Lerp(_animBlendY, targetY, animationBlendSpeed);

        if (_animator != null)
        {
            _animator.SetFloat(_animMoveX, _animBlendX);
            _animator.SetFloat(_animMoveY, _animBlendY);
        }
    }

    void RotateCamera()
    {
        if (_lookInput.sqrMagnitude >= 0.01f)
        {
            _cameraYaw += _lookInput.x;
            _cameraPitch -= _lookInput.y;
            _cameraPitch = Mathf.Clamp(_cameraPitch, bottomClamp, topClamp);
        }

        if (cameraTarget != null)
            cameraTarget.transform.rotation = Quaternion.Euler(
                _cameraPitch, _cameraYaw, 0f);
    }

    // Input callbacks from PlayerInput component
    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }

    public void OnSprint(InputValue value)
    {
        _isSprinting = value.isPressed;
    }

    public void OnJump(InputValue value)
    {
        if (_isGrounded && value.isPressed)
        {
            _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if (_animator != null)
                _animator.SetTrigger(_animJump);
        }
    }
}
