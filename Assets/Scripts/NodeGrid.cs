using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
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
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> path;
    public List<Node> zombieNeighbours;

    List<Grid.Tile> tilespath;

    public List<Grid.Tile> ConvertNodePathToTilePath(List<Node> path)
    {
        tilespath = new List<Grid.Tile>();

        foreach (Node node in path)
        {
            Grid.Tile tile = new Grid.Tile();
            tile.x = node.gridX;
            tile.y = node.gridY;
            tilespath.Add(tile);
        }
        tilespath.Reverse();
        return tilespath;
    }


    float Spacing = 0.4f;
    float VisualTileSize = 0.79f;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                if (path != null)
                {
                    if (path.Contains(node))
                    {
                        Gizmos.color = Color.blue;
                    }
                }
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
            foreach (Grid.Tile tile in tilespath)
            {
                Gizmos.color = Color.yellow;
                Vector3 cubeSize = new Vector3(Spacing * VisualTileSize, 0.1f, Spacing * VisualTileSize);


                Vector3 cubePos = new Vector3();

                cubePos.x = (tile.x * Spacing) + Spacing / 2.0f;
                cubePos.z = (tile.y * Spacing) + Spacing / 2.0f;

                Gizmos.DrawWireCube(cubePos + OffsetPos(), cubeSize);
            }
        }
    }

    Vector3 OffsetPos()
    {
        Vector3 offset = new Vector3();

        offset.x = transform.position.x - (Spacing * 40) / 2;
        offset.z = transform.position.z - (Spacing * 40) / 2;
        offset.y = transform.position.y;

        return offset;
    }
}
