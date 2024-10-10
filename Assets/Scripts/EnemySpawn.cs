using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemySpawns;
    public GameObject enemyPrefab;
    public GameObject shotgunPrefab;
    private float spawnTimer;
    private float spawnSpeed;
    enum Waves // For the first part we use wave 1 only
    {
        wave1, wave2, wave3, wave4
    }
    // Start is called before the first frame update
    void Start()
    {
        spawnSpeed = 5;

    }
    
    void SpawnEnemies()
    {
        int randSpawn = Random.Range(0, 3);
        
        
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
        switch ((int)(Time.timeSinceLevelLoad / 50))
        {
            case 4:  // If more than 200 seconds have passed
                spawnSpeed = 1f;
                break;
            case 3:  // If more than 150 seconds have passed
                spawnSpeed = 2f;
                break;
            case 2:  // If more than 100 seconds have passed
                spawnSpeed = 3f;
                break;
            case 1:  // If more than 50 seconds have passed
                spawnSpeed = 4f;
                break;
            default: // Less than 50 seconds
                spawnSpeed = 5f;
                break;
        }
        Debug.Log(spawnSpeed); 
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnSpeed)
        {
            enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
            spawnTimer = 0;
            SpawnEnemies();
        }

    }
}
