using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{


    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 90.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -90.0f;

    [Tooltip("How far in degrees can you move the camera when on a ladder")]
    public float MaxLadderYaw = 60f;


    [Tooltip("The camera follow should go here, this is used to calculate where the camera is currently facing")]
    [SerializeField] private Transform _forward;

    [Tooltip("How fast should the camera rotate?")]
    public float rotationSpeed = 1.0f;

    [Tooltip("dunno")]
    public float rotationThreshold = 60f;

    [Tooltip("The object which contains ONLY the player's body object, and not the camera follow")]
    [SerializeField] private Transform _playerBody;

    private float _desiredRotation;
    //private PlayerInputHandler _playerInputHandler;
    private bool _isRotating = false;
    public float _cinemachineTargetPitch;
    public float _cinemachineTargetYaw;
    private float _rotationVelocity;

    private bool _isOnLadder = false;
    private bool _isOnLadderTransition = false;
    private float _initialYaw;
    private float _initialPitch;
    private float _currentYaw;
    public float _yawDifference;
    public float _pitchDifference;
    private float _newYaw;
    private float _newPitch;

    private Vector2 rotation = Vector2.zero;


    [Header("Settings")]
    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private float pitchClampAngle = 80f;

    private Vector2 lookInput;
    public float pitch = 0f;
    public float yaw = 0f;
    //references
    private PlayerStateMachine playerStateMachine;
    private ICameraRotationBehavior _cameraRotationBehavior;

    void Awake()
    {
        //_playerInputHandler = GetComponent<PlayerInputHandler>();
        _cameraRotationBehavior = GetComponent<PlayerInputHandler>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }
    private void LateUpdate()
    {
        CameraRotation();
    }
    

    public void CameraRotation()
    {
        _cinemachineTargetPitch += _cameraRotationBehavior.GetMouseDelta().y * rotationSpeed * -1f * Time.deltaTime;
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
        if (playerStateMachine.CurrentState == playerStateMachine.States.OnLadder())
        {
            if (!_isOnLadder)
            {
                _isOnLadder = true;
                _initialYaw = _cinemachineTargetYaw;
                _currentYaw = _cinemachineTargetYaw; // Initialize the currentYaw with the initialYaw
            }

            // Calculate the desired delta yaw based on the player's input and rotation speed
            float deltaYaw = _cameraRotationBehavior.GetMouseDelta().x * rotationSpeed * Time.deltaTime;

            // Add the delta yaw to the currentYaw
            _currentYaw += deltaYaw;

            // Clamp the currentYaw to the desired range
            float minAngle = _initialYaw - MaxLadderYaw;
            float maxAngle = _initialYaw + MaxLadderYaw;
            _currentYaw = Mathf.Clamp(_currentYaw, minAngle, maxAngle);

            // Apply the adjusted yaw to the camera rotation
            _cinemachineTargetYaw = _currentYaw;
        }
        else
        {

            _isOnLadder = false;
                
            
            _cinemachineTargetYaw += _cameraRotationBehavior.GetMouseDelta().x * rotationSpeed * Time.deltaTime + _yawDifference;
        }

        _forward.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
        float yRotation = _forward.rotation.eulerAngles.y;
        Quaternion cameraRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRotation, transform.rotation.eulerAngles.z);

        if (playerStateMachine.CurrentState != playerStateMachine.States.OnLadder() && playerStateMachine.CurrentState != playerStateMachine.States.LadderTransition())
        {
            transform.rotation = cameraRotation;
        }
        if (playerStateMachine.CurrentState == playerStateMachine.States.LadderTransition())
        {
            playerStateMachine.InputAllowed = false;
            if (!_isOnLadderTransition)
            {
                _isOnLadderTransition = true;
            }
            _yawDifference = _playerBody.eulerAngles.y - _forward.eulerAngles.y;
            _pitchDifference = _playerBody.eulerAngles.x - _forward.eulerAngles.x;
            _forward.rotation = Quaternion.Euler(0f, _playerBody.eulerAngles.y, 0f);
        }
        else
        {
            playerStateMachine.InputAllowed = true;

        }


    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
