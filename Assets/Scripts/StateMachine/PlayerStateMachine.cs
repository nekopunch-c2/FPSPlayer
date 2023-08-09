using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NekoSpace;

public class PlayerStateMachine : MonoBehaviour
{

    //STATE VARIABLES
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    //GETSET
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public PlayerStateFactory States { get { return _states; } set { _states = value; } }
    //groundcheck
    public bool IsGrounded { get { return _isGrounded; } set { _isGrounded = value; } }
    //roofcheck
    public LayerMask RoofLayers { get { return _roofLayers; } }
    public float RoofCheckDistance { get { return _roofCheckDistance; } }
    public float RoofCheckStart { get { return _roofCheckStart; } }
    //basic movement
    public float CameraAngularSpeed { get { return _cameraAngularSpeed.AngularSpeedY; } }
    public bool OnLadder { get { return _onLadder; } set { _onLadder = value; } }
    public bool AboveLadder { get { return _aboveLadder; } set { _aboveLadder = value; } }
    public bool BelowLadder { get { return _belowLadder; } set { _belowLadder = value; } }
    public Transform PlayerAboveLadderClimbingPos { get { return playerAboveLadderClimbingPos; } }
    public Transform PlayerBelowLadderClimbingPos { get { return playerBelowLadderClimbingPos; } }
    public Transform PlayerAboveLadderGetDownPos { get { return playerAboveLadderGetDownPos; } }
    public Transform PlayerBelowLadderGetDownPos { get { return playerBelowLadderGetDownPos; } }
    public float Speed  { get { return _speed; } set { _speed = value; } }
    public float RunningSpeed { get { return _runningSpeed; } set { _runningSpeed = value; } }
    public float CrouchedSpeed { get { return _crouchedSpeedMultiplier; } set { _crouchedSpeedMultiplier = value; } }
    public float Gravity { get { return _gravity; } set { _gravity = value; } }
    public float JumpHeight { get { return _jumpHeight; } set { _jumpHeight = value; } }
    public float SpeedMultiplier { get { return _speedMultiplier; } set { _speedMultiplier = value; } }
    public float CrouchSpeedMultiplier { get { return _crouchSpeedMultiplier; } set { _crouchSpeedMultiplier = value; } }
    public float AirSmoothment { get { return _airSmoothment; } set { _airSmoothment = value; } }
    public float VelocityY { get { return _velocity.y; } set { _velocity.y = value; } }
    public Vector3 Velocity { get { return _velocity; } set { _velocity = value; } }
    public bool IsJumping { get { return _isJumping; } set { _isJumping = value; } }
    public float StandHeight { get { return _standHeight; } set { _standHeight = value; } }
    public Vector3 StandCenterAdjusted { get { return _standCenterAdjusted; } set { _standCenterAdjusted = value; } }
    public float CrouchHeight { get { return _crouchHeight; } set { _crouchHeight = value; } }
    public float CrouchSmoothTime { get { return _crouchSmoothTime; } set { _crouchSmoothTime = value; } }
    public Vector3 CrouchCenterAdjusted { get { return _crouchCenterAdjusted; } set { _crouchCenterAdjusted = value; } }
    public Vector3 AirMovementSmoothValueInAir { get { return _airMovementSmoothValueInAir; } set { _airMovementSmoothValueInAir = value; } }
    public Vector3 MovementSmooth { get { return _movementSmooth; } set { _movementSmooth = value; } }
    public CharacterController CharacterController { get { return _characterController; } }
    public Transform PlayerBody { get { return _playerBody; } set { _playerBody = value; } }
    public Vector3 PlayerBodyPosition { get { return _playerBody.position; } set { _playerBody.position = value; } }
    public float MoveInputX { get { return _moveInput.x; } set { _moveInput.x = value; } }
    public float MoveInputY { get { return _moveInput.y; } set { _moveInput.y = value; } }
    public Transform ActivePlatform { get { return _activePlatform; } set { _activePlatform = value; } }
    //stamina
    public float StaminaGetSet { get { return Stamina; } set { Stamina = value; } }
    public float MaxStaminaGet { get { return MaxStamina; } set { MaxStamina = value; } }
    public float StaminaDecIncSpeedGetSet { get { return StaminaDecIncSpeed; } set { StaminaDecIncSpeed = value; } }
    public bool ShouldUseStaminaGet { get { return ShouldUseStamina; } }
    public bool IsTooTired { get { return _isTooTired; } }

