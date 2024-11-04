using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RippleDefender : MonoBehaviour
{ 
public Transform target;
public GameObject rippleProjectile;
private float rpm = 2f;// Default 1.5(shoots slower, does more damage)
public float health;
public float maxHealth = 100;
public Transform firePoint;
public HealthBar healthBar;
public float rotationSpeed = 5f;

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
    
    if (target == null)
    {
        FindNewTarget();
    }
        else
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0;  // Only Around Y

            if (direction != Vector3.zero)
            {
                // Calulation rotation to face the target.
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Spherical rotation (Smooth)
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
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
    GameObject projectile = Instantiate(rippleProjectile, firePoint.transform.position, firePoint.transform.rotation);
    RippleProjectile bullet = projectile.GetComponent<RippleProjectile>();
    if (bullet != null)
    {
            bullet.TrackTarget(target.gameObject);
        
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
