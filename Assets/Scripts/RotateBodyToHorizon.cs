using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBodyToHorizon : MonoBehaviour
{
    //public variables
    public Transform playerBody; // Transform representing the player's body
    public Transform targetTransform; // Transform representing the target to rotate towards
    public Quaternion targetRotation; // Target rotation to match
    public float headRotationThreshold = 60f; // Threshold for head rotation in degrees
    public float bodyRotationSpeed = 5f; // Speed of body rotation
    public float rotationThreshold = 90f; //From this threshold on, the player will smoothly rotate instead of lerping

    //private variables
    private PlayerControl playerControl;
    private Quaternion originalBodyRotation; // Original rotation of the player's body
    private bool isBodyTurning; // Flag to track if the body is turning
    private float headRotationAngle; //angle that the head has rotated
    private Quaternion cameraForwardRotation; //stores the current forward rotation of the camera

    //properties
    public bool IsTurning { get; set; }


    //REFERENCES
    //camera manager
    private CameraAngularSpeed cameraAngularSpeed;
    private CameraLook cameraLook; //Reference to CameraLook scrupt
    //character animation
    private CharacterAnimation characterAnimation;
    //player movement
    private PlayerMovement playerMovement;


    
    void Start()
    {
        //GETTING REFERENCES

        //player control
        playerControl = PlayerControl.Instance;

        //under camera manager
        //camera look
        cameraLook = GameObject.FindWithTag("CameraManager").GetComponent<CameraLook>();
        //camera angular speed
        cameraAngularSpeed = GameObject.FindWithTag("CameraManager").GetComponent<CameraAngularSpeed>();

        //characteranimation
        characterAnimation = GetComponent<CharacterAnimation>();

        //player movement
        playerMovement = GetComponent<PlayerMovement>();

        //STUFF TO DO AT START
        originalBodyRotation = playerBody.rotation;
        targetRotation = playerBody.rotation;
        cameraForwardRotation = cameraLook.CameraRotation();

        // ENSURING REFERENCES ARE PROPERLY ASSIGNED!
        if (characterAnimation == null)
            Debug.LogError("Animator reference is null.");

        if (playerControl == null)
            Debug.LogError("PlayerControl reference is null.");

        if (cameraLook == null)
            Debug.LogError("CameraLook reference is null.");

    }

    private void Update()
    {
        // Check if the player is standing still
        if (playerControl.GetPlayerMovement() == Vector2.zero || !playerMovement.isGrounded)
        {
            // Calculate the rotation angle around the Y-axis for the target
            float targetRotationAngle = targetTransform.eulerAngles.y;

            // Calculate the rotation angle around the Y-axis for the head
            headRotationAngle = targetRotationAngle - playerBody.eulerAngles.y;

            // Wrap the head rotation angle to the range of -180 to 180 degrees
            if (headRotationAngle > 180f)
                headRotationAngle -= 360f;
            else if (headRotationAngle < -180f)
                headRotationAngle += 360f;

            // Check if the head rotation exceeds the threshold

            /*if (Mathf.Abs(headRotationAngle) > rotationThreshold)
            {
                // Rotate the entire body instantly
                //cameraLook.RotateLeftAndRight();
                cameraLook.RotateLeftAndRight();
            }*/
            //isTurning = true;
            //cameraLook.RotateLeftAndRight();
                
            
            
            
            if (Mathf.Abs(headRotationAngle) > headRotationThreshold && !isBodyTurning)
            {
                // Set the new target rotation
                targetRotation *= Quaternion.Euler(0f, headRotationAngle, 0f);
                isBodyTurning = true;
                IsTurning = true;
            }
            if (characterAnimation.AnimationsPlayed)
            {
                IsTurning = false;
            }


            /*// Check if the head rotation exceeds the threshold
            if (Mathf.Abs(headRotationAngle) > rotationThreshold)
            {
                // Rotate the entire body instantly
                playerBody.rotation = Quaternion.Euler(playerBody.eulerAngles.x, targetRotationAngle, playerBody.eulerAngles.z);
            }
            else
            {
                // Lerp the rotation for smooth movement
                targetRotation = Quaternion.Euler(playerBody.eulerAngles.x, targetRotationAngle, playerBody.eulerAngles.z);
                playerBody.rotation = Quaternion.Lerp(playerBody.rotation, targetRotation, bodyRotationSpeed * Time.deltaTime);
            }*/

        }
        else
        {
            
            PlayerRotationWhenMoving();
        }

        // Check if the body is currently turning
        if (isBodyTurning)
        {
            // Rotate the player's body towards the target rotation
            playerBody.rotation = Quaternion.Lerp(playerBody.rotation, targetRotation, ((bodyRotationSpeed * cameraAngularSpeed.AngularSpeedY) / 2) * Time.deltaTime);
            //Debug.Log(bodyRotationSpeed * cameraAngularSpeed.AngularSpeedY * Time.deltaTime);
            // Check if the body rotation is close enough to the target rotation
            if (Quaternion.Angle(playerBody.rotation, targetRotation) < 0.1f)
            {
                // Snap to the target rotation
                playerBody.rotation = targetRotation;
                isBodyTurning = false;
                IsTurning = false;
            }

        }
        
    }
    void PlayerRotationWhenMoving()
    {
        // Update the camera forward rotation
        cameraForwardRotation = cameraLook.CameraRotation();

        // Calculate the rotation angle around the Y-axis for the player body
        float bodyRotationAngle = cameraForwardRotation.eulerAngles.y - playerBody.eulerAngles.y;

        // Wrap the body rotation angle to the range of -180 to 180 degrees
        if (bodyRotationAngle > 180f)
            bodyRotationAngle -= 360f;
        else if (bodyRotationAngle < -180f)
            bodyRotationAngle += 360f;

        // Rotate the player's body towards the camera forward rotation
        playerBody.rotation *= Quaternion.Euler(0f, bodyRotationAngle, 0f);
    }

}
