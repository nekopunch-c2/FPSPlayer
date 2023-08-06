using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationEventReciever : MonoBehaviour
{
    public static event Action OnFootstep;
    public static event Action OnLandStep;

    private void Footstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            OnFootstep?.Invoke();
        }
    }
    private void LandStep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            Debug.Log("cui invokled");
            OnLandStep?.Invoke();
        }
    }
}
