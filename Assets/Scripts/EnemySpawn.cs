using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemySpawns;
    public GameObject enemyPrefab;
    public GameObject shotgunPrefab;
    public GameObject tank;
    private float spawnTimer;
    private float spawnSpeed;
    PlayerRes playerRes;
    private int gold;

    private float goldTrackingTimer;
    public bool allowShotgun = false;
    public bool allowTank = false;

    enum Waves // For the first part we use wave 1 only
    {
        wave1, wave2, wave3, wave4
    }
    // Start is called before the first frame update
    void Start()
    {
        spawnSpeed = 5;
        playerRes = GetComponent<PlayerRes>();
        
    }
    
    void SpawnEnemies()
    {
        int randSpawn = Random.Range(0, 3);
        
        
        switch (randSpawn)
        {
            case 0:
                DifferentEnemies(enemySpawns[0].transform.position); break;
                case 1:
                DifferentEnemies(enemySpawns[1].transform.position); break;
                case 2:
                DifferentEnemies(enemySpawns[2].transform.position); break;
        }
        
    }

    void DifferentEnemies(Vector3 spawnPosition)
    {
        if(allowShotgun)
        {
            int randomEnemy = Random.Range(0, 3);
            switch (randomEnemy)
            {
                case 0:
                    Instantiate(enemyPrefab,spawnPosition,transform.rotation); break;
                case 1:
                    Instantiate(shotgunPrefab, spawnPosition, transform.rotation); break;
            }
        }
        if (allowTank)
        {
            int randomEnemy = Random.Range(0, 3);
            switch (randomEnemy)
            {
                case 0:
                    Instantiate(enemyPrefab, spawnPosition, transform.rotation); break;
                case 1:
                    Instantiate(shotgunPrefab, spawnPosition, transform.rotation); break;
                case 2:
                    Instantiate(tank, spawnPosition, transform.rotation); break;
            }
        }
        else
        {
            Instantiate(enemyPrefab, spawnPosition, transform.rotation);
        }
        
        }

    void GoldHording()
    {
        if (gold > 300) // If they are hording, spawn more enemies
        {
            goldTrackingTimer += Time.deltaTime;
        }
        if(goldTrackingTimer > 10) //Every 10 seconds
        {
            switch (goldTrackingTimer / 10)//Every 10 Iteration
            {
                case 5: SpawnEnemies(); goldTrackingTimer = 0; break;//Reset timer

                case 4: SpawnEnemies();//Spawn 3 extra enemies
                        SpawnEnemies();
                        SpawnEnemies();
                     break;

                case 3: SpawnEnemies();         //Spawn 2 extra enemies
                        SpawnEnemies(); break;

                case 2: SpawnEnemies(); break;

                case 1:
                    SpawnEnemies(); //Spawn more enemies
                        break;
            }
            
        }
        if(gold < 300) //They arent hording gold reset timer
        {
            goldTrackingTimer = 0;
        }
        
    }
    

    // Update is called once per frame
    void Update()
    {
        gold = playerRes.gold;
        GoldHording();//Tracking players gold
        switch ((int)(Time.timeSinceLevelLoad / 50))
        {
            case 4:  // If more than 200 seconds have passed
                spawnSpeed = 1f;
                
                break;
            case 3:  // If more than 150 seconds have passed
                spawnSpeed = 2f;
                allowShotgun = false;
                allowTank = true;
                break;
            case 2:  // If more than 100 seconds have passed
                spawnSpeed = 3f;
                allowShotgun = true;
                break;
            case 1:  // If more than 50 seconds have passed
                spawnSpeed = 4f;
                break;
            case 0:
                spawnSpeed = 5f;
                break;
            default: // Less than 50 seconds
                spawnSpeed = 0.5f;
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