    //player input handler
    public bool GetPlayerClimb { get { return _playerInputHandler.GetPlayerClimbInput; } set { _playerInputHandler.GetPlayerClimbInput = value; } }
    public bool IsCrouching { get { return _playerInputHandler.IsCrouchingInput; } set { _playerInputHandler.IsCrouchingInput = value; } }
    public bool IsRunning { get { return _playerInputHandler.IsRunningInput; } set { _playerInputHandler.IsRunningInput = value; } }
    public bool IsMoving { get { return _playerInputHandler.IsMovingInput; } set { _playerInputHandler.IsMovingInput = value; } }
    public float VectorMultiplier { get { return _playerInputHandler.VectorMultiplierInput; } set { _playerInputHandler.VectorMultiplierInput = value; } }
    public bool InputAllowed { get { return _playerInputHandler.InputAllowedInput; } set { _playerInputHandler.InputAllowedInput = value; } }
    public bool PlayerJump { get { return _playerInputHandler.PlayerJumpInput; } }
    public Vector2 GetPlayerMovement { get { return _playerInputHandler.GetPlayerMovementInput; } }
    public float RotationSpeed { get { return _playerInputHandler.RotationSpeedInput; } }
    public CameraRotationDirection RotationDirectionRight { get { return CameraRotationDirection.Right; } }
    public CameraRotationDirection RotationDirectionLeft { get { return CameraRotationDirection.Left; } }
    public CameraRotationDirection RotationDirection { get { return _playerInputHandler.RotationDirectionInput; } }

    public bool OnSlopeGetSet { get { return OnSlope(); } }
    public bool IsFalling { get { return _isFalling; } set { _isFalling = value; } }
    public float LengthFromGround { get { return _lengthFromGround; } set { _lengthFromGround = value; } }
    public float GroundedTimer { get { return _groundedTimer; } set { _groundedTimer = value; } }
    public float IsCurrentlyGroundedTimer { get { return _isCurrentlyGroundedTimer; } set { _isCurrentlyGroundedTimer = value; } }
    public bool OnStairsGetSet { get { return OnStairs(); } }
    public bool OnStairsTagGetSet { get { return OnStairsTag(); } }
    public float GroundedGravity { get { return _groundedGravity; } set { _groundedGravity = value; } }
    public Vector3 Movement { get { return _movement; } set { _movement = value; } }
    public float MovementX { get { return _movement.x; } set { _movement.x = value; } }
    public float MovementZ { get { return _movement.z; } set { _movement.z = value; } }
    public float SlopeForceRayLength { get { return _slopeForceRayLength; } set { _slopeForceRayLength = value; } }
    public float StairsCheckLength { get { return _stairsCheckLength; } set { _stairsCheckLength = value; } }
    public Transform CurrentGround { get { return _currentGround; } set { _currentGround = value; } }

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
    public int AngularSpeedY { get { return _angularSpeedY; } set { _angularSpeedY = value; } }
    public float AnimationBlendSpeed  { get { return _animationBlendSpeed; } set { _animationBlendSpeed = value; } }
    public int Turning { get { return _turning; } set { _turning = value; } }
    public int TurningRight { get { return _turningRight; } set { _turningRight = value; } }
    public int TurnAngle { get { return _turnAngle; } set { _turnAngle = value; } }
    public int TurningAngleHash { get { return _turningAngleHash; } set { _turningAngleHash = value; } }
    public int OnLadderHash { get { return _onLadderHash; } set { _onLadderHash = value; } }

    public bool CanAnimate { get { return _playerInputHandler.CanAnimateInput; } set { _playerInputHandler.CanAnimateInput = value; } }

    public float CurrentInputVectorY { get { return _playerInputHandler.CurrentInputVectorYInput; } }
    public float CurrentInputVectorX { get { return _playerInputHandler.CurrentInputVectorXInput; } }

    //ANIMATION

    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDInAir;
    private int _animIDLanding;
    private int _animIDCrouching;
    private int _turningRight;
    private int _turning;
    private int _onLadderHash;

    private int _xVelHash;
    private int _yVelHash;

    private int _angularSpeedY;

    private int _turningAngleHash;
    private int _hitDistance;
    private int _turnAngle;

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



    //private CharacterAnimation characterAnimation;

    //PLAYER MOVEMENT
    //private CharacterController characterController;

    //reorder
    private Transform _activePlatform;

