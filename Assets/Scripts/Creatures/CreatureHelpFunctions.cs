using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHelpFunctions : MonoBehaviour
{
    [HideInInspector]
    public ThingsOnMap thingsOnMap;
    public TerrainGeneration generation;

    Chunks chunks;

    void Start()
    {
        thingsOnMap = FindObjectOfType<ThingsOnMap>();
        generation = FindObjectOfType<TerrainGeneration>();
        chunks = FindObjectOfType<Chunks>();
    }

    public Vector2Int FindClosestFood(Transform myPos, float senseRadious)
    {
        List<MapSpot> onMap = thingsOnMap.onMapList;

        int closesIndex = -1;
        float dist = Mathf.Infinity;

        myPos.position = new Vector3(myPos.position.x, 0, myPos.position.z);

        Vector3 myPosition = myPos.position;

        List<MapSpot> inrange = new List<MapSpot>();
        /*
        for (int i = 0; i < onMap.Count; i++)
        {
            //Vector2 diff = new Vector2(onMap[i].gridPos.x, onMap[i].gridPos.y) - new Vector2(myPosition.x, myPosition.z);
            MapSpot current = onMap[i];
            float diff1 = current.gridPos.x - myPosition.x;
            float diff2 = current.gridPos.y - myPosition.y;
            if (diff1 < senseRadious)
            {
                if(diff2 < senseRadious)
                {
                    inrange.Add(current);
                }
            }
        }*/

        inrange = chunks.GetMapSpotsFromSouroundChunks(myPos.position, senseRadious);

        for (int i = 0; i < inrange.Count; i++)
        {
            float newDist = Vector3.Distance(myPosition, new Vector3(inrange[i].gridPos.x, 0, inrange[i].gridPos.y));
            //Vector3 newDistV = new Vector3(onMap[i].gridPos.x, 0, onMap[i].gridPos.y) - myPos.position;
            //float newDist = newDistV.sqrMagnitude;
            if (newDist < dist && newDist < senseRadious)
            {
                dist = newDist;
                closesIndex = i;
            }
        }

        if (closesIndex == -1)
        {
            return new Vector2Int(-1, -1);
        }
        return inrange[closesIndex].gridPos;
    }

    public Vector2Int FindClosestWater(Transform myPos, float senseRadious)
    {
        List<MapSpot> shores = generation.shores;

        int closesIndex = -1;
        float dist = Mathf.Infinity;

        myPos.position = new Vector3(myPos.position.x, 0, myPos.position.z);

        for (int i = 0; i < shores.Count; i++)
        {
            float newDist = Vector3.Distance(myPos.position, new Vector3(shores[i].gridPos.x, 0, shores[i].gridPos.y));
            //Vector3 newDistV = new Vector3(shores[i].gridPos.x, 0, shores[i].gridPos.y) - myPos.position;
            //float newDist = newDistV.sqrMagnitude;
            if (newDist < dist && newDist < senseRadious)
            {
                dist = newDist;
                closesIndex = i;
            }
        }

        if (closesIndex == -1)
        {
            return new Vector2Int(-1, -1);
        }

        return shores[closesIndex].gridPos;
    }

    public MapSpot FindMapSpot(Vector2Int pos)
    {
        List<MapSpot> onMap = thingsOnMap.onMapList;

        for (int i = 0; i < onMap.Count; i++)
        {
            if(onMap[i].gridPos == pos)
            {
                return onMap[i];
            }
        }
        return null;
    }

    public Vector2Int FindRandomPosition(Transform myPos, float senseRadious)
    {
        int attempts = 0;
        while (true) {
            Vector2 randomPosInCircle = Random.insideUnitSphere * senseRadious * 2 + myPos.position;
            if (Vector3.Distance(new Vector3(randomPosInCircle.x, myPos.position.y, randomPosInCircle.y), myPos.position) > senseRadious)
            {
                Vector2Int randomGridPos = new Vector2Int(Mathf.RoundToInt(randomPosInCircle.x), Mathf.RoundToInt(randomPosInCircle.y));
                if (randomGridPos.x < 0 || randomGridPos.x >= generation.xSize || randomGridPos.y < 0 || randomGridPos.y >= generation.zSize)
                {
                }
                else
                {
                    if (generation.walkableGrid[randomGridPos.x, randomGridPos.y] == true)
                    {
                        return randomGridPos;
                    }
                }
            }

            attempts += 1;
            if(attempts > 20)
            {
                break;
            }
        }

        return new Vector2Int(-1, -1);
    }

    public bool isOnMap(Vector2Int position, ThingsOnMap.TypeOnMap type)
    {
        if(thingsOnMap.StuffOnMap[position.x, position.y] == type)
        {
            return true;
        }

        return false;
    }
}
