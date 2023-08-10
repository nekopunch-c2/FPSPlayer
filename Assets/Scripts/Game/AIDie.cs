using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDie : MonoBehaviour
{
    //public BulletTarget targetHealth;
    private BlazeAI blaze;
    private Collider m_Collider;
    //public Animator anim;
    //public float timeBeforeAliveAgain = 15f;
    public AudioSource source;


    // Update is called once per frame
    public void Die()
    {
        m_Collider = GetComponent<Collider>();
        //Debug.Log("die ai");
        blaze = GetComponent<BlazeAI>();
        source = GetComponent<AudioSource>();
        blaze.Death();
        m_Collider.enabled = false;
        //source.enabled = false;
        //StartCoroutine(BackToLife());

    }

    /*IEnumerator BackToLife()
    {
        yield return new WaitForSeconds(timeBeforeAliveAgain);
        Debug.Log("live ai");
        targetHealth.m_CurrentHealth = (targetHealth.health);
        anim.Play("BackToLife");
        m_Collider.enabled = true;
        blaze.enabled = true;
        aiDieScript.enabled = false;
        blaze.Hits(Player);
    }*/
}
