using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    //VARIABLES
    private bool _inputAllowed = false;
    private bool _canAnimate = false; //makes it so that when it's true after a few seconds from start, animations can play
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
    private bool _isCrouching;
    private bool _isMoving;

    private float _vectorMultiplier = 1f;

    public Vector2 CurrentInputVector { get { return _currentInputVector * _vectorMultiplier; } }

    private Vector2 _currentInputVector;

    //REFERENCES
    private FPSPlayer _fpsPlayer;
    
    void Awake()
    {
        StartCoroutine(DelayInputProcessing(0.1f));
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
       HoldToRun();
       HoldToCrouch();
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
        _canAnimate = true;
        

    }
    public Vector2 GetPlayerMovement()
    {
        Vector2 input = _fpsPlayer.Player.Movement.ReadValue<Vector2>();
        _currentInputVector = Vector2.SmoothDamp(_currentInputVector, input, ref _airMovementSmoothValueInLand, _controlSmoothment);
        return _currentInputVector;

    }

    public Vector2 GetMouseDelta()
    {
        Vector2 input = _fpsPlayer.Player.Look.ReadValue<Vector2>();
        Vector2 mouseSmooth = new Vector2(0, 0);
        _currentInputVectorForMouse = Vector2.SmoothDamp(_currentInputVectorForMouse, input, ref mouseSmooth, _mouseSmoothment * 0.1f);
        _currentInputVectorForMouse = _currentInputVectorForMouse * _mouseSensitivity;
        return _currentInputVectorForMouse;
    }

    public bool PlayerJump()
    {
        return _fpsPlayer.Player.Jump.triggered;
    }



    public void Crouch(InputAction.CallbackContext ctx)
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
