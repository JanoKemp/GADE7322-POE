using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DefenderTower : MonoBehaviour
{
    public Transform target;
    public GameObject defenderProj;
    public float rpm = 1.5f;
    public int health;
    public int maxHealth = 100;

    private float fireTimer;
    // Start is called before the first frame update
    void Start()
    {
        fireTimer = 0;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime;
        if(target != null && fireTimer <= 0)
        {
            Shoot();
            fireTimer = rpm;
        }
        if(target == null)
        {
            FindNewTarget();
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
}
