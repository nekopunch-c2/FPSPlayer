using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    //ANIMATION
    //private variables
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDInAir;
    private int _animIDLanding;
    private int _animIDCrouching;

    private int _xVelHash;
    private int _yVelHash;

    private int _turningAngleHash;

    private int _turningLeftHash;

    private float _animationBlend;

    //properties
    public bool AnimationsPlayed { get; set; }

    //REFERENCES
    private PlayerControl playerControl;

    private RotateBodyToHorizon rotateBodyToHorizon;

    private CameraLook cameraLook;

    private Animator _animator;

    private PlayerMovement playerMovement;


    void Start()
    {

        //getting references
        _animator = GetComponentInChildren<Animator>();
        playerControl = PlayerControl.Instance;
        rotateBodyToHorizon = GetComponent<RotateBodyToHorizon>();
        cameraLook = GameObject.FindWithTag("CameraManager").GetComponent<CameraLook>();
        playerMovement = GetComponent<PlayerMovement>();

        //asssigning animation IDs
        AssignAnimationIDs();

        // ENSURING REFERENCES ARE PROPERLY ASSIGNED
        if (_animator == null)
            Debug.LogError("Animator reference is null.");

        if (playerControl == null)
            Debug.LogError("PlayerControl reference is null.");

        if (rotateBodyToHorizon == null)
            Debug.LogError("RotateBodyToHorizon reference is null.");

        if(cameraLook == null)
            Debug.LogError("CameraLook reference is null.");

        if(playerMovement == null)
            Debug.LogError("PlayerMovement reference is null.");

    }
    void Update()
    {
        HandleAnim();
    }

    void HandleAnim()
    {
        _animator.SetFloat(_xVelHash, playerControl.CurrentInputVector.x);
        _animator.SetFloat(_yVelHash, playerControl.CurrentInputVector.y);

        //turning
        if (rotateBodyToHorizon.IsTurning)
        {
            if (!AnimationsPlayed)
            {
                float normalizedAngle = Mathf.Clamp01(rotateBodyToHorizon.targetRotation.y);
                // Trigger the appropriate blend tree animation based on the turn angle
                if (rotateBodyToHorizon.targetRotation.y < 0f)
                {

                    _animator.SetFloat(_turningAngleHash, normalizedAngle);

                    _animator.CrossFadeInFixedTime("TurnLeft", 0.3f);
                    normalizedAngle = 0f;
                }
                else if (rotateBodyToHorizon.targetRotation.y > 0f)
                {
                    _animator.SetFloat(_turningAngleHash, normalizedAngle);


                    _animator.CrossFadeInFixedTime("TurnLeft", 0.3f);
                    normalizedAngle = 0f;
                }
                AnimationsPlayed = true;
            }
        }
        else
        {
            AnimationsPlayed = false;
        }

        //crouching
        if (playerControl.IsCrouching)
        {
            _animator.SetBool(_animIDCrouching, true);
        }
        else
        {
            _animator.SetBool(_animIDCrouching, false);
        }
        //jumping
        if (playerControl.PlayerJump())
        {
            _animator.CrossFadeInFixedTime("JumpStart", 0.1f);
        }
        
        if (playerMovement.isGrounded)
        {
            _animator.SetBool(_animIDInAir, false);
            _animator.SetBool(_animIDLanding, true);
            _animator.SetBool(_animIDGrounded, true);
        }
        else
        {
            _animator.SetBool(_animIDInAir, true);
            _animator.SetBool(_animIDGrounded, false);
            _animator.SetBool(_animIDLanding, false);
        }
    }

    private void AssignAnimationIDs()
    {
        _xVelHash = Animator.StringToHash("x_velocity");
        _yVelHash = Animator.StringToHash("y_velocity");

        _turningAngleHash = Animator.StringToHash("turnAngle_velocity");
        _turningLeftHash = Animator.StringToHash("left_velocity");
        _animIDCrouching = Animator.StringToHash("Crouching");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDInAir = Animator.StringToHash("InAir");
        _animIDLanding = Animator.StringToHash("Landed");
    }
}
//_animator.SetBool(_animIDJump, false);
//_animator.SetBool(_animIDFreeFall, false);

//_animator.SetBool(_animIDJump, true);

//_animator.SetBool(_animIDFreeFall, true);