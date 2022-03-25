using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenerateTerrain : MonoBehaviour
{
    /*
    public int mapWidth;
    public int mapHeight;
    public float scale;
    public int seed;

    public GameObject[] StonePrefab;
    public GameObject GrassPrefab;
    public GameObject[] WaterPrefab;

    public int blocksize;

    public float[] mapLevels;

    public Transform TerrainParent;
    public NavMeshSurface landsurface;

    float[,] noiseMap;

    public GameObject[,] mapGridObjects;

    void Start()
    {
        mapGridObjects = new GameObject[mapWidth, mapHeight];

        GenerateBase();
        landsurface.BuildNavMesh();
        GameObject.Find("GameManager").GetComponent<ThingsOnMap>().Started(mapWidth, mapHeight, blocksize);
    }

    void GenerateBase()
    {
        noiseMap = NoiseGenerator.NoiseMap(mapWidth, mapHeight, scale, seed);

        for(int x = 0; x < mapWidth; x ++)
        {
            for(int y = 0; y < mapHeight; y ++)
            {
                //Debug.Log(noiseMap[x, y]);
                if(noiseMap[x,y] < mapLevels[0])
                {
                    GenerateBlock(x, y, 0, false);
                }
                else if(noiseMap[x,y] < mapLevels[1])
                {
                    GenerateBlock(x, y, 1, false);
                }
                else if(noiseMap[x,y] < mapLevels[2])
                {
                    GenerateBlock(x, y, 2, false);
                }
                else if(noiseMap[x,y] < mapLevels[3])
                {
                    GenerateBlock(x, y, 3, true);
                }
            }
        }
    }

    void GenerateBlock(int x, int y, int index, bool abowe)
    {
        GameObject current = null;
        if(index == 0) // DeepWater
        {
            current = WaterPrefab[1];
        }
        if(index == 1) // Water
        {
            current = WaterPrefab[0];
        }
        if(index == 2) // Grass
        {
            current = GrassPrefab;
        }
        if(index == 3) // Mountain
        {
            current = StonePrefab[0];
        }
        if(current != null)
        {
            if(abowe)
            {
                GameObject terrain;
                terrain = Instantiate(current, new Vector3(x * blocksize, 2, y * blocksize), transform.rotation); 
                terrain.transform.parent = TerrainParent;
                mapGridObjects[x, y] = terrain;
            } else {
                GameObject terrain;
                terrain = Instantiate(current, new Vector3(x * blocksize, 0, y * blocksize), transform.rotation);
                terrain.transform.parent = TerrainParent;
                mapGridObjects[x, y] = terrain;
            }
        }
    }

    void FixMountains()
    {
        for(int x = 0; x < mapWidth; x ++)
        {
            for(int y = 0; y < mapWidth; y ++)
            {
                bool right = false;
                bool left = false;
                bool up = false;
                bool down = false;
                if(x != 0)
                {
                    if(mapGridObjects[x - 1, y].tag == "Mountain")
                    {
                        left = true;
                    }
                }
                if(y != 0)
                {
                    if(mapGridObjects[x, y - 1].tag == "Mountain")
                    {
                        down = true;
                    }
                }
                if(x != mapWidth - 1)
                {
                    if(mapGridObjects[x + 1, y].tag == "Mountain")
                    {
                        right = true;
                    }
                }
                if(y != mapHeight - 1)
                {
                    if(mapGridObjects[x, y + 1].tag == "Mountain")
                    {
                        up = true;
                    }
                }

                if(up)
                {
                    if(down)
                    {
                        
                    } else if(right)
                    {
                        
                    } else if(left)
                    {
                         
                    } else {

                    }
                }
            }
        }
    }
    */
}   
