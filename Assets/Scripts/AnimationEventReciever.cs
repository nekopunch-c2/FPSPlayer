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
        if (Time.time - LastFootstepTime >= CooldownDuration)
        {
            OnFootstep?.Invoke();
            LastFootstepTime = Time.time;
        }
    }
    private void LandStep(AnimationEvent animationEvent)
    {
        Debug.Log("animatorClipInfo.weight = " + animationEvent.animatorClipInfo.weight + animationEvent.animatorClipInfo.clip);
        if (Time.time - LastFootstepTime >= CooldownDuration)
        {
            OnLandStep?.Invoke();
            LastFootstepTime = Time.time;
        }
    }
}
