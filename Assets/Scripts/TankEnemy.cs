using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class TankEnemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private float health;
    private float maxHealth = 200;
    public GameObject tower;
    public HealthBar healthBar;
    public GameObject[] defenders;
    public GameObject mainTower;
    public GameObject target;
    public Transform firePoint;
    public GameObject turret;
    public GameObject projectile;


    //public float[] distanceOfTowers;

    private bool canShoot = false;
    public float distanceToShootDefenders = 25f;
    public float stopDistance = 10f;

   

    private Quaternion desiredTurretRotation;


    // Start is called before the first frame update
    void Start()
    {

        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 1f; //Faster Move speed (Base 1.46f)
        agent.stoppingDistance = stopDistance;
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
        if (defenders == null)
        {

        }
        else if (defenders != null)
        {
            for (int i = 0; i < defenders.Length; i++)
            {
                float distance = Vector3.Distance(this.gameObject.transform.position, defenders[i].transform.position);
                if (distance < distanceToShootDefenders)
                {
                    target = defenders[i].gameObject;
                    canShoot = true;
                    Debug.Log("tank found target");
                    break;  // Stop checking after finding a valid target
                }
                else
                {
                    canShoot = false;
                }
            }

        }

        RotateTurretTowardsTarget();

    }
    void Update()
    {
        if (Vector3.Distance(transform.position, mainTower.transform.position)>stopDistance)
        {
            agent.SetDestination(tower.transform.position);

        }
        else agent.ResetPath();


    }

    private void TargetMainTower()
    {

        float distanceToTower = Vector3.Distance(this.gameObject.transform.position, mainTower.transform.position);
        if (distanceToTower < 2f)
        {
            target = mainTower;
        }

    }

    private void RotateTurretTowardsTarget()
    {
        if(target!=null)
        {
            Vector3 directiontoTarget = target.transform.position - turret.transform.position;
            directiontoTarget.y = 0;

            desiredTurretRotation=Quaternion.LookRotation(directiontoTarget);
            turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, desiredTurretRotation,Time.deltaTime * 5f);
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
            transform.LookAt(target.transform.position);
        }
        if (target != null && target.tag == "MainTower")
        {
            //To be done later
        }
    }
    IEnumerator Shoot()
    {
        while (true)
        {
            if (canShoot && target != null)
            {
                Debug.Log("Shoot");
                GameObject proj = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
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

    public void TakeDamage(float damage)
    {

        if (health > 0)
        {
            health -= damage;
            healthBar.UpdateHealth(health, maxHealth);
        }
        if (health <= 0)
        {
            GameObject goldFetch = GameObject.Find("WorldController");
            goldFetch.GetComponent<PlayerRes>().gold += 10;
            Die();
        }
    }

    private void Die() { Destroy(gameObject); }

    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("defenderProjectile"))
        {
            int projectileDamage = other.GetComponent<DefenderProjectile>().defenderProjectileDmg;


            //GetComponent<PlayerRes>().gold += 5;
            TakeDamage(projectileDamage);
            other.GetComponent<DefenderProjectile>().target = null;
        }
    }
}
