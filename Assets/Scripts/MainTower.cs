using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MainTower : MonoBehaviour
{
    public float health;
    public float maxHealth = 500f;
    private HealthBar healthBar;

    public Transform target;
    public GameObject defenderProj;
    public float rpm = 1.5f;
    private bool canShoot = true;

    public GameObject gameOverUI;

    public bool gameOver = false;
    public Flash flash;
    // Start is called before the first frame update
    void Start()
    {
        gameOverUI = GameObject.FindGameObjectWithTag("GameOver");
        gameOverUI.SetActive(false);
        health = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealth(health, maxHealth);
        StartCoroutine(passiveHealth());
        StartCoroutine(FireTimer());
        flash = FindAnyObjectByType<Flash>();
    }

    // Update is called once per frame
    void Update()
    {
            if (target == null)
            {
                FindNewTarget();
            }
    }

    IEnumerator passiveHealth()
    {
        if (health < 100)
        {
            health += 5;
            yield return new WaitForSeconds(3f);
        }
        else yield return null; 
        
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

            // Trigger screen flash
            if (flash != null)
            {
                flash.DamageFlash();
            }
        }

        if (health <= 0)
        {
            gameOverUI.SetActive(true);
            gameOver = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            TakeDamage(25f);
            Destroy(other.gameObject);
        }
    }
}
