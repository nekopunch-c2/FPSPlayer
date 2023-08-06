using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFootsteps : MonoBehaviour
{
    public FootstepSounds FootstepSounds;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit floor");
    }
}
