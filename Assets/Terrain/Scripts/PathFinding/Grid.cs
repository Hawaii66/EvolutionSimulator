using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [HideInInspector]
    public Vector2 gridWorldSize;
    public float nodeRadious;
    public Node[,] grid;

    float nodeDiameter;
    public int gridSizeX, gridSizeY;

    public bool drawGizmos;
    bool[,] walkable;

    void Awake()
    {
        TerrainGeneration terrainGen = FindObjectOfType<TerrainGeneration>();
        walkable = terrainGen.walkableGrid;

        nodeDiameter = nodeRadious * 2;

        gridWorldSize = new Vector2(terrainGen.xSize, terrainGen.zSize);
        transform.position = new Vector3(gridWorldSize.x / 2 - 0.5f, 1, gridWorldSize.y / 2 - 0.5f);

        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();

        GetComponent<PathFinding>().StartEd();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadious) + Vector3.forward * (y * nodeDiameter + nodeRadious);
                grid[x, y] = new Node(walkable[x, y], worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        worldPosition = new Vector3(Mathf.Clamp(worldPosition.x, 0, gridSizeX - 1), worldPosition.y, Mathf.Clamp(worldPosition.z, 0, gridSizeY - 1));
        return grid[Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.z)];

        /*
        worldPosition = new Vector3(worldPosition.x - gridWorldSize.x / 2, worldPosition.y, worldPosition.z - gridWorldSize.y / 2);
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];*/
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        
        if (grid != null && drawGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
