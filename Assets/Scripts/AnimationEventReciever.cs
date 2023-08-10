using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class AnimationEventReciever : MonoBehaviour
{
    public static event Action OnFootstep;
    public static event Action OnLandStep;

    public float CooldownDuration = 0.2f;
    public float LastFootstepTime;

    private void Footstep(AnimationEvent animationEvent)
    {
        if (Time.time - LastFootstepTime >= CooldownDuration && animationEvent.animatorClipInfo.weight > 0.1f)
        {
            OnFootstep?.Invoke();
            LastFootstepTime = Time.time;
        }
    }
    private void LandStep(AnimationEvent animationEvent)
    {
        if (Time.time - LastFootstepTime >= CooldownDuration && animationEvent.animatorClipInfo.weight > 0.1f)
        {
            OnLandStep?.Invoke();
            LastFootstepTime = Time.time;
        }
    }
}
