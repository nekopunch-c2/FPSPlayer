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
    private PlayerInputHandler _playerInputHandler;
    private PlayerStateMachine _playerStateMachine;

    void Start()
    {
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        // ENSURING REFERENCES ARE PROPERLY ASSIGNED!
        if (_playerInputHandler == null)
        Debug.LogError("PlayerInputHandler reference is null.");
    }


    void Update()
    {

        if (_playerInputHandler.IsMovingInput)
        {
            if (_playerInputHandler.IsRunningInput)
            {
                _runningCam.Priority = _activatePriority;
            }
            else if (!_playerStateMachine.IsGrounded)
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
