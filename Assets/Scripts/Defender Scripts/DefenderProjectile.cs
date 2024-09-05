using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DefenderProjectile : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    public int defenderProjectileDmg = 10;
    private float speed = 2f;
    Transform target;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }
}
