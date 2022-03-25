using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrynessMap : MonoBehaviour
{
    public DryLevel[] dryLevels;

    int xSize, zSize;
    public int scale;
    public int seed;

    public Renderer textureRenderer;
    Mesh mesh;
    Vector3[] vertices;
    int[] triangels;
    Color[] colorMap;
    Vector2[] uvs;

    float[,] noiseMap;

    public DryLevel[,] drynessGrid;

    public void ReDraw()
    {
        GenerateColorTexture(noiseMap);
        UpdateMesh(TextureFromColors());
    }

    public void SetUp(int gridX, int gridY, int _seed)
    {
        seed = _seed;
        drynessGrid = new DryLevel[gridX + 1, gridY + 1];
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        xSize = gridX;
        zSize = gridY;
        noiseMap = GenerateShape();
        GenerateColorTexture(noiseMap);
        UpdateMesh(TextureFromColors());
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

    void GenerateColorTexture(float[,] noiseMap)
    {
        colorMap = new Color[vertices.Length];

        for (int i = 0, z = 0; z < zSize + 1; z++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {
                float height = noiseMap[x, z];
                for (int j = 0; j < dryLevels.Length; j++)
                {
                    if (dryLevels[j].dryness >= height)
                    {
                        drynessGrid[x, z] = dryLevels[j];
                        colorMap[i] = dryLevels[j].myColor;
                        break;
                        //colorMap[i] = colorGradient.Evaluate(height);
                    }
                }
                i++;
            }
        }
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
}

[System.Serializable]
public struct DryLevel
{
    public float dryness;
    public Color myColor;
    public bool canSpawnAppleTrees;
}