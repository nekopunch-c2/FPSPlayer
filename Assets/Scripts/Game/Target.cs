using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Target : MonoBehaviour
{
    //public float health = 5.0f;

    //public float m_CurrentHealth;

    [Header("AI Damage")]
    public BlazeAI aihit;
    private GameObject player;
    public AIDie aideath;
    public float DamageModif = 0f;
    public float counter = 0f;
    public float counterHowMuch = 0f;
    public bool isExtremity = false;
    public GameObject extremityThatFellOff;
    //public GameObject decal;

    public SkinnedMeshRenderer[] objToDestroy3;
    public GameObject[] colliders;
    private int indexBodyParts;
    private int partsToDestroy;
    private bool isDead = false;

    public BulletTarget bulletTarget;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        isDead = false;
    }


    public void Got(float damage)
    {
        bulletTarget.m_CurrentHealth -= (damage + DamageModif) ;
        counter++;
        //decal.SetActive(true);

        if (isExtremity && counter == counterHowMuch)
        {
            Instantiate(extremityThatFellOff, transform.position, transform.rotation);

            indexBodyParts = Random.Range(0, objToDestroy3.Length);

            
            /*partsToDestroy = objToDestroy3[indexBodyParts];
            Destroy(partsToDestroy);*/
            Destroy(objToDestroy3[1]);
            Destroy(objToDestroy3[2]);

            Destroy(colliders[0]);
            Destroy(colliders[1]);
        }
        if (bulletTarget.m_CurrentHealth > 0)

        {
            aihit.Hit(player);
            return;
        }
        if (bulletTarget.m_CurrentHealth < 0 && isDead == false)

        {
            isDead = true;
            aideath.Die();
        }
        
        Vector3 position = transform.position;
        
    }
}
