using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionUpdater : MonoBehaviour
{
    public Transform cameraSocket;
    void FixedUpdate()
    {
        transform.position = cameraSocket.transform.position;
    }
}
