using UnityEngine;

namespace BlazeAISpace
{
    public class AnimationManager
    {
        Animator anim;
        BlazeAI blaze;

        string currentState;


        // constructor
        public AnimationManager (Animator animator, BlazeAI blazeAI)
        {
            anim = animator;
            blaze = blazeAI;
        }


        // actual animation playing function
        public void Play(string state, float time = 0.25f, bool overplay = false)
        {
            if (state == currentState) return;

            
            // check if passed animation name doesn't exist in the Animator
            if (!CheckAnimExists(state)) {
                if (string.IsNullOrEmpty(state)) {
                    anim.enabled = false;
                    return;
                }
            
                anim.enabled = true;
                return;
            }

            
            anim.enabled = true;
            anim.CrossFadeInFixedTime(state, time, 0);


            if (overplay) currentState = "";
            else currentState = state;
        }


        // check whether the passed animation name exists or not
        public bool CheckAnimExists(string animName)
        {
            if (string.IsNullOrEmpty(animName)) 
            {
                #if UNITY_EDITOR
                if (blaze.warnEmptyAnimations) {
                    Debug.LogWarning("Empty animation.");
                }
                #endif

                return false;
            }

            
            bool animCheck = anim.HasState(0, Animator.StringToHash(animName));

            if (!animCheck) 
            {
                #if UNITY_EDITOR
                if (blaze.warnEmptyAnimations) {
                    Debug.LogWarning($"The animation name: {animName} - doesn't exist and has been ignored. Please re-check your animation names.");
                }
                #endif

                return false;
            }


            return animCheck;
        }


        public void ResetLastState()
        {
            currentState = "";
        }
    }
}
