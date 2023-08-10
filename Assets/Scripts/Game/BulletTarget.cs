using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTarget : MonoBehaviour
{
    public float health = 5.0f;

    public float m_CurrentHealth;

    [Header("AI Damage")]
    private BlazeAI aihit;
    private GameObject Player;
    private AIDie aideath;

    //public Collider m_HeadCollider;
    //public Collider m_ChestCollider;

    void Start()
    {
        aideath = GetComponent<AIDie>();
        aihit = GetComponent<BlazeAI>();
        Player = GameObject.FindWithTag("Player");
        m_CurrentHealth = health;

    }

}
