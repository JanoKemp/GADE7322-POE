using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DefenderTower : MonoBehaviour
{
    public GameObject projectile;
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

    }
    public void Shoot()
    {
        if(fireTimer > 0)
        {
            return;
        }
        else if (fireTimer <= 0)
        {
            fireTimer = rpm;
            Instantiate(projectile);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Shoot();
        }
    }
}
