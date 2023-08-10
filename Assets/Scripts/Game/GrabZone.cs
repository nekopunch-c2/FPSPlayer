using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class GrabZone : MonoBehaviour
    {
        public Transform whereThePlayerShouldBe;
        public GameObject player;
        //private Movement movement;
        public HurtZone hurtZone;
        public bool startCoroutiner = false;

        void OnTriggerEnter(Collider other)
        {

            Vector3 vec = whereThePlayerShouldBe.position;
            if (other.CompareTag("Player"))
            {
                player.transform.position = whereThePlayerShouldBe.position;
                player.transform.rotation = whereThePlayerShouldBe.rotation;
                //movement = player.GetComponent<Movement>();

                //movement.enabled = false;
                startCoroutiner = true;
                
                //StartCoroutine(hurtZone.MovementBack());
            }

        }
    }

