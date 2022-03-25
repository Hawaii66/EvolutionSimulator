using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject ParentObject;
    public GameObject ToSpawn;
    public Vector3 spawnOffset;
    public ThingsOnMap.TypeOnMap ThingOnMapType;
    public int InitialPopulation;

    public float elapsedTime;

    ThingsOnMap stuff;
    TerrainGeneration generation;
    DrynessMap dryness;

    public List<FoodBrain> canSpawnNext = new List<FoodBrain>();

    void Start()
    {
        stuff = FindObjectOfType<ThingsOnMap>();
        generation = FindObjectOfType<TerrainGeneration>();
        dryness = FindObjectOfType<DrynessMap>();

        for (int i = 0; i < InitialPopulation; i++)
        {
            Vector2Int randomPos = GetRandomPos();
            while(randomPos == new Vector2Int(-1, -1))
            {
                randomPos = GetRandomPos();
            }

            canSpawnNext.Add(Spawn(randomPos.x, randomPos.y, new FoodStats(3f, 100, 0.05f, 100f)));
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        List<FoodBrain> canSpawnNextNext = new List<FoodBrain>();

        for (int i = 0; i < canSpawnNext.Count; i++)
        {
            if(canSpawnNext[i].nextTime <= elapsedTime)
            {
                if (Random.Range(0f, 1f) < 0.05)
                {
                    //Bug where creature eats and then tries to access that.
                    if (canSpawnNext[i] == null)
                    {
                        continue;
                    }
                    canSpawnNext[i].nextTime = elapsedTime + canSpawnNext[i].mutation.spreadTime;
                    Vector2Int randomNeighbour = GetPositionNextTo(canSpawnNext[i].transform.position); // HERE ERROR
                    if (randomNeighbour == new Vector2Int(-1, -1))
                    {
                        canSpawnNext[i].nextTime = -1;
                        continue;
                    }
                    else
                    {
                        FoodStats stats = new FoodStats(canSpawnNext[i].mutation);
                        canSpawnNextNext.Add(Spawn(randomNeighbour.x, randomNeighbour.y, stats));
                    }
                }
            }

            canSpawnNextNext.Add(canSpawnNext[i]);
        }

        canSpawnNext = canSpawnNextNext;
    }

    Vector2Int GetPositionNextTo(Vector3 currentPos)
    {
        List<Vector2Int> avaliable = new List<Vector2Int>();
        Vector3Int currentPosInt = Vector3Int.FloorToInt(currentPos);
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if(currentPosInt.x == 0 || currentPosInt.x == generation.xSize - 1 || currentPosInt.z == 0 || currentPosInt.z == generation.zSize - 1)
                {
                    continue;
                }

                Vector2Int newPos = new Vector2Int(currentPosInt.x + x, currentPosInt.z + z);
                if(stuff.StuffOnMap[newPos.x, newPos.y] == ThingsOnMap.TypeOnMap.none){
                    if (generation.mapBiomes[newPos.x, newPos.y].isGrass)
                    {
                        if (dryness.drynessGrid[newPos.x, newPos.y].canSpawnAppleTrees)
                        {
                            avaliable.Add(newPos);
                        }
                    }
                }
            }
        }

        if(avaliable.Count == 0)
        {
            return new Vector2Int(-1, -1);
        }

        int randomIndex = Random.Range(0, avaliable.Count);
        Vector2Int randomObj = avaliable[randomIndex];

        return randomObj;
    }

    Vector2Int GetRandomPos()
    {
        Vector2Int mapSize = new Vector2Int(generation.xSize, generation.zSize);

        int randX = Random.Range(0, mapSize.x);
        int randZ = Random.Range(0, mapSize.y);

        if(generation.mapBiomes[randX, randZ].isGrass == true)
        {
            if (stuff.StuffOnMap[randX, randZ] == 0)
            {
                return new Vector2Int(randX, randZ);
            }
        }

        return new Vector2Int(-1, -1);
    }

    FoodBrain Spawn(int x, int z, FoodStats stats)
    {
        GameObject temp = Instantiate(ToSpawn);

        temp.transform.position = new Vector3(x, 0, z) + spawnOffset;

        MapSpot spot = stuff.AddToMap(x, z, ThingOnMapType, temp.transform);
        temp.GetComponent<FoodBrain>().gridPos = spot.gridPos;
        temp.GetComponent<FoodBrain>().thingsOnMap = stuff;
        temp.GetComponent<FoodBrain>().mutation = stats;

        if (ParentObject != null)
        {
            temp.transform.parent = ParentObject.transform;
        }
        else
        {
            temp.transform.parent = transform;
        }

        return temp.GetComponent<FoodBrain>();
    }
}

[System.Serializable]
public class FoodStats
{
    public float spreadTime;
    public float spreadTimeMutAmount;
    public float energy;
    public float energyMutAmount;

    public FoodStats(float _s, float _e, float _sm, float _em)
    {
        spreadTime = _s;
        energy = _e;
        spreadTimeMutAmount = _sm;
        energyMutAmount = _em;

        Mutate();
    }

    public FoodStats(FoodStats stats)
    {
        spreadTime = stats.spreadTime;
        energy = stats.energy;
        spreadTimeMutAmount = stats.spreadTimeMutAmount;
        energyMutAmount = stats.energyMutAmount;

        Mutate();
    }

    public void Mutate()
    {
        spreadTime += Random.Range(-spreadTimeMutAmount, spreadTimeMutAmount);
        energy += Random.Range(-energyMutAmount, energyMutAmount);

        spreadTime = Mathf.Max(0.1f, spreadTime);
        energy = Mathf.Max(0.1f, energy);
    }
}