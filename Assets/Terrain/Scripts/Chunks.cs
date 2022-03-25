using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunks : MonoBehaviour
{
    public Chunk[,] chunks;
    int chunkSize = 16;

    public bool drawChunkBorders;
    public bool LogChunkCoords;

    public Vector2Int currentMousePos;
    public List<Chunk> sensechuks;

    void Awake()
    {
        TerrainGeneration generation = FindObjectOfType<TerrainGeneration>();
        int gridX = generation.xSize;
        int gridZ = generation.zSize;

        chunks = new Chunk[gridX / chunkSize, gridZ / chunkSize];

        for (int x = 0; x < chunks.GetLength(0); x++)
        {
            for (int z = 0; z < chunks.GetLength(1); z++)
            {
                chunks[x, z] = new Chunk(x, z);
            }
        }
    }

    public void AddToChunk(MapSpot spot)
    {
        GetChunkFromGridCoords(spot.gridPos).AddToChunk(spot);
    }

    public void RemoveFromChunk(MapSpot spot)
    {
        GetChunkFromGridCoords(spot.gridPos).RemoveFromChunk(spot);
    }

    public List<MapSpot> GetMapSpotsFromSouroundChunks(Vector3 pos, float senseRadious)
    {
        Vector2Int chunkPos = GetChunkCoordsFromWorld(pos);
        currentMousePos = chunkPos;

        int senseChunks = Mathf.CeilToInt(senseRadious / chunkSize);

        sensechuks = new List<Chunk>();

        List<MapSpot> spots = new List<MapSpot>();

        for (int x = chunkPos.x - senseChunks; x < chunkPos.x + senseChunks; x++)
        {
            for (int z = chunkPos.y - senseChunks; z < chunkPos.y + senseChunks; z++)
            {
                if(x < 0 || z < 0 || x > chunks.GetLength(0) - 1  || z > chunks.GetLength(1) - 1)
                {
                    continue;
                }
                List<MapSpot> chunkSpots = GetChunkFromChunkCoords(new Vector2Int(x, z)).inChunk;
                sensechuks.Add(GetChunkFromChunkCoords(new Vector2Int(x, z)));

                for (int i = 0; i < chunkSpots.Count; i++)
                {
                    spots.Add(chunkSpots[i]);
                }
            }
        }

        return spots;
    }

    Vector2Int GetChunkCoordsFromWorld(Vector3 pos)
    {
        return new Vector2Int(Mathf.FloorToInt(pos.x / chunkSize), Mathf.FloorToInt(pos.z / chunkSize));
    }

    Chunk GetChunkFromChunkCoords(Vector2Int pos)
    {
        return chunks[pos.x, pos.y];

    }

    Chunk GetChunkFromGridCoords(Vector2Int pos)
    {
        int chunkX = Mathf.FloorToInt(pos.x / chunkSize);
        int chunkZ = Mathf.FloorToInt(pos.y / chunkSize);

        if (LogChunkCoords)
        {
            //Debug.Log(chunkX.ToString() + ":" + chunkZ.ToString());
        }

        return chunks[chunkX, chunkZ];
    }
    
    void OnDrawGizmos()
    {
        if (drawChunkBorders && chunks != null)
        {
            for (int x = 0; x < chunks.GetLength(0); x++)
            {
                for (int z = 0; z < chunks.GetLength(1); z++)
                {
                    Gizmos.DrawWireCube(new Vector3(x * chunkSize + chunkSize / 2, 2, z * chunkSize + chunkSize / 2), new Vector3(chunkSize, 1, chunkSize));
                }
            }

            for (int i = 0; i < sensechuks.Count; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(new Vector3(sensechuks[i].x * chunkSize + chunkSize / 2, 2, sensechuks[i].z * chunkSize + chunkSize / 2), new Vector3(chunkSize, 1, chunkSize));
            }


            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(currentMousePos.x * chunkSize + chunkSize / 2, 2, currentMousePos.y * chunkSize + chunkSize / 2), new Vector3(chunkSize, 1, chunkSize));


        }
    }
}

public class Chunk
{
    public int x;
    public int z;
    public List<MapSpot> inChunk;

    public Chunk(int _x, int _z)
    {
        x = _x;
        z = _z;
        inChunk = new List<MapSpot>();
    }

    public List<MapSpot> AddToChunk(MapSpot spot)
    {
        inChunk.Add(spot);
        return inChunk;
    }

    public List<MapSpot> RemoveFromChunk(MapSpot spot)
    {
        inChunk.Remove(spot);
        return inChunk;
    }

    public Vector3 GetCenter(int chunkSize, float y)
    {
        return new Vector3(x + chunkSize / 2, y, z + chunkSize / 2);
    }
}
