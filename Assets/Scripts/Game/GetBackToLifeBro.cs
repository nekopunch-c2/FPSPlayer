using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GetBackToLifeBro : MonoBehaviour
{
    private BlazeAI blaze;
    public IEnumerator GetBackToLife()
    {
        blaze = GetComponent<BlazeAI>();
        yield return new WaitForSeconds(10);
        blaze.enabled = true;
    }
}
