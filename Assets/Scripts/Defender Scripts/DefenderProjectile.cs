using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DefenderProjectile : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    public int defenderProjectileDmg = 10;
    private float speed = 10f;
    Transform target;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {

        if (target == null)
        {
            Destroy(gameObject);
        }



        Vector3 dir = target.position - transform.position;//+ new Vector3(0f, 5f, 0f) - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }
}
