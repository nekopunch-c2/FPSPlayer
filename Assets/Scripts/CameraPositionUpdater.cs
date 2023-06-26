using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionUpdater : MonoBehaviour
{
    public Transform playerBody;
    void Update()
    {
        transform.position = playerBody.transform.position;
    }
}
