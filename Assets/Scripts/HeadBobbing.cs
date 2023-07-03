using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class HeadBobbing : MonoBehaviour
{
    //serialized fields
    [SerializeField] CinemachineVirtualCamera _runningCam;
    [SerializeField] CinemachineVirtualCamera _walkingCam;
    [SerializeField] CinemachineVirtualCamera _notGroundedCam;
    [SerializeField] private int _activatePriority = 15;
    [SerializeField] private int _standingPriority = 5;

    //references
    private PlayerMovement _playerMovement;
    private PlayerControl _playerControl;

    void Start()
    {
        _playerControl = PlayerControl.Instance;
        _playerMovement = GetComponentInParent<PlayerMovement>();
        // ENSURING REFERENCES ARE PROPERLY ASSIGNED!
        if (_playerControl == null)
            Debug.LogError("PlayerControl reference is null.");

        if (_playerMovement == null)
            Debug.LogError("PlayerMovement reference is null.");
    }


    void Update()
    {

        if (_playerControl.IsMoving)
        {
            if (_playerControl.IsRunning || _playerControl.GetPlayerRunning() > 0)
            {
                _runningCam.Priority = _activatePriority;
            }
            else if (!_playerMovement.isGrounded)
            {
                _notGroundedCam.Priority = _activatePriority;
            }
            else
            {
                _runningCam.Priority = _standingPriority;
                _walkingCam.Priority = _activatePriority;
                _notGroundedCam.Priority = _standingPriority;
            }
        }
            
        else
        {
            _runningCam.Priority = _standingPriority;
            _walkingCam.Priority = _standingPriority;
            _notGroundedCam.Priority = _standingPriority;
        }

        
    }
}
