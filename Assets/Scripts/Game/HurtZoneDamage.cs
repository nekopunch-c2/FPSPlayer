using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtZoneDamage : MonoBehaviour
{
    public int damageAmount;
    public PlayerManager player;

    void OnTriggerEnter(Collider other)
    {
        player = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        if (other.CompareTag("Player"))
        {
            StartCoroutine(player.TakeDamage(damageAmount));
        }
    }


}
