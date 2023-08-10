﻿using UnityEngine;
using System.Collections;
using BlazeAISpace;

namespace BlazeAIDemo
{
    public class Shoot : MonoBehaviour
    {
        [HideInInspector] public LineRenderer lr;
        [HideInInspector] public BlazeAI blaze;
        [HideInInspector] public BlazeAISpace.CoverShooterBehaviour coverShooter;

        public Transform gun;
        public Material shootMaterial;
        public AudioSource gunShot;


        void Start() 
        {
            blaze = GetComponent<BlazeAI>(); 
            coverShooter = GetComponent<CoverShooterBehaviour>();
            lr = GetComponent<LineRenderer>();

            lr.enabled = false; 
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
        }

        public void ShootTarget()
        {
            lr.material = shootMaterial;
            gunShot.Play();
            
            lr.enabled = true;
            lr.SetPosition(0, gun.position + new Vector3(0f, 0.2f, 0f));
            lr.SetPosition(1, coverShooter.hitPoint + new Vector3(0f, 0f, 0f));

            StartCoroutine(TurnRendererOff());
        }

        IEnumerator TurnRendererOff()
        {
            yield return new WaitForSeconds(0.2f);
            
            lr.enabled = false;
        }
    }
}

