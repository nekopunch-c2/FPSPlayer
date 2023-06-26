using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class HeadBobbing : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera runningCam;
    [SerializeField] CinemachineVirtualCamera walkingCam;
    [SerializeField] CinemachineVirtualCamera notGroundedCam;
    private PlayerControl playerControl;
    [SerializeField] int activatePriority = 15;
    [SerializeField] int standingPriority = 5;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerControl = PlayerControl.Instance;
        playerMovement = GetComponentInParent<PlayerMovement>();
    }


    void Update()
    {

        if (playerControl.IsMoving)
        {
            if (playerControl.IsRunning || playerControl.GetPlayerRunning() > 0)
            {
                runningCam.Priority = activatePriority;
            }
            else if (!playerMovement.isGrounded)
            {
                notGroundedCam.Priority = activatePriority;
            }
            else
            {
                runningCam.Priority = standingPriority;
                walkingCam.Priority = activatePriority;
                notGroundedCam.Priority = standingPriority;
            }
        }
            
        else
        {
            runningCam.Priority = standingPriority;
            walkingCam.Priority = standingPriority;
            notGroundedCam.Priority = standingPriority;
        }

        
    }
}
