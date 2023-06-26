using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAngularSpeed : MonoBehaviour
{
    public Transform cameraTransform;
    private float previousRotationY;
    private float angularSpeedY;

    public float AngularSpeedY
    {
        get
        {
            return angularSpeedY;
        }
    }

    private void Start()
    {
        previousRotationY = cameraTransform.eulerAngles.y;
    }

    private void Update()
    {
        // Calculate the angular speed on the Y-axis in degrees per second
        float currentRotationY = cameraTransform.eulerAngles.y;
        float angleY = Mathf.DeltaAngle(previousRotationY, currentRotationY);
        angularSpeedY = Mathf.Abs(angleY) / Time.deltaTime;

        // Update the previous rotation for the next frame
        previousRotationY = currentRotationY;

        // Output the angular speed on the Y-axis
        //Debug.Log("Angular Speed Y: " + angularSpeedY);
    }
}
