using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunProjectile : MonoBehaviour
{
    Rigidbody body;
    
    private float speed = 10f;
    public int projectileDmg = 5;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LifeTimer());
        body = GetComponent<Rigidbody>();
        body.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      
    }

    IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
