﻿using UnityEngine;

namespace BlazeAIDemo
{
    public class FlagAttack : MonoBehaviour
    {
        public bool attacking { get; set; }
        
        public void Attacking()
        {
            attacking = true;
        }

        public void MeleeHitFinished()
        {
            attacking = false;
        }
    }
}

