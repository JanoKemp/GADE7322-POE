using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShotgun : MonoBehaviour
{
    private NavMeshAgent agent;
    private float health;
    private float maxHealth = 50;
    public GameObject tower;
    public HealthBar healthBar;
    public GameObject[] defenders;
    public GameObject mainTower;
    public GameObject target;
    public Transform[] projectileSpawns;

    //public float[] distanceOfTowers;

    private bool canShoot = false;
    public float distanceToShootDefenders = 10f; //Distance is less (Closer for shotgun

    public GameObject shotgunProjectile;

    // Start is called before the first frame update
    void Start()
    {

        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 2.2f; //Faster Move speed (Base 1.46f)
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
        agent.SetDestination(tower.transform.position);
        

    }

    private void TargetMainTower()
    {

        float distanceToTower = Vector3.Distance(this.gameObject.transform.position, mainTower.transform.position);
        if (distanceToTower < 2f)
        {
            target = mainTower;
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
                
                for (int index = 0; index < projectileSpawns.Length; index++)
                {
                  Instantiate(shotgunProjectile, projectileSpawns[index].transform.position, projectileSpawns[index].rotation);
                    
                    
                }
                canShoot = false;  // Prevent continuous shooting
                yield return new WaitForSeconds(2f);  // Wait for fire rate interval
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
            goldFetch.GetComponent<PlayerRes>().gold += 5;
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
        if (other.CompareTag("RippleProjectile"))
        {
            int projectileDamage = other.GetComponent<RippleProjectile>().projectileRippleDmg;
            TakeDamage(projectileDamage);
            
        }
    }
}
