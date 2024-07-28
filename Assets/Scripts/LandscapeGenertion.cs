using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeGenertion : MonoBehaviour
{
    public GameObject landBlock;
    public GameObject treeObject;
    private int landSizeX = 20; //Map size
    private int landSizeZ = 20; //Map size
    private int noiseHeight = 3;//determines height of blocks
    private int gridOffset = 1;//space bewtween each block
    private float seedX;
    private float seedY;
    private List<Vector3> landPos = new List<Vector3>();
    void Start()
    {
        seedX = Random.Range(0f, 1000f);
        seedY = Random.Range(0f, 1000f);
        for (int x = 0; x < landSizeX;  x++) //increases X size of map until it reaches landSizeX value
        {
            for (int z = 0; z < landSizeZ; z++) //increases Z size of map until it reaches landSizeZ value
            {
                Vector3 position = new Vector3(x * gridOffset, generatePerlinNoise(x, z, 8f) * noiseHeight, z * gridOffset); //calculating position of where landBlock must be placed
                
                GameObject land = Instantiate(landBlock, position, Quaternion.identity) as GameObject;//placing landBlock on position defined above by using instantiate function provided by unity
                landPos.Add(land.transform.position); //populating landBlock list with positions of instanntiated locks in scene
                land.transform.SetParent(this.transform);//setting landBlock tansform to transform of object that this script is attached to, aka WorldController
            }
        }
        ObjectSpawn();//calling method to spawn when game is run
    }
 
    private float generatePerlinNoise (int x, int y,  float noiseScale)
    {
        //NoiseScale determines how smooth or rugged the noise is. Higher value = smoother, lower value = more rugged
        float xNoise = (x + this.transform.position.x + seedX) / noiseScale; //calculates adjusted x coordinate for perlin noise by adding gameobject x val and random seed x value to input x vlaue so that noise is unique
        float yNoise = (y + this.transform.position.y + seedY) / noiseScale; //calculates adjusted y coordinate adds gameobject y and random seed y value to input y vlaue so that noise is unique

        return Mathf.PerlinNoise(xNoise, yNoise);  //generats perlin noise by using built in perlinNoise function
    }
    private Vector3 ObjectSpawnPos()
    {
        int rndIndex = Random.Range(0, landPos.Count);//getting random index number in landPos list to spawn objects
        Vector3 objectPos = new Vector3(
            landPos[rndIndex].x,
            landPos[rndIndex].y + 0.5f,
            landPos[rndIndex].z
        );//getting position of object in landPos list
        landPos.RemoveAt(rndIndex);//to prevent overlapping spawns
        return objectPos;
    }

    private void ObjectSpawn()
    {
        for (int A = 0; A < 8; A++) 
        {
            GameObject spawnObject = Instantiate(treeObject, ObjectSpawnPos(), Quaternion.identity); //spawns object
        }
    }

    
}
