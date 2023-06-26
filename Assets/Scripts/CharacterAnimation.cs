using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    //animation
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDInAir;
    private int _animIDLanding;
    private int _animIDMotionSpeed;
    private int _animIDCrouching;

    private int _xVelHash;
    private int _yVelHash;

    private int _turningAngleHash;

    private int _turningLeftHash;

    private float _animationBlend;

    public bool animationsPlayed { get; set; }

    //jump timeout
    public float jumpTimeout { get; set; }
    public float jumpTimerFull = 0.3f;
    //land timeout
    public float landTimeout { get; set; }
    public float landTimerFull = 0.15f;

    //references
    private PlayerControl playerControl;

    private RotateBodyToHorizon rotateBodyToHorizon;

    private CameraLook cameraLook;

    private Animator _animator;

    private PlayerMovement playerMovement;


    void Start()
    {
        //jump timeout
        jumpTimeout = jumpTimerFull;

        //land timeout
        landTimeout = landTimerFull;

        //getting references
        _animator = GetComponentInChildren<Animator>();
        playerControl = PlayerControl.Instance;
        rotateBodyToHorizon = GetComponent<RotateBodyToHorizon>();
        cameraLook = GameObject.FindWithTag("CameraManager").GetComponent<CameraLook>();
        playerMovement = GetComponent<PlayerMovement>();

        //asssigning animation IDs
        AssignAnimationIDs();

        // ensuring that the references are properly assigned
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
        
        //Debug.Log("Cibi");
        _animator.SetFloat(_xVelHash, playerControl.CurrentInputVector.x);
        _animator.SetFloat(_yVelHash, playerControl.CurrentInputVector.y);
        //Debug.Log(rotateBodyToHorizon.targetRotation.y);


        if (rotateBodyToHorizon.isTurning)
        {
            if (!animationsPlayed)
            {
                //_animator.SetFloat(_turningAngleHash, rotateBodyToHorizon.HeadRotationAngle);
                float normalizedAngle = Mathf.Clamp01(rotateBodyToHorizon.targetRotation.y);
                // Trigger the appropriate blend tree animation based on the turn angle
                if (rotateBodyToHorizon.targetRotation.y < 0f)
                {

                    _animator.SetFloat(_turningAngleHash, normalizedAngle);
                    /*_animator.IsInTransition(0));
                    {
                        return;
                    }*/

                    _animator.CrossFadeInFixedTime("TurnLeft", 0.3f);
                    //_animator.Play("TurnLeft");
                    normalizedAngle = 0f;
                }
                else if (rotateBodyToHorizon.targetRotation.y > 0f)
                {
                    _animator.SetFloat(_turningAngleHash, normalizedAngle);
                    /*_animator.IsInTransition(0));
                    {
                        return;
                    }*/

                    _animator.CrossFadeInFixedTime("TurnLeft", 0.3f);
                    //_animator.Play("TurnRight");
                    normalizedAngle = 0f;
                }
                animationsPlayed = true;
            }
        }
        else
        {
            animationsPlayed = false;
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
        //_animator.SetFloat(_turningLeftHash, playerControl.CurrentInputVector.y);
        if (playerControl.PlayerJump())
        {
            jumpTimeout -= Time.deltaTime; //subtract from jump timeout over time
            _animator.CrossFadeInFixedTime("JumpStart", 0.1f);
        }
        
        if (playerMovement.isGrounded)
        {
            _animator.SetBool(_animIDInAir, false);
            _animator.SetBool(_animIDLanding, true);

            /*landTimeout -= Time.deltaTime; //subtract from land timeout over time
            
            if (!playerControl.PlayerJump() || landTimeout == 0)
            {*/
            _animator.SetBool(_animIDGrounded, true);
                
                //landTimeout = landTimerFull; //reset land timeout
            //}
        }
        else
        {
            /*if (landTimeout >= 0.0f)
            {
                landTimeout -= Time.deltaTime;
            }
            else
            {           
                _animator.SetBool(_animIDInAir, true);
                _animator.SetBool(_animIDGrounded, false);
                _animator.SetBool(_animIDLanding, false);
            }
            //_animator.SetBool(_animIDInAir, true);
            */
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
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }
}
//_animator.SetBool(_animIDJump, false);
//_animator.SetBool(_animIDFreeFall, false);

//_animator.SetBool(_animIDJump, true);

//_animator.SetBool(_animIDFreeFall, true);