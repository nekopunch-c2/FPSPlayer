using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;




    //stairs
    [Header("Stairs And Slopes")]
    [Tooltip("this value will be used to find the current ground and determine if it's tagged 'stairs'. Higher values will work for more situations, but extreme ones will consume too many resources")]
    [SerializeField] private float stairsCheckLength = 0.1f;
    [Tooltip("this value will be used, when in the air, to determine how far the ground is from the point the player is at. Higher values will work for more situations, but extreme ones will consume too many resources")]
    [SerializeField] private float lengthFromGround;
    [Tooltip("to prevent the bouncing that happens to most first person controllers when going down slopes or stairs, gravity is multiplied by this value. Higher values work for more situations, but extreme ones risk the player clipping through the floor.")]
    [SerializeField] private float slopeAndStairForce;
    [Tooltip("this value will be used to determine the surface the player is standing on is a slope (using normals).")]
    [SerializeField] private float slopeForceRayLength;

    


    [Header("Basic Movement")]
    [SerializeField] private float speed = 10f;
    [Tooltip("how much should the player accelerate when pulled by gravity? A value around -30 is not too floaty, but doesn't snap you right down either.")]
    [SerializeField] private float gravity = -9.81f;
    [Tooltip("how far should the player jump before gravity starts pulling it down.")]
    [SerializeField] private float jumpHeight = 2f;
    [Tooltip("the 'speed' value will be multiplied by this value when running is toggled.")]
    [SerializeField] private float speedMultiplier;
    [Tooltip("the 'speed' value will be multiplied by this value when crouching is toggled.")]
    [SerializeField] private float crouchSpeedMultiplier;
    [Tooltip("how smooth movement in the air should be. This is used both for in air movement as well as for keeping momentum")]
    [SerializeField] private float airSmoothment = 0.6f;

    //crouching
    [Header("Crouching")]
    [Tooltip("")]
    [SerializeField] private float standHeight = 2f;
    [Tooltip("")]
    [SerializeField] private float crouchHeight = 1f;
    [Tooltip("")]
    [SerializeField] private float standHeightCamera;
    [Tooltip("")]
    [SerializeField] private float crouchHeightCamera;
    private bool canGoUp;

    Vector3 crouchCenterAdjusted;
    Vector3 standCenterAdjusted;

    [Tooltip("")]
    [SerializeField] private float cameraHeight;
    [Tooltip("")]
    [SerializeField] private float crouchSmoothTime = 1f;
    private float velocityCrouch;
    private float velocityCrouchOther;
    //groundcheck
    [SerializeField] private LayerMask roofLayers;
    [SerializeField] private float roofCheckDistance = 3f;
    //[SerializeField] private float groundCheckRadius;
    private float roofCheckStart;

    private Vector3 MovementSmooth = Vector3.zero;
    private Vector3 airMovementSmoothValueInAir;
    private Transform currentGround;
    private Vector2 moveInput;
    private bool isFalling;
    private bool _hasFired;



    //animation
    public Vector3 movementVectorForAnim;


    Vector3 velocity;

    private bool isJumping;


    //private Vector3 movement;


    //Transform mTransform;

    [System.NonSerialized] public bool isGrounded;


    RaycastHit groundInfo;

    private PlayerControl playerControl;

    private Transform cameraTransform;

    void Awake()
    {
        OnValidate();
    }

    void OnValidate()
    {
        crouchCenterAdjusted.y = crouchHeight / 2f;
        standCenterAdjusted.y = standHeight / 2f;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        cameraTransform = GameObject.FindWithTag("CameraFollow").transform;
        playerControl = PlayerControl.Instance;

        cameraHeight = cameraTransform.localPosition.y;
        //mTransform = transform;

    }


    void Update()
    {
        //currentPosition = transform.position;
        
        GroundCheck();
        RoofCheck();
        HandleJump();
        HandleMovement();
        GroundFarEnough();
        HandleGravity();
        OnSlope();
        /*if (isFalling)
        {
            Debug.Log("is fallinf");
        }
        else
        {
            Debug.Log("is not falling, therfore probably stairs");
        }*/

    }


    private bool OnStairs()
    {
        if (!isGrounded && !isJumping && !isFalling)
        {
            return true;
        }
        
        return false;
    }
    private bool OnStairsTag()
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position * (characterController.height / 2), Vector3.down, out hit, stairsCheckLength))
        {
            currentGround = hit.transform;
            if (!isJumping && currentGround.tag == "Stairs" && !isFalling)
            {
                //Debug.Log("is on stairs via tag");
                return true;
            }
        }
        return false;
    }

    private bool OnSlope()
    {
        if(!isGrounded)
        {
            return false;
        }
        RaycastHit hit;

        if(Physics.Raycast(transform.position * (characterController.height / 2), Vector3.down, out hit, characterController.height / 2 * slopeForceRayLength))
        {
            if(hit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }

    private void GroundFarEnough()
    {
        
        if (!isGrounded && !_hasFired)
        {
            _hasFired = true;
            RaycastHit hit;

            if (Physics.SphereCast(transform.position * (characterController.height / 2), 1f, Vector3.down, out hit, lengthFromGround))
            {
                
                //Debug.Log(hit.distance);
                if (hit.distance >= 0.001f)
                {
                    isFalling = true;
                    
                }
                else
                {
                    Debug.Log(hit.distance);
                    isFalling = false;
                }
            }
        }
        
        else if (isGrounded)
        {
            _hasFired = false;
            isFalling = false;
        }
    }


    void HandleMovement()
    {
        moveInput = playerControl.GetPlayerMovement();
        Vector3 movement = (moveInput.y * transform.forward) + (moveInput.x * transform.right);
        /*movement = cameraTransform.forward * movement.z + cameraTransform.right * moveInput.x;
        movement.y = 0f;*/
        
            if (playerControl.IsRunning && isGrounded)
            {
                movement = movement * speedMultiplier;
            }
        


        //if (playerControl.holdToCrouch)
        //{
            if (playerControl.IsCrouching && isGrounded)
            {
                //charcontroller
                movement = movement * crouchSpeedMultiplier;
                float crouching = Mathf.SmoothDamp(characterController.height, crouchHeight, ref velocityCrouch, crouchSmoothTime);
                characterController.height = crouching;

                Vector3 crouchingCenter = Vector3.SmoothDamp(characterController.center, crouchCenterAdjusted, ref MovementSmooth, crouchSmoothTime);
                characterController.center = crouchingCenter;

                //camera
                //cameraHeight = Mathf.SmoothDamp(cameraTransform.localPosition.y, crouchHeightCamera, ref velocityCrouchOther, crouchSmoothTime);

                //cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, cameraHeight, cameraTransform.localPosition.z);
            }
            else if (canGoUp)
            {
                float standing = Mathf.SmoothDamp(characterController.height, standHeight, ref velocityCrouch, crouchSmoothTime);
                characterController.height = standing;
                Vector3 standingCenter = Vector3.SmoothDamp(characterController.center, standCenterAdjusted, ref MovementSmooth, crouchSmoothTime);
                characterController.center = standingCenter;

                //camera
                //cameraHeight = Mathf.SmoothDamp(cameraTransform.localPosition.y, standHeightCamera, ref velocityCrouchOther, crouchSmoothTime);

                //cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, cameraHeight, cameraTransform.localPosition.z);
            }
        //}
        /*else
        {
            if (playerControl.IsCrouching && isGrounded)
            {
                movement = movement * crouchSpeedMultiplier;
            }
        }*/

        //Debug.Log(playerControl.GetPlayerRunning());
        

        if (!isGrounded)
        {
            

            airMovementSmoothValueInAir = Vector3.SmoothDamp(airMovementSmoothValueInAir, movement, ref MovementSmooth, airSmoothment);
            characterController.Move(airMovementSmoothValueInAir * speed * Time.deltaTime);
        }
        else
        {
            airMovementSmoothValueInAir = movement;

            characterController.Move(movement * speed * Time.deltaTime);
        }
        // Debug.Log(airMovementSmoothValueInAir);
        movementVectorForAnim = movement;
    }

    void RoofCheck()
    {
        Ray ray = new Ray(transform.position, transform.up);
        if (Physics.Raycast(ray, out groundInfo, roofCheckDistance, roofLayers, QueryTriggerInteraction.Ignore))
        {
            canGoUp = false;
            Debug.DrawRay(transform.position, transform.up * roofCheckDistance);
        }
        else
        {
            canGoUp = true;
        }

    }

    void GroundCheck()
    {
        
        //Ray ray = new Ray(transform.position + Vector3.up * groundCheckStart, Vector3.down);
        if /*(Physics.SphereCast(ray, groundCheckRadius, out groundInfo, groundCheckDistance, groundLayers, QueryTriggerInteraction.Ignore))*/ (characterController.isGrounded)
        {
            isGrounded = true;
            isJumping = false;
            
        }
        else
        {
            isGrounded = false;
            
        }
        
    }
    void HandleGravity()
    {

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }



        if ((moveInput.x != 0 || moveInput.y != 0) && (OnStairsTag() || OnSlope()))
        {
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * slopeAndStairForce * Time.deltaTime);
            //Debug.Log("on stairs or slope via tag");
        }
        else if (/*(moveInput.x != 0 || moveInput.y != 0) && */(OnStairs() || OnSlope()))
        {
           velocity.y += gravity * Time.deltaTime;
           characterController.Move(velocity * slopeAndStairForce * Time.deltaTime);
           //Debug.Log("on stairs or slope via grouncheck");
        }

        else
        {
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
            //Debug.Log("not on stairs or slope");
        }

        if (characterController.collisionFlags == CollisionFlags.Above && !isGrounded)
        {
            velocity.y += gravity * Time.deltaTime * 10;
            characterController.Move(velocity * Time.deltaTime);
        }

    }

    void HandleJump()
    {

        if (playerControl.PlayerJump() && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;
            isGrounded = false;
        }
       
    }

    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position * (characterController.height / 2), 1f);
            
            Gizmos.DrawRay(transform.position * (characterController.height / 2), Vector3.down * lengthFromGround);
        
    }*/
}
