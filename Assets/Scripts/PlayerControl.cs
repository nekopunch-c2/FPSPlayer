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

    public bool holdToRun;
    public bool holdToCrouch;

    [SerializeField] private float controlSmoothment = 0.1f;
    [SerializeField] private float mouseSmoothment = 0.1f;
    [SerializeField] private float mouseSensitivity;
    private Vector2 airMovementSmoothValueInLand;
    
    private Vector2 currentInputVectorAir;
    private Vector2 currentInputVectorForMouse;

    [SerializeField] private bool hasFiredOnce;
    
    public bool IsRunning { get; set; }
    public bool IsCrouching { get; set; }
    public bool IsMoving { get; set; }

    [SerializeField] private float _vectorMultiplier;

    public Vector2 CurrentInputVector
    { get { return currentInputVector * _vectorMultiplier; } }

    private Vector2 currentInputVector;



    private CharacterController characterController;

    private FPSPlayer fpsPlayer;

    private CharacterAnimation characterAnimation;
    void Awake()
    {

        characterAnimation = GetComponent<CharacterAnimation>();

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        fpsPlayer = new FPSPlayer();
        characterController = GetComponent<CharacterController>();
        fpsPlayer.Player.Run.performed += Run;
        fpsPlayer.Player.Run.started += Run;
        fpsPlayer.Player.Run.canceled += Run;

        fpsPlayer.Player.Crouch.performed += Crouch;
        fpsPlayer.Player.Crouch.started += Crouch;
        fpsPlayer.Player.Crouch.canceled += Crouch;

        fpsPlayer.Player.Movement.performed += Move;
        fpsPlayer.Player.Movement.canceled += Reset;
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
        fpsPlayer.Enable();
    }
    private void OnDisable()
    {
        fpsPlayer.Disable();
    }

    void HoldToCrouch()
    {
        if(holdToCrouch)
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
    void HoldToRun()
    {
        if (holdToRun)
        {
            if (GetPlayerRunning() > 0)
            {
                IsRunning = true;
                //_vectorMultiplier = 2f;
                //Debug.Log("vectorMultiplier = 2");
            }
            else
            {
                IsRunning = false;
                //_vectorMultiplier = 1f;
                //Debug.Log("vectorMultiplier = 1");
            }

        }


    }
    /*public void Accelerate(InputAction.CallbackContext context)
    {
    }*/
    public void Reset(InputAction.CallbackContext context)
    {
        IsRunning = false;
        IsMoving = false;
    }
    void Move(InputAction.CallbackContext context)
    {
        //if (context.started)
            IsMoving = true;
        /*else if (context.canceled)
        {
            IsMoving = false;
        }*/
    }
    public Vector2 GetPlayerMovement()
    {

        /*if (!characterController.isGrounded)
        {
            Vector2 inputAir = fpsPlayer.Player.Movement.ReadValue<Vector2>();
            currentInputVectorAir = Vector2.SmoothDamp(currentInputVectorAir, inputAir, ref airMovementSmoothValueInLand, controlSmoothment);
            return currentInputVectorAir;
        }*/


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
            if (context.started && !IsRunning && !IsCrouching)
            {
                IsRunning = true;
                //_vectorMultiplier = 2f;
            }

            else if (context.started && IsRunning)
            {
                IsRunning = false;
                //_vectorMultiplier = 1f;
            }
        }

    }
}
