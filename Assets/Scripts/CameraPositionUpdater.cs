using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionUpdater : MonoBehaviour
{
    public Transform cameraSocket;
    void LateUpdate()
    {
        transform.position = cameraSocket.transform.position;
    }
}
