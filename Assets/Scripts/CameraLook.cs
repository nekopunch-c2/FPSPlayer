using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private Transform playerBody;

    private PlayerControl playerControl;

    [SerializeField] private Transform forward;

    public float rotationSpeed = 1.0f;

    public float rotationSpeedForBody = 1.0f;

    private bool isRotating = false;
    //private float _threshold = 0.01f;

    private float _cinemachineTargetPitch;
    private float _cinemachineTargetYaw;

    private float _rotationVelocity;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 90.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -90.0f;

    private float desiredRotation;
    //[SerializeField] private Quaternion startRotation;
    public float rotationThreshold = 360f;

    void Awake()
    {
        playerControl = PlayerControl.Instance;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void LateUpdate()
    {
        CameraRotation();
    }
    

    public Quaternion CameraRotation()
    {
        _cinemachineTargetYaw += playerControl.GetMouseDelta().x * rotationSpeed;
        _cinemachineTargetPitch += playerControl.GetMouseDelta().y * rotationSpeed * -1f;
        _rotationVelocity = playerControl.GetMouseDelta().x * rotationSpeed;
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        
        forward.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);

        return forward.rotation;
    }


    public void RotateLeftAndRight()
    {
        /*Vector3 eulerAngles = playerBody.transform.rotation.eulerAngles;
        eulerAngles.y = forward.rotation.y;
        Quaternion modifiedRotation = Quaternion.Euler(eulerAngles);
        playerBody.transform.rotation = modifiedRotation;*/

        playerBody.transform.Rotate(Vector3.up * _rotationVelocity);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
