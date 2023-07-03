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
    public float Speed  { get { return speed; } set { speed = value; } }
    public float Gravity { get { return gravity; } set { gravity = value; } }
    public float JumpHeight { get { return jumpHeight; } set { jumpHeight = value; } }
    public float JumpHeightPull { get { return jumpHeightPull; } set { jumpHeightPull = value; } }
    public float SpeedMultiplier { get { return speedMultiplier; } set { speedMultiplier = value; } }
    public float CrouchSpeedMultiplier { get { return crouchSpeedMultiplier; } set { crouchSpeedMultiplier = value; } }
    public float AirSmoothment { get { return airSmoothment; } set { airSmoothment = value; } }
    public float VelocityY { get { return velocity.y; } set { velocity.y = value; } }
    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }
    public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
    public bool IsGrounded { get { return isGrounded; } set { isGrounded = value; } }
    public Vector3 AirMovementSmoothValueInAir { get { return airMovementSmoothValueInAir; } set { airMovementSmoothValueInAir = value; } }
    public Vector3 MovementSmooth { get { return movementSmooth; } set { movementSmooth = value; } }
    public CharacterController CharacterController { get { return characterController; } }
    public Transform PlayerBody { get { return playerBody; } }
    public float MoveInputX { get { return moveInput.x; } set { moveInput.x = value; } }
    public float MoveInputY { get { return moveInput.y; } set { moveInput.y = value; } }
    public bool IsRunning { get { return isRunning; } set { isRunning = value; } }
    public Vector2 GetPlayerMovementApplied { get { return GetPlayerMovement(); } }
    public bool IsMoving { get { return isMoving; } set { isMoving = value; } }


    //animation
    public Animator Animator { get { return _animator; } }
    public int AnimIDSpeed { get { return _animIDSpeed; } set { _animIDSpeed = value; } }
    public int AnimIDGrounded { get { return _animIDGrounded; } set { _animIDGrounded = value; } }
    public int AnimIDJump { get { return _animIDJump; } set { _animIDJump = value; } }
    public int AnimIDInAir { get { return _animIDInAir; } set { _animIDInAir = value; } }
    public int AnimIDLanding { get { return _animIDLanding; } set { _animIDLanding = value; } }
    public int AnimIDInCrouching { get { return _animIDCrouching; } set { _animIDCrouching = value; } }
    public int XVelHash { get { return _xVelHash; } set { _xVelHash = value; } }
    public int YVelHash { get { return _yVelHash; } set { _yVelHash = value; } }

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
    private PlayerControl playerControl;

    private RotateBodyToHorizon rotateBodyToHorizon;

    private CameraLook cameraLook;

    private Animator _animator;

    private PlayerMovement playerMovement;

    //PLAYER CONTROL
    /*private static PlayerControl _instance;

    public static PlayerControl Instance
    {
        get { return _instance; }
    }*/

    public bool holdToRun;
    public bool holdToCrouch;

    [SerializeField] private float controlSmoothment = 0.1f;
    [SerializeField] private float mouseSmoothment = 0.1f;
    [SerializeField] private float mouseSensitivity;
    private Vector2 airMovementSmoothValueInLand;

    private Vector2 currentInputVectorAir;
    private Vector2 currentInputVectorForMouse;

    [SerializeField] private bool hasFiredOnce;

    private bool isRunning;
    public bool IsCrouching { get; set; }
    public bool isMoving;

    private float _vectorMultiplier;

    public Vector2 CurrentInputVector { get { return currentInputVector; } }

    private Vector2 currentInputVector;



    private CharacterController characterController;

    private FPSPlayer fpsPlayer;

    //private CharacterAnimation characterAnimation;

    //PLAYER MOVEMENT
    //private CharacterController characterController;




    //stairs
    [Header("Stairs And Slopes")]
    [Tooltip("this value will be used to find the current ground and determine if it's tagged 'stairs'. Higher values will work for more situations, but extreme ones will consume too many resources")]
    [SerializeField] private float stairsCheckLength = 0.1f;
    [Tooltip("this value will be used, when in the air, to determine how far the ground is from the point the player is at. Higher values will work for more situations, but extreme ones will consume too many resources")]
    [SerializeField] private float lengthFromGround;
    [Tooltip("to prevent the bouncing that happens to most first person controllers when going down slopes or stairs, gravity is multiplied by this value. Higher values work for more situations, but extreme ones risk the player clipping through the floor.")]
    [SerializeField] private float slopeAndStairForce;
    [Tooltip("this value will be used to determine the surface the player is standing on is a slope (using normals).")]
    [SerializeField] private float slopeForceRayLength;




    [Header("Basic Movement")]
    [SerializeField] private float speed = 10f;
    [Tooltip("how much should the player accelerate when pulled by gravity? A value around -30 is not too floaty, but doesn't snap you right down either.")]
    [SerializeField] private float gravity = -9.81f;
    [Tooltip("this value is used to calculate the vertical velocity. best to leave it between x and y.")]
    [SerializeField] private float jumpHeight = 2f;
    [Tooltip("how far should the player jump before it's slowed down by gravity.")]
    [SerializeField] private float jumpHeightPull = 2f;
    [Tooltip("the 'speed' value will be multiplied by this value when running is toggled.")]
    [SerializeField] private float speedMultiplier;
    [Tooltip("the 'speed' value will be multiplied by this value when crouching is toggled.")]
    [SerializeField] private float crouchSpeedMultiplier;
    [Tooltip("how smooth movement in the air should be. This is used both for in air movement as well as for keeping momentum")]
    [SerializeField] private float airSmoothment = 0.6f;
    [Tooltip("Transform representing the player's body")]
    [SerializeField] private Transform playerBody; 

    //private basic movement
    private Vector3 movementSmooth = Vector3.zero;
    private Vector3 airMovementSmoothValueInAir;
    private Transform currentGround;
    private Vector2 moveInput;
    private bool isFalling;
    private bool _hasFired;
    Vector3 velocity;
     private bool isJumping;
    [SerializeField] private bool isGrounded;

    //crouching
    [Header("Crouching")]
    [Tooltip("")]
    [SerializeField] private float standHeight = 2f;
    [Tooltip("")]
    [SerializeField] private float crouchHeight = 1f;
    private bool canGoUp;

    Vector3 crouchCenterAdjusted;
    Vector3 standCenterAdjusted;

    [Tooltip("")]
    [SerializeField] private float cameraHeight;
    [Tooltip("")]
    [SerializeField] private float crouchSmoothTime = 1f;
    private float velocityCrouch;
    private float velocityCrouchOther;
    //groundcheck
    [SerializeField] private LayerMask roofLayers;
    [SerializeField] private float roofCheckDistance = 3f;
    //[SerializeField] private float groundCheckRadius;
    private float roofCheckStart;

    



    //animation
    public Vector3 movementVectorForAnim;


    

    

    //private Vector3 movement;


    //Transform mTransform;




    RaycastHit groundInfo;


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
        fpsPlayer = new FPSPlayer();
        
        fpsPlayer.Player.Run.performed += Run;
        fpsPlayer.Player.Run.started += Run;
        fpsPlayer.Player.Run.canceled += Run;

        fpsPlayer.Player.Crouch.performed += Crouch;
        fpsPlayer.Player.Crouch.started += Crouch;
        fpsPlayer.Player.Crouch.canceled += Crouch;

        fpsPlayer.Player.Movement.performed += Move;
        fpsPlayer.Player.Movement.canceled += Reset;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        AssignAnimationIDs();


        if (_animator == null)
            Debug.LogError("Animator reference is null.");
    }

    void Update()
    {
        GroundCheck();
        _currentState.UpdateStates();

        moveInput = GetPlayerMovement();
        //characterController.Move(Velocity * Time.deltaTime);
        HandleAnimStateMachine();

    }

    private void HandleAnimStateMachine()
    {
        _animator.SetFloat(_xVelHash, CurrentInputVector.x);
        _animator.SetFloat(_yVelHash, CurrentInputVector.y);
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

        if (characterController.isGrounded)
        {
            isGrounded = true;
            //isJumping = false;

        }
        else
        {
            isGrounded = false;

        }

    }

    private void OnEnable()
    {
        fpsPlayer.Enable();
    }
    private void OnDisable()
    {
        fpsPlayer.Disable();
    }

    void HoldToCrouch()
    {
        if (holdToCrouch)
        {
            if (GetPlayerCrouching() > 0)
            {
                IsCrouching = true;
                //_vectorMultiplier = 0.5f;
                //Debug.Log("vectorMultiplier = 0.5");
            }
            else
            {
                IsCrouching = false;
                //_vectorMultiplier = 1f;
            }
        }
    }
    
    public void Reset(InputAction.CallbackContext context)
    {
        isRunning = false;
        isMoving = false;
    }
    void Move(InputAction.CallbackContext context)
    {
        isMoving = true;
    }
    public Vector2 GetPlayerMovement()
    {

        Vector2 input = fpsPlayer.Player.Movement.ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref airMovementSmoothValueInLand, controlSmoothment);
        return currentInputVector;
    }

    public Vector2 GetMouseDelta()
    {
        Vector2 input = fpsPlayer.Player.Look.ReadValue<Vector2>();
        Vector2 mouseSmooth = new Vector2(0, 0);
        currentInputVectorForMouse = Vector2.SmoothDamp(currentInputVectorForMouse, input, ref mouseSmooth, mouseSmoothment * 0.1f);
        currentInputVectorForMouse = currentInputVectorForMouse * mouseSensitivity;
        return currentInputVectorForMouse;
        //return fpsPlayer.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJump()
    {

        return fpsPlayer.Player.Jump.triggered;

    }



    public void Crouch(InputAction.CallbackContext ctx)
    {
        if (!holdToCrouch)
        {
            if (ctx.started && !IsCrouching)
            {
                //_vectorMultiplier = 0.5f;
                IsCrouching = true;
            }

            else if (ctx.started && IsCrouching)
            {
                IsCrouching = false;
                //_vectorMultiplier = 1f;
            }
        }
    }
    public float GetPlayerCrouching()
    {
        return fpsPlayer.Player.Crouch.ReadValue<float>();
    }
    public float GetPlayerRunning()
    {
        return fpsPlayer.Player.Run.ReadValue<float>();
    }
    public void Run(InputAction.CallbackContext context)
    {
        if (!holdToRun)
        {
            if (context.started && !isRunning && !IsCrouching)
            {
                isRunning = true;
                //_vectorMultiplier = 2f;
            }

            else if (context.started && isRunning)
            {
                isRunning = false;
                //_vectorMultiplier = 1f;
            }
        }
    }
    void HoldToRun()
    {
        if (holdToRun)
        {
            if (GetPlayerRunning() > 0)
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }

        }
    }
}
