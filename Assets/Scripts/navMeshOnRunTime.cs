using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class navMeshOnRunTime : MonoBehaviour
{
    public NavMeshSurface[] pathNav;
    public GameObject[] pathBlocks;
    // Start is called before the first frame update
    void Start()
    {
        pathBlocks = GameObject.FindGameObjectsWithTag("PathBlock");
       for(int i = 0; i < pathBlocks.Length; i++)
        {
            pathNav[i] = pathBlocks[i].GetComponent<NavMeshSurface>();
            pathNav[i].BuildNavMesh();
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
