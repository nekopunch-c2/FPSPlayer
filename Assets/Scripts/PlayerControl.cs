using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{

    private static PlayerControl _instance;

    public static PlayerControl Instance
    {
        get { return _instance; }
    }

    //serialized or public variables
    public bool holdToRun;
    public bool holdToCrouch;
    [SerializeField] private float _controlSmoothment = 0.1f;
    [SerializeField] private float _mouseSmoothment = 0.1f;
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private bool _hasFiredOnce;
    [SerializeField] private float _vectorMultiplier;

    //private variables
    private Vector2 _airMovementSmoothValueInLand;
    private Vector2 _currentInputVectorAir;
    private Vector2 _currentInputVectorForMouse;
    private Vector2 _currentInputVector;

    //properties
    public bool IsRunning { get; set; }
    public bool IsCrouching { get; set; }
    public bool IsMoving { get; set; }
    public Vector2 CurrentInputVector
    { 
        get 
        { 
            return _currentInputVector * _vectorMultiplier;
        } 
    }
    //references
    private CharacterController _characterController;
    private FPSPlayer _fpsPlayer;
    private CharacterAnimation _characterAnimation;

    void Awake()
    {

        _characterAnimation = GetComponent<CharacterAnimation>();

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        _fpsPlayer = new FPSPlayer();
        _characterController = GetComponent<CharacterController>();
        _fpsPlayer.Player.Run.performed += Run;
        _fpsPlayer.Player.Run.started += Run;
        _fpsPlayer.Player.Run.canceled += Run;

        _fpsPlayer.Player.Crouch.performed += Crouch;
        _fpsPlayer.Player.Crouch.started += Crouch;
        _fpsPlayer.Player.Crouch.canceled += Crouch;

        _fpsPlayer.Player.Movement.performed += Move;
        _fpsPlayer.Player.Movement.canceled += Reset;
    }

    private void Update()
    {
        HoldToRun();
        HoldToCrouch();
        if (IsRunning)
        {
            _vectorMultiplier = 2f;
        }
        else if (IsCrouching)
        {
            _vectorMultiplier = 0.5f;
        }
        else
        {
            _vectorMultiplier = 1f;
        }

        ///////////////////////////////////
        if (_vectorMultiplier == 2)
        {
            Debug.Log("vectorMultiplier = 2");
        }
        if (_vectorMultiplier == 1)
        {
            Debug.Log("vectorMultiplier = 1");
        }
        if (_vectorMultiplier == 0.5f)
        {
            Debug.Log("vectorMultiplier = 0.5");
        }
        ////////////////////////////////////

        //Debug.Log(GetPlayerMovement());


    }
    private void OnEnable()
    {
        _fpsPlayer.Enable();
    }
    private void OnDisable()
    {
        _fpsPlayer.Disable();
    }

    void HoldToCrouch()
    {
        if(holdToCrouch)
        {
            if (GetPlayerCrouching() > 0)
            {
                IsCrouching = true;
                //Debug.Log("vectorMultiplier = 0.5");
            }
            else
            {
                IsCrouching = false;
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
                IsRunning = true;
                //Debug.Log("vectorMultiplier = 2");
            }
            else
            {
                IsRunning = false;
                //Debug.Log("vectorMultiplier = 1");
            }

        }


    }

    public void Reset(InputAction.CallbackContext context)
    {
        IsRunning = false;
        IsMoving = false;
    }
    void Move(InputAction.CallbackContext context)
    {
            IsMoving = true;
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
            if (ctx.started && !IsCrouching)
            {
                IsCrouching = true;
            }

            else if (ctx.started && IsCrouching)
            {
                IsCrouching = false;
            }
        }    
    }
    public float GetPlayerCrouching()
    {
        return _fpsPlayer.Player.Crouch.ReadValue<float>();
    }
    public float GetPlayerRunning()
    {
        return _fpsPlayer.Player.Run.ReadValue<float>();
    }
    public void Run(InputAction.CallbackContext context)
    {
        if (!holdToRun)
        {
            if (context.started && !IsRunning && !IsCrouching)
            {
                IsRunning = true;
            }

            else if (context.started && IsRunning)
            {
                IsRunning = false;
            }
        }

    }
}
