using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class EnemyBase : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public float moveSpeed = 1f;
    public int damageToDefenders = 10;
    public int damageToPlayerTower = 5;
    
    public EnemyBase(int health, int maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;
    }
    
    public virtual void TakeDamage(int damageToEnemy)
    {
        if(health > 0 )
        {
            health -= damageToEnemy;
        }
        else if(health <= 0 )
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
