using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTypeOne : MonoBehaviour
{
    private NavMeshAgent agent;
    private int health;
    private int maxHealth = 100;
    public GameObject tower;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        tower = GameObject.FindGameObjectWithTag("PlayerTower");
    }

    private void TakeDamage(int damage)
    {
        if(health > 0 )
        {
            health -= damage;
        }
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die() { Destroy(gameObject); }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(tower.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("defenderProjectile"))
        {
            int projectileDamage = other.GetComponent<DefenderProjectile>().defenderProjectileDmg;
            TakeDamage(projectileDamage);
        }
    }
}
