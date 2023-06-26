using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpineOrientation : MonoBehaviour
{
    //public Transform spineToOrientate;
    public List<Transform> headToOrientate;
    public Transform objectToOrientateTo;

    // Update is called once per frame
    void LateUpdate()
    {
        // spineToOrientate.rotation = transform.rotation;
        foreach (Transform transform in headToOrientate)
        {
            transform.rotation = objectToOrientateTo.rotation;
            //transform.position = objectToOrientateTo.position;
        }
    }
}
