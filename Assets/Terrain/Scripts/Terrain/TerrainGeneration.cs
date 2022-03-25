using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    Mesh mesh;
    public Renderer textureRenderer;
    public bool useGizmos;
    public bool drawShore;
    public bool drawTiles;
    Vector3[] vertices;
    int[] triangels;
    Color[] colorMap;
    Vector2[] uvs;

    public bool useScriptable;
    public TerrainRegion scriptableColors;
    public TerrainType[] regions;
    [HideInInspector]
    public TerrainType[,] mapBiomes;

    public int xSize = 20;
    public int zSize = 20;
    public float scale;
    public int seed;
    public int blocksize;

    public bool[,] walkableGrid;

    public Gradient colorGradient;

    public List<MapSpot> shores;

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        if(useScriptable)
        {
            regions = scriptableColors.regions;
        }

        GenerateColorTexture(GenerateShape());
        UpdateMesh(TextureFromColors());

        FindObjectOfType<DrynessMap>().SetUp(xSize, zSize, seed + 1);

        //Populate things on map
        FindObjectOfType<ThingsOnMap>().Started(xSize, zSize);
    }

    float[,] GenerateShape()
    {
        float[,] noiseMap = NoiseGenerator.NoiseMap(xSize + 1, zSize + 1, scale, seed);

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z < zSize + 1; z++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {
                float height = noiseMap[x, z];
                vertices[i] = new Vector3(x, height, z);
                i++;
            }

        }

        triangels = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangels[tris + 0] = vert + 0;
                triangels[tris + 1] = vert + xSize + 1;
                triangels[tris + 2] = vert + 1;
                triangels[tris + 3] = vert + 1;
                triangels[tris + 4] = vert + xSize + 1;
                triangels[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        uvs = new Vector2[vertices.Length];

        for (int i = 0, z = 0; z < zSize + 1; z++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {
                uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
                i++;
            }

        }

        return noiseMap;
    }

    Texture2D TextureFromColors()
    {
        Texture2D texture = new Texture2D(xSize + 1, zSize + 1);
        texture.SetPixels(colorMap);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }

    void GenerateColorTexture(float[,] noiseMap)
    {
        colorMap = new Color[vertices.Length];
        walkableGrid = new bool[xSize + 1, zSize + 1];
        mapBiomes = new TerrainType[xSize + 1, zSize + 1];

        for (int i = 0, z = 0; z < zSize + 1; z++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {
                float height = noiseMap[x, z];
                for (int j = 0; j < regions.Length; j++)
                {
                    if(regions[j].noiseHeight >= height)
                    {
                        colorMap[i] = regions[j].myColor;
                        vertices[i].y = regions[j].finalHeight;
                        walkableGrid[x, z] = regions[j].isWalkable;
                        mapBiomes[x, z] = regions[j];
                        break;
                        //colorMap[i] = colorGradient.Evaluate(height);
                    }
                }
                i++;
            }
        }

        for (int z = 0; z < zSize + 1; z++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {
                if(mapBiomes[x, z].isSand)
                {
                    bool hasWater = false;
                    if (x > 0)
                    {
                        if (mapBiomes[x - 1, z].isWater)
                        {
                            hasWater = true;
                            //shores.Add(new MapSpot(new Vector2Int(x, z), null, 2));
                        }
                    }
                    if (x < xSize)
                    {
                        if (mapBiomes[x + 1, z].isWater)
                        {
                            hasWater = true;
                            //shores.Add(new MapSpot(new Vector2Int(x, z), null, 2));
                        }
                    }
                    if (z > 0)
                    {
                        if (mapBiomes[x, z - 1].isWater)
                        {
                            hasWater = true;
                            //shores.Add(new MapSpot(new Vector2Int(x, z), null, 2));
                        }
                    }
                    if (z < zSize)
                    {
                        if (mapBiomes[x, z + 1].isWater)
                        {
                            hasWater = true;
                            //shores.Add(new MapSpot(new Vector2Int(x, z), null, 2));
                        }
                    }
                    if (hasWater)
                    {
                        shores.Add(new MapSpot(new Vector2Int(x, z), null, ThingsOnMap.TypeOnMap.water));
                    }
                }
            }
        }

        //Debug.Log(colorMap.Length);
    }

    void UpdateMesh(Texture2D texture)
    {
        mesh.Clear();
        //Debug.Log(vertices.Length);
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = triangels;
        mesh.uv = uvs;
        textureRenderer.material.mainTexture = texture;

        //mesh.RecalculateNormals();
        //mesh.RecalculateTangents();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(xSize / 2, 0, zSize / 2), new Vector3(xSize, 1, zSize));

        if (!useGizmos) { return; }

        if (drawTiles)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }

        if (drawShore)
        {
            for (int i = 0; i < shores.Count; i++)
            {
                Gizmos.DrawWireSphere(new Vector3(shores[i].gridPos.x, 1, shores[i].gridPos.y), 0.1f);
            }
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float noiseHeight;
    public Color myColor;
    public float finalHeight;
    public bool isWalkable;
    public bool isWater;
    public bool isGrass;
    public bool isMountain;
    public bool isSand;
}
