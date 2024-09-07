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
    public GameObject[] defenders;
    public GameObject mainTower;
    public GameObject target;
    public float[] distanceOfTowers;

    private bool canShoot = false;
    public float distanceToShootDefenders = 15f;

    public GameObject projectile;
    
    // Start is called before the first frame update
    void Start()    
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        tower = GameObject.FindGameObjectWithTag("PlayerTower");
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealth(health, maxHealth);
        defenders = GameObject.FindGameObjectsWithTag("Defender");
        mainTower = GameObject.FindGameObjectWithTag("PlayerTower");
    }
    private void FixedUpdate()
    {
        TargetMainTower();
        defenders = GameObject.FindGameObjectsWithTag("Defender");
        if (defenders == null)
        {
          
        }
        else if(defenders != null)
        { 
            for(int i = 0; i < defenders.Length; i++)
            {
                float distance = Vector3.Distance(this.gameObject.transform.position, defenders[i].transform.position);
                if(distance < distanceToShootDefenders)
                {
                    target = defenders[i];
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
        if(distanceToTower < 2f)
        {
            target = mainTower;
        }
        
    }
    private void Attack()
    {
        if(target == null)
        {
            
            return;
        }
        if(target != null && target.tag != "MainTower")
        {
            StartCoroutine(Shoot());  
        }
        if(target != null && target.tag == "MainTower")
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
                
                GameObject proj = Instantiate(projectile, projectile.transform.position, projectile.transform.rotation);
                enemyProjectile bullet = proj.GetComponent<enemyProjectile>();
                if (bullet != null)
                {
                    bullet.Seek(target);
                }
                canShoot = false;  // Prevent continuous shooting
                yield return new WaitForSeconds(2f);  // Wait for fire rate interval
                canShoot = true;
            }
            yield return null;  // Continue next frame
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("defenderProjectile"))
        {
            int projectileDamage = other.GetComponent<DefenderProjectile>().defenderProjectileDmg;
            GameObject goldFetch = GameObject.Find("WorldController");
            goldFetch.GetComponent<PlayerRes>().gold += 5;
            //GetComponent<PlayerRes>().gold += 5;
            TakeDamage(projectileDamage);
        }
    }
}
