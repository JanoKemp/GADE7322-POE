using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LandscapeGeneration : MonoBehaviour
{
    public GameObject landBlock; // GameObject for individual landBlocks
    public GameObject pathBlock; // GameObject for smaller blocks along the path
    public GameObject enemySpawn;
    private GameObject centreCube;
    public GameObject playerTower;
    public int spawnHeight = 1;
    public int gridRadius = 30; // Grid radius
    public float noiseScale = 10f; // Scale to adjust frequency of noise. The higher the scale, the smoother the terrain
    public float heightScale = 5f; // Scale to adjust the height of blocks (hills of terrain)
    public NavMeshSurface terrain;

    private GameObject[,] land; // 2D array for landBlocks

    void Start()
    {
        GenerateTerrain();
        GeneratePaths();
        BakeNavMesh();
        locateCenterCube();
        spawnTower();
    }

    void GenerateTerrain()
    {
        land = new GameObject[gridRadius, gridRadius]; // Initializing 2D array to hold references for the landBlock gameObjects
        float rndX = Random.Range(0f, 100f);
        float rndZ = Random.Range(0f, 100f); // Assigning random values for x and Z coordinates so that the Perlin noise will generate differently every time, resulting in a unique terrain each time the game is run

        for (int x = 0; x < gridRadius; x++)
        {
            for (int z = 0; z < gridRadius; z++)
            {
                float y = Mathf.PerlinNoise((x + rndX) / noiseScale, (z + rndZ) / noiseScale) * heightScale; // Using built-in Perlin noise function and multiplying by heightScale to generate a smooth noise map
                Vector3 position = new Vector3(x, y, z); // Assigning coordinates of each block to position
                land[x, z] = Instantiate(landBlock, position, Quaternion.identity); // Instantiating each block in the 2D array using the position assigned above
            }
        }
    }

    void GeneratePaths()
    {
        List<Vector2Int> corners = new List<Vector2Int> // Vector2Int is used as the terrain is represented as a 2D array
        {
            new Vector2Int(0, 0), // Bottom-left corner
            new Vector2Int(0, gridRadius - 1), // Top-left corner
            new Vector2Int(gridRadius - 1, 0), // Bottom-right corner
            new Vector2Int(gridRadius - 1, gridRadius - 1) // Top-right corner
        }; // A list to store the corners of the terrain

        Vector2Int center = new Vector2Int(gridRadius / 2, gridRadius / 2); // Store the center point of the terrain as an endpoint for enemies to go to

        for (int i = 0; i < 3; i++)
        {
            Vector2Int startBlock = corners[Random.Range(0, corners.Count)]; // Assigning starting coordinates to start
            corners.Remove(startBlock); // Removing the start block from the list to ensure uniqueness
            CreatePath(startBlock, center); // Creating the path from the selected corner to the center

            // Instantiate the game object at the starting block position
            Vector3 startPosition = land[startBlock.x, startBlock.y].transform.position;
            startPosition.y += spawnHeight;
            enemySpawn.tag = "EnemySpawn";
            Instantiate(enemySpawn, startPosition, Quaternion.identity);
        }
        foreach (var pathBlock in land) //Specify which blocks to bake the navMesh on
        {
            if (pathBlock.CompareTag("PathBlock"))
            {
                NavMeshModifier modifier = pathBlock.AddComponent<NavMeshModifier>();
                modifier.overrideArea = true; // Override the area for NavMesh
                modifier.area = 0; // Default walkable area
            }
        }


    }

    void CreatePath(Vector2Int startBlock, Vector2Int endBlock)
    {
        Vector2Int currentBlock = startBlock; // Assigning start position generated in GeneratePaths function to current variable

        while (currentBlock != endBlock) // Destroys blocks in generated path until current equals end block
        {
            // Replace the original block with a smaller block
            if (land[currentBlock.x, currentBlock.y] != null)
            {
                Vector3 position = land[currentBlock.x, currentBlock.y].transform.position;
                Destroy(land[currentBlock.x, currentBlock.y]);
                land[currentBlock.x, currentBlock.y] = Instantiate(pathBlock, position, Quaternion.identity);
            }

            if (currentBlock.x != endBlock.x && currentBlock.y != endBlock.y)
            {
                // Randomly decide whether to move horizontally or vertically
                if (Random.value > 0.5f)
                {
                    currentBlock.x += (int)Mathf.Sign(endBlock.x - currentBlock.x);
                }
                else
                {
                    currentBlock.y += (int)Mathf.Sign(endBlock.y - currentBlock.y);
                }
            }
            else if (currentBlock.x != endBlock.x)
            {
                currentBlock.x += (int)Mathf.Sign(endBlock.x - currentBlock.x); // Mathf.Sign returns 1, -1, or 0. If end.x > current.x, it will move right; if less, it will move left
            }
            else if (currentBlock.y != endBlock.y)
            {
                currentBlock.y += (int)Mathf.Sign(endBlock.y - currentBlock.y); // Same as above but will determine if y (vertical) should move up or down
            }
        }

        // Replace the final block at the center with a smaller block
        if (land[endBlock.x, endBlock.y] != null)
        {
            Vector3 position = land[endBlock.x, endBlock.y].transform.position;
            Destroy(land[endBlock.x, endBlock.y]);
            land[endBlock.x, endBlock.y] = Instantiate(pathBlock, position, Quaternion.identity);
        }

    }
    
   void locateCenterCube()//Locates the center cube within the array and fetches the gameobject that is declared as center.
    {
        Vector2Int center = new Vector2Int(gridRadius / 2, gridRadius / 2);
        centreCube = land[center.x, center.y];
        if(centreCube != null )
        {
            Renderer cubeRender = centreCube.GetComponent<Renderer>(); //Fetches the unity renderer on the object to allow for visual changes after it is instantiated
            if(cubeRender != null )
            {
                cubeRender.material.color = Color.yellow;
            }
        }
    }
    void BakeNavMesh()
    {
        if(terrain != null)
        {
            terrain.BuildNavMesh();
        }
        else
        {
            Debug.Log("There is no terrain");
        }
    }

    void spawnTower()//Spawns the player tower on the center cube
    {
        Vector3 towerSpawn = centreCube.transform.position + new Vector3(0,0.7f,0);
        playerTower.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        playerTower.tag = "PlayerTower";
        Instantiate(playerTower, towerSpawn, Quaternion.identity);
        
    }
}
