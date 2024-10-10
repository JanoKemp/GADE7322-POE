using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankEnemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private float health;
    private float maxHealth = 150;
    public GameObject tower;
    public HealthBar healthBar;
    public GameObject[] defenders;
    public GameObject mainTower;
    public GameObject target;

    private bool canShoot = false;
    public float distanceToShootDefenders = 30f;

    public GameObject projectile;
    public Transform shootingPoint;  // Empty child object on turret for shooting
    public Transform turret;         // Reference to the turret (top part)
    public Transform tankBase;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 2f;  // Slow movement speed
        tower = GameObject.FindGameObjectWithTag("PlayerTower");
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealth(health, maxHealth);
        defenders = GameObject.FindGameObjectsWithTag("Defender");
        mainTower = GameObject.FindGameObjectWithTag("PlayerTower");
        StartCoroutine(Shoot());
    }

    private void FixedUpdate()
    {
        target = null;
        TargetMainTower();
        defenders = GameObject.FindGameObjectsWithTag("Defender");
        if (defenders != null)
        {
            for (int i = 0; i < defenders.Length; i++)
            {
                float distance = Vector3.Distance(this.gameObject.transform.position, defenders[i].transform.position);
                if (distance < distanceToShootDefenders)
                {
                    target = defenders[i].gameObject;
                    canShoot = true;
                    RotateTurretTowardsTarget(target);  // Rotate turret towards the defender
                    break;  // Stop checking after finding a valid target
                }
                else
                {
                    canShoot = false;
                }
            }
        }
    }

    void Update()
    {
        // Move the base towards the tower
        agent.SetDestination(tower.transform.position);
    }

    private void TargetMainTower()
    {
        float distanceToTower = Vector3.Distance(this.gameObject.transform.position, mainTower.transform.position);
        if (distanceToTower < 2f)
        {
            target = mainTower;
            RotateTurretTowardsTarget(mainTower);  // Rotate turret towards the tower
        }
    }

    private void RotateTurretTowardsTarget(GameObject target)
    {
        // Calculate the direction to the target
        Vector3 directionToTarget = (target.transform.position - turret.position).normalized;
        directionToTarget.y = 0;  // Ensure turret only rotates on the Y-axis (no vertical tilt)

        // Smoothly rotate the turret towards the target
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        turret.rotation = Quaternion.Slerp(turret.rotation, lookRotation, Time.deltaTime * 5f);  // Adjust 5f for speed of rotation
    }

    private void RotateTankBase(GameObject target)
    {
        Vector3 velocity = agent.velocity;

        if (velocity.sqrMagnitude > 0.1f)  
        {
            Vector3 direction = new Vector3(velocity.x, 0, velocity.z).normalized;

            // Rotate the tank base towards the movement direction
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            tankBase.rotation = Quaternion.Slerp(tankBase.rotation, lookRotation, Time.deltaTime * 10f);  // Adjust rotation speed
        }
    }

    private void Attack()
    {
        if (target == null)
        {
            return;
        }
        if (target != null && target.tag != "MainTower")
        {
            StartCoroutine(Shoot());
        }
        if (target != null && target.tag == "MainTower")
        {
            // To be done later
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            if (canShoot && target != null)
            {
                Debug.Log("Shoot");
                // Instantiate projectile from the shooting point (on the turret)
                GameObject proj = Instantiate(projectile, shootingPoint.position, projectile.transform.rotation);
                enemyProjectile bullet = proj.GetComponent<enemyProjectile>();
                if (bullet != null)
                {
                    bullet.Seek(target);
                }
                canShoot = false;  // Prevent continuous shooting
                yield return new WaitForSeconds(5f);  // Wait for fire rate interval
                canShoot = true;
            }
            yield return null;  // Continue next frame
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
            GameObject goldFetch = GameObject.Find("WorldController");
            goldFetch.GetComponent<PlayerRes>().gold += 5;
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("defenderProjectile"))
        {
            int projectileDamage = other.GetComponent<DefenderProjectile>().defenderProjectileDmg;
            TakeDamage(projectileDamage);
            other.GetComponent<DefenderProjectile>().target = null;
        }
    }
}
