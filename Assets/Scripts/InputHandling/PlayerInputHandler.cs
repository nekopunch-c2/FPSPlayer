using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NekoSpace;

public class PlayerInputHandler : MonoBehaviour
{
    //GETSET
    public bool IsRunningInput { get { return _isRunning; } set { _isRunning = value; } }
    public bool IsCrouchingInput { get { return _isCrouching; } set { _isCrouching = value; } }
    public bool IsMovingInput { get { return _isMoving; } set { _isMoving = value; } }
    public float VectorMultiplierInput { get { return _vectorMultiplier; } set { _vectorMultiplier = value; } }
    public bool InputAllowedInput { get { return _inputAllowed; } set { _inputAllowed = value; } }
    public bool CanAnimateInput { get { return _canAnimate; } set { _canAnimate = value; } }
    public float CurrentInputVectorYInput { get { return CurrentInputVector.y; } }
    public float CurrentInputVectorXInput { get { return CurrentInputVector.x; } }
    public Vector2 GetPlayerMovementInput { get { return GetPlayerMovement(); } }
    public Vector2 GetMouseDeltaInput { get { return GetMouseDelta(); } }
    public CameraRotationDirection RotationDirectionInput { get { return _rotationDirection; } }
    public float RotationSpeedInput { get { return _rotationSpeed; } }
    public bool PlayerJumpInput { get { return PlayerJump(); } }
    public bool GetPlayerClimbInput { get { return _isReadyToClimb; } set { _isReadyToClimb = value; } }

    //VARIABLES
    private bool _inputAllowed = false;
    private bool _canAnimate = false; //makes it so that when it's true after a few seconds from start, animations can play
    public bool holdToRun;
    public bool holdToCrouch;

    private bool _isReadyToClimb;

    [SerializeField] private float _controlSmoothment = 0.1f;
    [SerializeField] [Range (0.0f, 5f)] private float _mouseSmoothment = 0.1f;
    [SerializeField][Range(0.3f, 60f)] private float _mouseSensitivity;
    private Vector2 _airMovementSmoothValueInLand;

    private Vector2 _currentInputVectorAir;
    private Vector2 _currentInputVectorForMouse;

    [SerializeField] private bool _hasFiredOnce;

    private bool _isRunning;
    private bool _isCrouching;
    private bool _isMoving;

    private CameraRotationDirection _rotationDirection;
    private float _rotationSpeed;

    private float _vectorMultiplier = 1f;

    public Vector2 CurrentInputVector { get { return _currentInputVector * _vectorMultiplier; } }

    private Vector2 _currentInputVector;

    //REFERENCES
    private FPSPlayer _fpsPlayer;
    
    void Awake()
    {
        StartCoroutine(DelayInputProcessing(2f));
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

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_isReadyToClimb);
        if (_inputAllowed)
        {
            HoldToRun();
            HoldToCrouch();
        }
        if (GetPlayerClimb())
        {
            _isReadyToClimb = true;
        }
        else
        {
            _isReadyToClimb = false;
        }
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
        _isMoving = false;
    }
    void Move(InputAction.CallbackContext context)
    {

        _isMoving = true;
        if(_inputAllowed)
        {
            _canAnimate = true;
        }
    }
    private Vector2 GetPlayerMovement()
    {
        if(_inputAllowed)
        {
            Vector2 input = _fpsPlayer.Player.Movement.ReadValue<Vector2>();
            _currentInputVector = Vector2.SmoothDamp(_currentInputVector, input, ref _airMovementSmoothValueInLand, _controlSmoothment);
            return _currentInputVector;
        }
        return Vector2.zero;
    }

    private Vector2 GetMouseDelta()
    {
        if (_inputAllowed)
        {
            Vector2 input = _fpsPlayer.Player.Look.ReadValue<Vector2>() * _mouseSensitivity;
            Vector2 mouseSmooth = new Vector2(0, 0);
            _currentInputVectorForMouse = Vector2.SmoothDamp(_currentInputVectorForMouse, input, ref mouseSmooth, _mouseSmoothment * 0.1f);

            // Get the X-component of the smoothed mouse input
            float xRotation = _currentInputVectorForMouse.x;

            // Determine the direction of rotation (positive, negative or none)
            if (/*Mathf.Approximately(xRotation, 0f)*/  xRotation < 10f && xRotation > -10f)
            {
                _rotationDirection = CameraRotationDirection.None;
            }
            else
            {
                _rotationDirection = xRotation > 0f ? CameraRotationDirection.Right : CameraRotationDirection.Left;
            }

            // Get the speed of rotation (magnitude of the X-component)
            _rotationSpeed = Mathf.Abs(xRotation);

            return _currentInputVectorForMouse;
        }
        _rotationDirection = CameraRotationDirection.None;
        return Vector2.zero;
    }


    private bool PlayerJump()
    {
        if (_inputAllowed)
        {
            return _fpsPlayer.Player.Jump.triggered;
        }
        return false;
    }

    private bool GetPlayerClimb()
    {
        if(_inputAllowed)
        {
            return _fpsPlayer.Player.Climb.triggered;
        }
        return false;
    }

    private void Crouch(InputAction.CallbackContext ctx)
    {
        if (_inputAllowed)
        {
            if (!holdToCrouch)
            {
                if (ctx.started && !_isCrouching)
                {
                    _isCrouching = true;
                }

                else if (ctx.started && _isCrouching)
                {
                    _isCrouching = false;
                }
            }
        }
    }
    private float GetPlayerCrouching()
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

    private void Run(InputAction.CallbackContext context)
    {
        if(_inputAllowed)
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
    }
    private float GetPlayerRunning()
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
