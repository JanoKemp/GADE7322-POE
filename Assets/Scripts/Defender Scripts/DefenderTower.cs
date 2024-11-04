using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DefenderTower : MonoBehaviour
{
    public Transform target;
    public GameObject defenderProj;
    public float rpm = 1.5f;
    public float health;
    public float maxHealth = 100;

    public HealthBar healthBar;
    
    private bool canShoot = true;

    public int upgradeHealthCounter = 0;
    public int upgradeFireRateCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        //fireTimer = 0;
        health = maxHealth;
        StartCoroutine(FireTimer());
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealth(health, maxHealth);

    }

    // Update is called once per frame
    void Update()
    {
        
        if(target == null)
        {
            FindNewTarget();
        }
        
    }
    IEnumerator FireTimer()
    {
        while (true)
        {
            if (canShoot && target != null)
            {
                Shoot();
                canShoot = false;  // Prevent continuous shooting
                yield return new WaitForSeconds(rpm);  // Wait for fire rate interval
                canShoot = true;
            }
            yield return null;  // Continue next frame
        }
    }
    private void Shoot()
    {
        GameObject projectile = Instantiate(defenderProj, transform.position, transform.rotation);
        DefenderProjectile bullet = projectile.GetComponent<DefenderProjectile>();
        if (bullet != null)
        {
            bullet.Seek(target);
        }



    }
    private void FindNewTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
    }
    private void TakeDamage(float damage)
    {

        if (health > 0)
        {
            health -= damage;
            healthBar.UpdateHealth(health, maxHealth);
        }
        if (health <= 0)
        {
           Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemyProjectile"))
        {
            TakeDamage(other.GetComponent<enemyProjectile>().enemyProjectileDmg);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("ShotgunProjectile"))
        {
            TakeDamage(other.GetComponent<ShotgunProjectile>().projectileDmg);
            
        }
        if (other.CompareTag("tankProjectile"))
        {
            TakeDamage(other.GetComponent<tankProjectile>().tankProjectileDmg);
            Destroy(other.gameObject);
        }
    }

   
}
