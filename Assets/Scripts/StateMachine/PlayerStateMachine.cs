using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{

    //STATE VARIABLES
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    //GETSET
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    //basic movement
    public float Speed  { get { return _speed; } set { _speed = value; } }
    public float RunningSpeed { get { return _runningSpeed; } set { _runningSpeed = value; } }
    public float Gravity { get { return _gravity; } set { _gravity = value; } }
    public float JumpHeight { get { return _jumpHeight; } set { _jumpHeight = value; } }
    public float SpeedMultiplier { get { return _speedMultiplier; } set { _speedMultiplier = value; } }
    public float CrouchSpeedMultiplier { get { return _crouchSpeedMultiplier; } set { _crouchSpeedMultiplier = value; } }
    public float AirSmoothment { get { return _airSmoothment; } set { _airSmoothment = value; } }
    public float VelocityY { get { return _velocity.y; } set { _velocity.y = value; } }
    public Vector3 Velocity { get { return _velocity; } set { _velocity = value; } }
    public bool IsJumping { get { return _isJumping; } set { _isJumping = value; } }
    public bool IsCrouching { get { return _isCrouching; } set { _isCrouching = value; } }
    public float StandHeight { get { return _standHeight; } set { _standHeight = value; } }
    public float CrouchHeight { get { return _crouchHeight; } set { _crouchHeight = value; } }
    public float CrouchSmoothTime { get { return _crouchSmoothTime; } set { _crouchSmoothTime = value; } }
    public bool CanGoUp { get { return _canGoUp; } set { _canGoUp = value; } }
    public bool IsGrounded { get { return _isGrounded; } set { _isGrounded = value; } }
    public Vector3 AirMovementSmoothValueInAir { get { return _airMovementSmoothValueInAir; } set { _airMovementSmoothValueInAir = value; } }
    public Vector3 MovementSmooth { get { return _movementSmooth; } set { _movementSmooth = value; } }
    public CharacterController CharacterController { get { return _characterController; } }
    public Transform PlayerBody { get { return _playerBody; } }
    public float MoveInputX { get { return _moveInput.x; } set { _moveInput.x = value; } }
    public float MoveInputY { get { return _moveInput.y; } set { _moveInput.y = value; } }
    public bool IsRunning { get { return _isRunning; } set { _isRunning = value; } }
    public bool IsMoving { get { return isMoving; } set { isMoving = value; } }
    public float VectorMultiplier { get { return _vectorMultiplier; } set { _vectorMultiplier = value; } }
    public bool OnSlopeGetSet { get { return OnSlope(); } }
    public bool IsFalling { get { return _isFalling; } set { _isFalling = value; } }
    public bool InputAllowed { get { return _inputAllowed; } set { _inputAllowed = value; } }
    public float LengthFromGround { get { return _lengthFromGround; } set { _lengthFromGround = value; } }
    public bool OnStairsTagGetSet { get { return OnStairsTag(); } }
    public float SlopeAndStairForce { get { return _slopeAndStairForce; } set { _slopeAndStairForce = value; } }
    public Vector3 Movement { get { return _movement; } set { _movement = value; } }
    public float MovementX { get { return _movement.x; } set { _movement.x = value; } }
    public float MovementZ { get { return _movement.z; } set { _movement.z = value; } }


    //animation
    public Animator Animator { get { return _animator; } }
    public int AnimIDSpeed { get { return _animIDSpeed; } set { _animIDSpeed = value; } }
    public int AnimIDGrounded { get { return _animIDGrounded; } set { _animIDGrounded = value; } }
    public int AnimIDJump { get { return _animIDJump; } set { _animIDJump = value; } }
    public int AnimIDInAir { get { return _animIDInAir; } set { _animIDInAir = value; } }
    public int AnimIDLanding { get { return _animIDLanding; } set { _animIDLanding = value; } }
    public int AnimIDCrouching { get { return _animIDCrouching; } set { _animIDCrouching = value; } }
    public int XVelHash { get { return _xVelHash; } set { _xVelHash = value; } }
    public int YVelHash { get { return _yVelHash; } set { _yVelHash = value; } }
    public float AnimationBlendSpeed  { get { return _animationBlendSpeed; } set { _animationBlendSpeed = value; } }

    public bool CanAnimate { get { return _canAnimate; } set { _canAnimate = value; } }

    public float CurrentInputVectorY { get { return CurrentInputVector.y; } }
    public float CurrentInputVectorX { get { return CurrentInputVector.x; } }

    //ANIMATION

    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDInAir;
    private int _animIDLanding;
    private int _animIDCrouching;

    private int _xVelHash;
    private int _yVelHash;

    private int _turningAngleHash;

    private int _turningLeftHash;

    private float _animationBlend;

    public bool AnimationsPlayed { get; set; }

    //references
    //private PlayerControl playerControl;

    //private RotateBodyToHorizon rotateBodyToHorizon;

    private CameraLook _cameraLook;

    private Animator _animator;

    //private PlayerMovement playerMovement;

    //PLAYER CONTROL
    /*private static PlayerControl _instance;

    public static PlayerControl Instance
    {
        get { return _instance; }
    }*/

    public bool holdToRun;
    public bool holdToCrouch;

    [SerializeField] private float _controlSmoothment = 0.1f;
    [SerializeField] private float _mouseSmoothment = 0.1f;
    [SerializeField] private float _mouseSensitivity;
    private Vector2 _airMovementSmoothValueInLand;

    private Vector2 _currentInputVectorAir;
    private Vector2 _currentInputVectorForMouse;

    [SerializeField] private bool _hasFiredOnce;

    private bool _isRunning;
    public bool _isCrouching;
    public bool isMoving;

    private float _vectorMultiplier = 1f;

    public Vector2 CurrentInputVector { get { return _currentInputVector * _vectorMultiplier; } }

    private Vector2 _currentInputVector;



    private CharacterController _characterController;

    private FPSPlayer _fpsPlayer;

    //private CharacterAnimation characterAnimation;

    //PLAYER MOVEMENT
    //private CharacterController characterController;




    //stairs
    [Header("Stairs And Slopes")]
    [Tooltip("this value will be used to find the current ground and determine if it's tagged 'stairs'. Higher values will work for more situations, but extreme ones will consume too many resources")]
    [SerializeField] private float _stairsCheckLength = 0.1f;
    [Tooltip("this value will be used, when in the air, to determine how far the ground is from the point the player is at. Higher values will work for more situations, but extreme ones will consume too many resources")]
    [SerializeField] private float _lengthFromGround = 1f;
    [Tooltip("to prevent the bouncing that happens to most first person controllers when going down slopes or stairs, gravity is multiplied by this value. Higher values work for more situations, but extreme ones risk the player clipping through the floor.")]
    [SerializeField] private float _slopeAndStairForce = 1f;
    [Tooltip("this value will be used to determine the surface the player is standing on is a slope (using normals).")]
    [SerializeField] private float _slopeForceRayLength = 1f;




    [Header("Basic Movement")]
    [SerializeField] private float _speed = 10f;
    [Tooltip("how much should the player accelerate when pulled by gravity? A value around -30 is not too floaty, but doesn't snap you right down either.")]
    [SerializeField] private float _gravity = -9.81f;
    [Tooltip("this value is used to calculate the vertical velocity. best to leave it between x and y.")]
    [SerializeField] private float _jumpHeight = 2f;
    [Tooltip("the 'speed' value will be multiplied by this value when running is toggled.")]
    [SerializeField] private float _speedMultiplier;

    [SerializeField] private float _runningSpeed;

    [Tooltip("the 'speed' value will be multiplied by this value when crouching is toggled.")]
    [SerializeField] private float _crouchSpeedMultiplier;
    [Tooltip("how smooth movement in the air should be. This is used both for in air movement as well as for keeping momentum")]
    [SerializeField] private float _airSmoothment = 0.6f;
    [Tooltip("Transform representing the player's body")]
    [SerializeField] private Transform _playerBody; 

    //private basic movement
    private Vector3 _movementSmooth = Vector3.zero;
    private Vector3 _airMovementSmoothValueInAir;
    private Transform _currentGround;
    private Vector2 _moveInput;
    private bool _isFalling;
    private bool _hasFired;
    Vector3 _velocity;
    public bool _isJumping;
    [SerializeField] private bool _isGrounded;
    public Vector3 _movement;
    private Vector3 _baseMovement;

    //crouching
    [Header("Crouching")]
    [Tooltip("")]
    [SerializeField] private float _standHeight = 2f;
    [Tooltip("")]
    [SerializeField] private float _crouchHeight = 1f;
    private bool _canGoUp;
    private RaycastHit _roofCheckInfo;

    Vector3 _crouchCenterAdjusted;
    Vector3 _standCenterAdjusted;

    [Tooltip("")]
    [SerializeField] private float _cameraHeight;
    [Tooltip("")]
    [SerializeField] private float _crouchSmoothTime = 1f;
    private float _velocityCrouch;
    private float _velocityCrouchOther;
    [SerializeField] private LayerMask _roofLayers;
    [SerializeField] private float _roofCheckDistance = 3f;
    private float _roofCheckStart;

    //groundcheck
    [Header("Ground Check")]
    [SerializeField] private float _groundCheckRadius = 0.32f;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 0.5f, 0f);
    [SerializeField] private float _maxDistance = 0.22f;
    private Vector3 _groundCheckOrigin;
    private RaycastHit _hitInfo;
    

    //animation
    [Header("Animation")]
    [SerializeField] private float _animationBlendSpeed = 1.5f;


    Vector3 movementVectorForAnim;


    private Transform cameraTransform;

    //CAMERA LOOK
    //[SerializeField] private Transform playerBody;


    [SerializeField] private Transform forward;

    public float rotationSpeed = 1.0f;

    public float rotationSpeedForBody = 1.0f;

    private bool isRotating = false;


    private float _cinemachineTargetPitch;
    private float _cinemachineTargetYaw;

    private float _rotationVelocity;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 90.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -90.0f;

    private float desiredRotation;

    public float rotationThresholdDunno = 360f;

    //ROTATION TO HORIZON
    [SerializeField] private GameObject player;

    private bool _inputAllowed = false;
    private bool _canAnimate = false;

    public float headRotationThreshold = 60f; // Threshold for head rotation in degrees
    public float bodyRotationSpeed = 5f; // Speed of body rotation
    public Transform targetTransform; // Transform representing the target to rotate towards
    private Quaternion originalBodyRotation; // Original rotation of the player's body
    public Quaternion targetRotation; // Target rotation to match
    private bool isBodyTurning; // Flag to track if the body is turning

    //camera manager
    private CameraAngularSpeed cameraAngularSpeed;
    //private CameraLook cameraLook; //Reference to CameraLook scrupt

    //character animation
    //private CharacterAnimation characterAnimation;

    //player movement
    //private PlayerMovement playerMovement;

    private float headRotationAngle; //angle that the head has rotated

    private Quaternion cameraForwardRotation; //stores the current forward rotation of the camera

    public float rotationThreshold = 90f; //From this threshold on, the player will smoothly rotate instead of lerping

    public bool isTurning { get; set; }


    void Awake()
    {
        StartCoroutine(DelayInputProcessing(0.1f));
        //states setup
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState(); 

        //
        //characterAnimation = GetComponent<CharacterAnimation>();

        /*f (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }*/
        _fpsPlayer = new FPSPlayer();
        
        _fpsPlayer.Player.Run.performed += Run;
        _fpsPlayer.Player.Run.started += Run;
        _fpsPlayer.Player.Run.canceled += Run;

        _fpsPlayer.Player.Crouch.performed += Crouch;
        _fpsPlayer.Player.Crouch.started += Crouch;
        _fpsPlayer.Player.Crouch.canceled += Crouch;

        _fpsPlayer.Player.Movement.performed += Move;
        _fpsPlayer.Player.Movement.canceled += Reset;
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        AssignAnimationIDs();
        _runningSpeed = 1f;

        if (_animator == null)
            Debug.LogError("Animator reference is null.");
    }

    void Update()
    {
        if (_isJumping)
        {
            Debug.Log("_isJumping = true");
        }
        GroundCheck();
        RoofCheck();
        _currentState.UpdateStates();
        
        _moveInput = GetPlayerMovement();
        
        HandleMovement();

        //HandleAnimStateMachine();
        HoldToRun();
        HoldToCrouch();
        OnSlope();
        //HandleMovement();
    }

    void RoofCheck()
    {
        Ray ray = new Ray(transform.position, transform.up);
        if (Physics.Raycast(ray, out _roofCheckInfo, _roofCheckDistance, _roofLayers, QueryTriggerInteraction.Ignore))
        {
            _canGoUp = false;
            Debug.DrawRay(transform.position, transform.up * _roofCheckDistance);
        }
        else
        {
            _canGoUp = true;
        }

    }

    void HandleMovement()
    {
        Vector3 velocityMovementSpeedAndSmoothCombo = _velocity + ((_movement + _airMovementSmoothValueInAir) * _speed * _runningSpeed);
        _characterController.Move(velocityMovementSpeedAndSmoothCombo  * Time.deltaTime);
        //_movement = (_moveInput.y * transform.forward) + (_moveInput.x * transform.right);
        //_movementSmoothed = _movement;
        //_airMovementSmoothValueInAir = Vector3.SmoothDamp(_airMovementSmoothValueInAir, _movement, ref _movementSmooth, _airSmoothment);

    }
    private IEnumerator DelayInputProcessing(float delaySeconds)
    {
        // Wait for the specified delay before allowing input
        yield return new WaitForSeconds(delaySeconds);

        // Enable input processing
        _inputAllowed = true;

        // Raise an event or trigger a method to indicate that input processing is now allowed
        //OnInputAllowed();
    }

    private bool OnSlope()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position * (_characterController.height / 2), Vector3.down, out hit, _characterController.height / 2 * _slopeForceRayLength))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }
    private bool OnStairsTag()
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position * (_characterController.height / 2), Vector3.down, out hit, _stairsCheckLength))
        {
            _currentGround = hit.transform;
            if (!_isJumping && _currentGround.tag == "Stairs" && !_isFalling)
            {
                //Debug.Log("is on stairs via tag");
                return true;
            }
        }
        return false;
    }
    private void GroundFarEnough()
    {

        if (!_isGrounded && !_hasFired)
        {
            _hasFired = true;
            RaycastHit hit;

            if (Physics.SphereCast(transform.position * (_characterController.height / 2), 1f, Vector3.down, out hit, _lengthFromGround))
            {

                //Debug.Log(hit.distance);
                if (hit.distance >= 0.001f)
                {
                    //_isFalling = true;

                }
                else
                {
                    Debug.Log(hit.distance);
                    //_isFalling = false;
                }
            }
        }

        else if (_isGrounded)
        {
            _hasFired = false;
            //_isFalling = false;
        }
    }
    

    private void AssignAnimationIDs()
    {
        _xVelHash = Animator.StringToHash("x_velocity");
        _yVelHash = Animator.StringToHash("y_velocity");


        _turningAngleHash = Animator.StringToHash("turnAngle_velocity");


        _turningLeftHash = Animator.StringToHash("left_velocity");

        _animIDCrouching = Animator.StringToHash("Crouching");

        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDInAir = Animator.StringToHash("InAir");
        _animIDLanding = Animator.StringToHash("Landed");
    }
    void GroundCheck()
    {
        _groundCheckOrigin = transform.position + _offset;
        bool hitFloor = Physics.SphereCast(_groundCheckOrigin, _groundCheckRadius, Vector3.down, out _hitInfo, _maxDistance);

        if (/*characterController.isGrounded*/ hitFloor)
        {
            _isGrounded = true;

            // Draw a line from the origin to the hit point
            //Debug.DrawLine(_groundCheckOrigin, _hitInfo.point, Color.yellow);
        }
        else
        {
            _isGrounded = false;
        }

    }

    void OnDrawGizmos()
    {
        _groundCheckOrigin = transform.position + _offset;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_groundCheckOrigin, _groundCheckRadius);
        // Draw a sphere at the hit point
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_hitInfo.point, 0.1f);
        Gizmos.DrawLine(_groundCheckOrigin, _groundCheckOrigin + Vector3.down * _maxDistance);

    }
    private void OnEnable()
    {
        _fpsPlayer.Enable();
    }
    private void OnDisable()
    {
        _fpsPlayer.Disable();
    }

    
    public void Reset(InputAction.CallbackContext context)
    {
        _isRunning = false;
        isMoving = false;
    }
    void Move(InputAction.CallbackContext context)
    {
        if (_inputAllowed)
        {
            isMoving = true;
            _canAnimate = true;
        }
    
    }
    public Vector2 GetPlayerMovement()
    {
        if (_inputAllowed)
        {
            Vector2 input = _fpsPlayer.Player.Movement.ReadValue<Vector2>();
            _currentInputVector = Vector2.SmoothDamp(_currentInputVector, input, ref _airMovementSmoothValueInLand, _controlSmoothment);
            return _currentInputVector;
        }
        return Vector2.zero;
    }

    public Vector2 GetMouseDelta()
    {
        Vector2 input = _fpsPlayer.Player.Look.ReadValue<Vector2>();
        Vector2 mouseSmooth = new Vector2(0, 0);
        _currentInputVectorForMouse = Vector2.SmoothDamp(_currentInputVectorForMouse, input, ref mouseSmooth, _mouseSmoothment * 0.1f);
        _currentInputVectorForMouse = _currentInputVectorForMouse * _mouseSensitivity;
        return _currentInputVectorForMouse;
        //return fpsPlayer.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJump()
    {
        if (_inputAllowed)
        {
            return _fpsPlayer.Player.Jump.triggered;
        }
        return false;

    }



    public void Crouch(InputAction.CallbackContext ctx)
    {
            if (!holdToCrouch)
            {
                if (ctx.started && !_isCrouching)
                {
                    //_vectorMultiplier = 0.5f;
                    _isCrouching = true;
                }

                else if (ctx.started && _isCrouching)
                {
                    _isCrouching = false;
                    //_vectorMultiplier = 1f;
                }
            }
        
    }
    public float GetPlayerCrouching()
    {
        return _fpsPlayer.Player.Crouch.ReadValue<float>();
    }
    void HoldToCrouch()
    {
        if (holdToCrouch)
        {

            if (GetPlayerCrouching() > 0)
            {
                _isCrouching = true;
            }
            else
            {
                _isCrouching = false;
            }

        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (!holdToRun)
        {
            if (context.started && !_isRunning)
            {
                _isRunning = true;
                //_vectorMultiplier = 2f;
            }

            else if (context.started && _isRunning)
            {
                _isRunning = false;
                //_vectorMultiplier = 1f;
            }
        }
    }
    public float GetPlayerRunning()
    {
        return _fpsPlayer.Player.Run.ReadValue<float>();
    }
    void HoldToRun()
    {
        if (holdToRun)
        {
            
            if (GetPlayerRunning() > 0)
            {
                _isRunning = true;
            }
            else
            {
                _isRunning = false;
            }

        }
    }
}