    //stairs
    [Header("Stairs And Slopes")]
    [Tooltip("this value will be used to find the current ground and determine if it's tagged 'stairs'. Higher values will work for more situations, but extreme ones will consume too many resources")]
    [SerializeField] private float _stairsCheckLength = 0.1f;
    [Tooltip("this value will be used, when in the air, to determine how far the ground is from the point the player is at. Higher values will work for more situations, but extreme ones will consume too many resources")]
    [SerializeField] private float _lengthFromGround = 1f;
    [Tooltip("to prevent the bouncing that happens to most first person controllers when going down slopes or stairs, gravity is multiplied by this value. Higher values work for more situations, but extreme ones risk the player clipping through the floor.")]
    [SerializeField] private float _groundedGravity = 1f;
    [Tooltip("this value will be used to determine the surface the player is standing on is a slope (using normals).")]
    [SerializeField] private float _slopeForceRayLength = 1f;


    [Header("Ladders")]
    public Transform playerAboveLadderClimbingPos;
    public Transform playerBelowLadderClimbingPos;
    public Transform playerAboveLadderGetDownPos;
    public Transform playerBelowLadderGetDownPos;
    private bool _aboveLadder;
    private bool _belowLadder;
    [SerializeField] private bool _onLadder;
    private Transform _currentLadder;

    [Header("Basic Movement")]
    [SerializeField] private float _speed = 10f;
    [Tooltip("how much should the player accelerate when pulled by gravity? A value around -30 is not too floaty, but doesn't snap you right down either.")]
    [SerializeField] private float _gravity = -9.81f;
    [Tooltip("this value is used to calculate the vertical velocity. best to leave it between x and y.")]
    [SerializeField] private float _jumpHeight = 2f;
    [Tooltip("the 'speed' value will be multiplied by this value when running is toggled.")]
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private Transform _playerRoot;
    [SerializeField] private float _runningSpeed;
    [SerializeField] private float _crouchedSpeedMultiplier;
    [SerializeField] private float _groundedOffset = -0.14f;
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
    public Vector3 _velocity;
    public bool _isJumping;
    [SerializeField] private bool _isGrounded;
    public Vector3 _movement;
    private Vector3 _baseMovement;
    [SerializeField] private float _groundedTimer = 0f;
    [SerializeField] private float _isCurrentlyGroundedTimer = 0f;

    [Header("Stamina")]
    public float Stamina = 10f;
    public float MaxStamina = 10f;
    public float StaminaDecIncSpeed = 2f;
    public bool ShouldUseStamina = false;
    private bool _isTooTired = false;

    //crouching
    [Header("Crouching")]
    [Tooltip("")]
    [SerializeField] private float _standHeight = 2f;
    [Tooltip("")]
    [SerializeField] private float _crouchHeight = 1f;
    

    public Vector3 _crouchCenterAdjusted;
    public Vector3 _standCenterAdjusted;

    [Tooltip("")]
    [SerializeField] private float _cameraHeight;
    [Tooltip("time of transition to crouch")]
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
    [SerializeField] private LayerMask _groundLayers;
    Vector3 _spherePosition;


    //animation
    [Header("Animation")]
    [SerializeField] private float _animationBlendSpeed = 1.5f;


    


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

    //REFERENCES
    private PlayerInputHandler _playerInputHandler;
    private CharacterController _characterController;
    private CameraAngularSpeed _cameraAngularSpeed;

    void Awake()
    {
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        _runningSpeed = 1f;
        //states setup
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
        _cameraAngularSpeed = GameObject.FindWithTag("CameraManager").GetComponent<CameraAngularSpeed>();
        if (_playerInputHandler == null)
            Debug.LogError("States reference is null.");
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

    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        AssignAnimationIDs();

        
        if (_animator == null)
            Debug.LogError("Animator reference is null.");
    }

    void Update()
    {
        GroundCheck();
        //RoofCheck();
        _currentState.UpdateStates();
        Debug.Log(_currentState);
        _moveInput = GetPlayerMovement;
        OnStairs();
        OnStairsTag();
        HandleMovement();
        if (ShouldUseStamina)
        {
            if (Stamina <= 0f)
            {
                _isTooTired = true;
                
            }
            if (Stamina > 1f)
            {
                _isTooTired = false;
            }
        }

    }
    void HandleMovement()
    {
        Vector3 velocityMovementSpeedAndSmoothCombo = _velocity + (_movement * (_speed * _runningSpeed * _crouchedSpeedMultiplier));
        if (_currentState != _states.LadderTransition() && _currentState != _states.FromLadderTransition())
        {
            _characterController.Move(velocityMovementSpeedAndSmoothCombo * Time.deltaTime);
        }

    }
    
