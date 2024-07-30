using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeGenertion : MonoBehaviour
{
    public GameObject landBlock; //GameObject for individual landBlocks
    public int gridRadius = 30; //grid radius
    public float noiseScale = 10f; //scale to adjust frequency of noise. The higher the scale, the smooth the terrain
    public float heightScale = 5f; //scale to adjust height of of blocks (hills of terrain)

    private GameObject[,] land; //2D array for landBlocks

    void Start()
    {
        GenerateTerrain();
        GeneratePaths();
    }

    void GenerateTerrain()
    {
        land = new GameObject[gridRadius, gridRadius];//initialising 2d arry to hold references for the landBlock gameObjects
        float rndX = Random.Range(0f, 100f);
        float rndZ = Random.Range(0f, 100f);//assigning random values for x and Z coordinates so that the perlin noise will generate differently everytime
                                            //which will result in a unique terrain each time the game is run      

        for (int x = 0; x < gridRadius; x++)
        {
            for (int z = 0; z < gridRadius; z++)
            {
                float y = Mathf.PerlinNoise((x + rndX) / noiseScale, (z + rndZ) / noiseScale) * heightScale; //using built perlin noise function and multiplaying by heightScale to generate a smooth noise map
                Vector3 position = new Vector3(x, y, z);// assiging coordinates of each block to postion
                land[x, z] = Instantiate(landBlock, position, Quaternion.identity);//instantiating each block in the 2d array using the position assigned above
            }
        }
    }

    void GeneratePaths()
    {
        List<Vector2Int> edges = new List<Vector2Int>//Vector2 is used as the terrain is represented as a 2d array
        {
            new Vector2Int(0, Random.Range(0, gridRadius)),
            new Vector2Int(gridRadius - 1, Random.Range(0, gridRadius)),
            new Vector2Int(Random.Range(0, gridRadius), 0),
            new Vector2Int(Random.Range(0, gridRadius), gridRadius - 1)
        };// a list to store potential starting points on edges of terrain

        Vector2Int center = new Vector2Int(gridRadius / 2, gridRadius / 2); //store center point of terrain as a end point for enemies to go to

        for (int i = 0; i < 3; i++)
        {
            Vector2Int start = edges[Random.Range(0, edges.Count)];//assiging sarting coordinates to start
            edges.Remove(start);//removing start block
            CreatePath(start, center);
        }//randomly choosing 3 starting points
    }

    void CreatePath(Vector2Int start, Vector2Int end)
    {
        Vector2Int current = start; //assigning start position generated in GeneratePath function to current variable

        while (current != end) //destroys blocks in generated path until current = end block
        {
            Destroy(land[current.x, current.y]);
            land[current.x, current.y] = null;

            if (current.x != end.x)
            {
                current.x += (int)Mathf.Sign(end.x - current.x);//Mathf.Sign return 1, -1, 0. If end.x > current.x it  will move right, if < then left
            }
            else if (current.y != end.y)
            {
                current.y += (int)Mathf.Sign(end.y - current.y);//same as above but will determine if y(vertical) should move up or down
            }
        }

        // Destroy the final block at the center
        Destroy(land[end.x, end.y]);
        land[end.x, end.y] = null;
    }
}