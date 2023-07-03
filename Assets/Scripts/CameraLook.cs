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
    private PlayerControl _playerControl;
    private bool _isRotating = false;
    private float _cinemachineTargetPitch;
    private float _cinemachineTargetYaw;
    private float _rotationVelocity;

    void Awake()
    {
        _playerControl = PlayerControl.Instance;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void LateUpdate()
    {
        CameraRotation();
    }
    

    public Quaternion CameraRotation()
    {
        _cinemachineTargetYaw += _playerControl.GetMouseDelta().x * rotationSpeed;
        _cinemachineTargetPitch += _playerControl.GetMouseDelta().y * rotationSpeed * -1f;
        _rotationVelocity = _playerControl.GetMouseDelta().x * rotationSpeed;
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        
        _forward.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);

        return _forward.rotation;
    }


    public void RotateLeftAndRight()
    {
        _playerBody.transform.Rotate(Vector3.up * _rotationVelocity);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
