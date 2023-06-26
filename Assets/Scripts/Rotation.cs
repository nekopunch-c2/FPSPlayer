using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Transform cameraTargetTransform; // The transform of the target object to follow

    private void Update()
    {
        // Check if the target transform is assigned
        if (cameraTargetTransform != null)
        {
            // Set the follower object's rotation to match the target object's rotation
            transform.rotation = cameraTargetTransform.localRotation;
        }
    }
}
