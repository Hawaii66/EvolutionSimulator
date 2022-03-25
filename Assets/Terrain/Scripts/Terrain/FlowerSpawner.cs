using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
    public float spawnRate;
    float timer;
    public float growRate;
    public float lifetime;
    public int spawnattempts;
    
    public GameObject flowerPrefab;
    public Transform flowerHolder;

    public TerrainGeneration terrain;
    public ThingsOnMap things;

    void Start()
    {
        things = FindObjectOfType<ThingsOnMap>();
        terrain = FindObjectOfType<TerrainGeneration>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            SpawnTest();
            timer = spawnRate;
        }
    }

    void SpawnTest()
    {
        for(int i = 0; i < spawnattempts; i ++)
        {
            int Randomx = Random.Range(0, terrain.xSize);
            int Randomy = Random.Range(0, terrain.zSize);
            if(terrain.mapBiomes[Randomx, Randomy].name == "Grass")
            {
                if(things.StuffOnMap[Randomx, Randomy] == 0)
                {
                    SpawnFlower(Randomx, Randomy);
                    break;
                }
            }
        }
    }

    public void SpawnChildFlower(int x, int y)
    {
        if(terrain.mapBiomes[x, y].name == "Grass")
        {
            if(things.StuffOnMap[x, y] == 0)
            {
                SpawnFlower(x, y);
            }
        }
    }
    void SpawnFlower(int x, int y)
    {

    }
}
