using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemySpawns;
    public GameObject enemyPrefab;
    private float spawnTimer;

    enum Waves // For the first part we use wave 1 only
    {
        wave1, wave2, wave3, wave4
    }
    // Start is called before the first frame update
    void Start()
    {
        

    }
    
    void SpawnEnemies()
    {
        int randSpawn = Random.Range(0, 3);
        Debug.Log(randSpawn);
        
        switch (randSpawn)
        {
            case 0:
                Instantiate(enemyPrefab, enemySpawns[0].transform.position, transform.rotation); break;
                case 1:
                Instantiate(enemyPrefab, enemySpawns[1].transform.position, transform.rotation); break;
                case 2:
                Instantiate(enemyPrefab, enemySpawns[2].transform.position, transform.rotation); break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        spawnTimer = spawnTimer +Time.deltaTime;
        if(spawnTimer > 5)
        {
            enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
            spawnTimer = 0;
            SpawnEnemies();
        }

    }
}
