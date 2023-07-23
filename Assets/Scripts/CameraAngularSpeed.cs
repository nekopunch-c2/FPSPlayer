using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAngularSpeed : MonoBehaviour
{
    public Transform CameraTransform;
    private float _previousRotationY;
    private float _angularSpeedY;

    public float AngularSpeedY
    {  get { return _angularSpeedY; } }

    private void Start()
    {
        _previousRotationY = CameraTransform.eulerAngles.y;
    }

    private void Update()
    {
        // Calculate the angular speed on the Y-axis in degrees per second
        float currentRotationY = CameraTransform.eulerAngles.y;
        float angleY = Mathf.DeltaAngle(_previousRotationY, currentRotationY);
        _angularSpeedY = Mathf.Abs(angleY) / Time.deltaTime;

        // Update the previous rotation for the next frame
        _previousRotationY = currentRotationY;

        // Output the angular speed on the Y-axis
        //Debug.Log("Angular Speed Y: " + _angularSpeedY);
    }
}
