using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTypeOne : MonoBehaviour
{
    private NavMeshAgent agent;
    private float health;
    private float maxHealth = 100;
    public GameObject tower;
    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()    
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        tower = GameObject.FindGameObjectWithTag("PlayerTower");
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealth(health, maxHealth);

    }

    private void TakeDamage(float damage)
    {

        if(health > 0 )
        {
            health -= damage;
            healthBar.UpdateHealth(health, maxHealth);
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
            GetComponent<PlayerRes>().gold += 5;
            TakeDamage(projectileDamage);
        }
    }
}
