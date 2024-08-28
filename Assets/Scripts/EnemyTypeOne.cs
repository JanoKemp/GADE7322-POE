using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTypeOne : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject tower;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tower = GameObject.FindGameObjectWithTag("PlayerTower");
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(tower.transform.position);
    }
}
