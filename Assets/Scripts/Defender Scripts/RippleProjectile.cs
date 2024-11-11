using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RippleProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Targets;
    public GameObject currentTarget;
    public int projectileRippleDmg = 10;
    public Rigidbody rb;
    private int hitCounter = 0;
    private float speed = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Targets = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        
        if(currentTarget == null )
        {
            Destroy(gameObject);
        }

        Vector3 dir = currentTarget.transform.position - transform.position;//+ new Vector3(0f, 5f, 0f) - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(currentTarget.transform);


    }

    public void TrackTarget(GameObject target)
    {
        currentTarget = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Apply damage logic here if needed, such as calling a method on the enemy
            hitCounter++;

            if (hitCounter < 3 && Targets.Length > hitCounter)
            {
                // Switch to the next target if fewer than 3 hits and more enemies remain
                currentTarget = Targets[hitCounter];
            }
            else
            {
                // Destroy the projectile if it has hit 3 targets or there are no more enemies
                Destroy(gameObject);
            }
        }
    }
}
