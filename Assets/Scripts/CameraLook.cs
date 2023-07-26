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

    [Tooltip("The camera follow should go here, this is used to calculate where the camera is currently facing")]
    [SerializeField] private Transform _forward;

    [Tooltip("How fast should the camera rotate?")]
    public float rotationSpeed = 1.0f;

    [Tooltip("dunno")]
    public float rotationThreshold = 60f;

    [Tooltip("The object which contains ONLY the player's body object, and not the camera follow")]
    [SerializeField] private Transform _playerBody;

    private float _desiredRotation;
    private PlayerInputHandler _playerInputHandler;
    private bool _isRotating = false;
    private float _cinemachineTargetPitch;
    private float _cinemachineTargetYaw;
    private float _rotationVelocity;

    void Awake()
    {
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void LateUpdate()
    {
        CameraRotation();
    }
    

    public void CameraRotation()
    {
        _cinemachineTargetYaw += _playerInputHandler.GetMouseDeltaInput.x * rotationSpeed * Time.deltaTime;
        _cinemachineTargetPitch += _playerInputHandler.GetMouseDeltaInput.y * rotationSpeed * -1f * Time.deltaTime;
        _rotationVelocity = _playerInputHandler.GetMouseDeltaInput.x * rotationSpeed;
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        
        _forward.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);

        // rotate to face input direction relative to camera position
        //transform.Rotate(Vector3.up * _playerInputHandler.GetMouseDeltaInput.x * rotationSpeed * Time.deltaTime);
        float yRotation = _forward.rotation.eulerAngles.y;
        Quaternion cameraRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRotation, transform.rotation.eulerAngles.z);
        transform.rotation = cameraRotation;
    }


    public void RotateLeftAndRight()
    {
        transform.Rotate(Vector3.up * _cinemachineTargetYaw);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
