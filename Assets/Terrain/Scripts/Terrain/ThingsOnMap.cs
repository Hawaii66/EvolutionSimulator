using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingsOnMap : MonoBehaviour
{
    public enum TypeOnMap { none, apple, water}
    public TypeOnMap[,] StuffOnMap;
    public Transform[,] StuffOnMapTransform;

    public List<MapSpot> onMapList = new List<MapSpot>();

    int blocksize;

    Chunks chunks;

    void Start()
    {
        blocksize = FindObjectOfType<TerrainGeneration>().blocksize;
    }
    public void Started(int width, int height)
    {
        chunks = FindObjectOfType<Chunks>();
        StuffOnMap = new TypeOnMap[width,height];
        StuffOnMapTransform = new Transform[width, height];
        for(int x = 0; x < width; x ++)
        {
            for(int y = 0; y < width; y ++)
            {
                StuffOnMap[x,y] = TypeOnMap.none;
                StuffOnMapTransform[x, y] = null;
            }
        }
    }

    public MapSpot AddToMap(int x, int z, TypeOnMap number, Transform t)
    {
        StuffOnMap[x, z] = number;
        StuffOnMapTransform[x, z] = t;

        MapSpot temp = new MapSpot(new Vector2Int(x, z), t, number);
        onMapList.Add(temp);

        chunks.AddToChunk(temp);

        return temp;
    }

    public void DeleteFromMap(int x, int z)
    {
        StuffOnMap[x, z] = 0;
        StuffOnMapTransform[x, z] = null;

        for (int i = 0; i < onMapList.Count; i++)
        {
            if (onMapList[i].gridPos == new Vector2Int(x, z))
            {
                chunks.RemoveFromChunk(onMapList[i]);
                onMapList.RemoveAt(i);
                break;
            }
        }
    }
}

[System.Serializable]
public class MapSpot
{
    public Vector2Int gridPos;
    public Transform transform;
    public ThingsOnMap.TypeOnMap type;

    public MapSpot(Vector2Int _g, Transform _t, ThingsOnMap.TypeOnMap _type)
    {
        gridPos = _g;
        transform = _t;
        type = _type;
    }
}