    private bool OnStairs()
    {
        if (!_isGrounded && !_isJumping && !_isFalling)
        {
            return true;
        }

        return false;
    }
    Transform FindWithTag(Transform root, string tag)
    {
        foreach (Transform t in root.GetComponentsInChildren<Transform>())
        {
            if (t.CompareTag(tag)) return t;
        }
        return null;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            _activePlatform = other.transform;
        }
        if (other.gameObject.CompareTag("Ladder"))
        {
            _onLadder = true;
        }
        if (other.gameObject.CompareTag("TopOfLadder"))
        {
            //interpolate player to ladder place
            //above ladder true
            _aboveLadder = true;
            _currentLadder = other.transform.parent;
            //playerAboveStairsClimbingPos = currentLadder.transform.Find("TopOfLadderPos");
            playerAboveLadderClimbingPos = FindWithTag(_currentLadder, "TopOfLadderPos");
            playerAboveLadderGetDownPos = FindWithTag(_currentLadder, "TopOfLadderGetDownPos");
            if (playerAboveLadderClimbingPos == null)
            {
                // Do something with the found child GameObject
                Debug.Log("Not found child with tag: " + "TopOfLadderPos");
            }
            if (playerAboveLadderGetDownPos == null)
            {
                // Do something with the found child GameObject
                Debug.Log("Not found child with tag: " + "TopOfLadderGetDownPos");
            }
        }
        if (other.gameObject.CompareTag("BottomOfLadder"))
        {
            //interpolate player to ladder place
            //below ladder true
            _belowLadder = true;
            _currentLadder = other.transform.parent;
            playerBelowLadderClimbingPos = FindWithTag(_currentLadder, "BottomOfLadderPos");
            playerBelowLadderGetDownPos = FindWithTag(_currentLadder, "BottomOfLadderGetDownPos");
            if (playerBelowLadderClimbingPos == null)
            {
                // Do something with the found child GameObject
                Debug.Log("Not found child with tag: " + "BottomOfLadderPos");
            }
            if (playerBelowLadderGetDownPos == null)
            {
                // Do something with the found child GameObject
                Debug.Log("Not found child with tag: " + "BottomOfLadderGetDownPos");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Unparent the player from the moving platform when they step off it.
        if (other.transform == _activePlatform)
        {
            _activePlatform = null;
            SetParent(_playerRoot);
        }
        if (other.gameObject.CompareTag("Ladder"))
        {
            _onLadder = false;
        }
        if (other.gameObject.CompareTag("TopOfLadder"))
        {
            _aboveLadder = false;
        }
        if (other.gameObject.CompareTag("BottomOfLadder"))
        {
            _belowLadder = false;
        }
    }

    public void SetParent(Transform newParent)
    {
        transform.SetParent(newParent, true);
    }
    /*void HandleSlopesAndStairs()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position * (_characterController.height / 2), 1f, Vector3.down, out hit, _lengthFromGround))
        {
            if (hit.distance >= 0.01f)
            {
                _isFalling = true;

            }
            else
            {
                //Debug.Log(hit.distance);
                _isFalling = false;
            }
        }
    }*/



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
            if (!_isJumping && _currentGround.tag == "Stairs")
            {
                //Debug.Log("is on stairs via tag");
                return true;
            }
        }
        return false;
    }
    public void GroundFarEnough()
    {

        //if (!_isGrounded)
        //{
            _hasFired = true;
            RaycastHit hit;
            if (Physics.SphereCast(transform.position * (_characterController.height / 2), 1f, Vector3.down, out hit, _lengthFromGround))
            {
                _animator.SetFloat(_hitDistance, hit.distance);
            }
        //}

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


        _turning = Animator.StringToHash("TurningLeft");
        _turningRight = Animator.StringToHash("TurningRight");

        _animIDCrouching = Animator.StringToHash("Crouching");

        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDInAir = Animator.StringToHash("InAir");
        _animIDLanding = Animator.StringToHash("Landed");
        _hitDistance = Animator.StringToHash("hitDistance");
        _onLadderHash = Animator.StringToHash("OnLadder");
    }
    void GroundCheck()
    {
        _spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z);
        _isGrounded = Physics.CheckSphere(_spherePosition, _groundCheckRadius, _groundLayers, QueryTriggerInteraction.Ignore);

    }

    void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (_isGrounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere( new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z), _groundCheckRadius);

    }
    
}
