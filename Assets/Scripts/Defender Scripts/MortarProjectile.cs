using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MortarProjectile : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    private int defenderProjectileDmg = 25;
    private float speed = 10f;
    private float launchForce = 10f;
    private float explosionRadius = 5f;
    private float explosionForce = 500f;
    private GameObject[] targetsInVacinity;

    public Transform target;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        LaunchProjectile();
    }

    public void FetchTarget(Transform _target)
    {
        target = _target;
    }

    void Update()
    {

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }


    }



    private void LaunchProjectile() // Arc calculation
    {
        // Calculate direction to target
        Vector3 direction = target.position - transform.position;

        // Uses gravity to calculate launch velocity
        float gravity = Physics.gravity.magnitude;
        //float heightDifference = target.position.y - transform.position.y;

        // Solve for velocity
        float distance = new Vector3(direction.x, 0, direction.z).magnitude;
        float launchAngle = Mathf.Deg2Rad * 45f;  // Launch angle 45 Deg
        float requiredVelocity = Mathf.Sqrt(gravity * distance / Mathf.Sin(2 * launchAngle));

        // Set launch angle and speed
        Vector3 velocity = new Vector3(direction.x, requiredVelocity * Mathf.Sin(launchAngle), direction.z).normalized * requiredVelocity;

        // apply velocity to rigidBody
        rb.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LandBlock") || other.CompareTag("PathBlock") || other.CompareTag("Enemy"))
        {
            
            PushNearbyEnemies();
            Destroy(gameObject);
        }
    }

    void PushNearbyEnemies()
    {
        // Detect all enemies within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach (Collider nearbyObject in colliders)
        {
            // Check if the nearby object has the "Enemy" tag
            if (nearbyObject.CompareTag("Enemy"))
            {
                if (nearbyObject.TryGetComponent<EnemyShotgun>(out EnemyShotgun shotgunEnemy))
                {
                    shotgunEnemy.TakeDamage(defenderProjectileDmg);
                }

                // Check for TankEnemy script
                else if (nearbyObject.TryGetComponent<TankEnemy>(out TankEnemy tankEnemy))
                {
                    tankEnemy.TakeDamage(defenderProjectileDmg);
                }

                else if(nearbyObject.TryGetComponent<EnemyTypeOne>(out EnemyTypeOne baseEnemy))
                {
                    baseEnemy.TakeDamage(defenderProjectileDmg);
                }

            }
        }

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
