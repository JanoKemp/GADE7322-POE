using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankProjectile : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    public int enemyProjectileDmg = 5;
    private float speed = 10f;
    Transform target;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Defender").transform;
    }

    public void Seek(GameObject _target)
    {
        target = _target.transform;
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
