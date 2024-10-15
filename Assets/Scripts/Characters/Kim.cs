using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Kim : CharacterController
{
    [SerializeField] float ContextRadius;
    [SerializeField] NodeGrid grid;
    [SerializeField] Transform burger;
    List<Grid.Tile> tilesPath;
    List<Node> zombieNeighbours;
    Zombie closest;
    public override void StartCharacter()
    {
        base.StartCharacter();
    }

    public override void UpdateCharacter()
    {
        base.UpdateCharacter();

        FindShortestPathToTarget(transform.position, burger.position);
        closest = GetClosest(GetContextByTag("Zombie"))?.GetComponent<Zombie>();
        if (closest != null)
        {
            GetallZombies();
        }

        if (tilesPath != null)
        {
            // Debug.Log("go farward");
            SetWalkBuffer(tilesPath);
        }
    }

    void ResetZombieNeighbours()
    {
        if (grid != null && zombieNeighbours != null)
        {
            List<Node> previousNeightbours = new List<Node>();
            previousNeightbours = zombieNeighbours;
            foreach (Node zombieNode in previousNeightbours)
            {
                zombieNode.walkable = true;
            }
        }
    }
    public Vector3 lastTransformPos;
    public Node lastZombieNode;
    public Node Currentzombie;
    void GetallZombies()
    {
        if (grid != null)
        {
            Currentzombie = grid.NodeFromWorldPoint(closest.transform.position);
            if (lastZombieNode != null)
            {
                lastZombieNode = Currentzombie;
            }
            zombieNeighbours = grid.GetNeighbours(Currentzombie);

            for (int i = 0; i < 8; i++)
            {
                zombieNeighbours.AddRange(grid.GetNeighbours(zombieNeighbours[i]));
            }

            var diffVector = closest.transform.position - lastTransformPos;
            if (lastZombieNode != Currentzombie)
            {

                lastTransformPos = closest.transform.position;
                foreach (Node zombieNode in zombieNeighbours)
                {


                    zombieNode.walkable = true;


                }
                lastZombieNode = Currentzombie;
            }
            else if(lastZombieNode == Currentzombie)
            {
                List<Node> previousNeightbours = new List<Node>();
                previousNeightbours = zombieNeighbours;
                foreach (Node zombieNode in zombieNeighbours)
                {
                    zombieNode.walkable = false;
                }

            }

        }
    }

    float nodeDiameter = 0.4f;
    private void OnDrawGizmos()
    {
        if (grid != null && zombieNeighbours != null)
        {
            foreach (Node node in zombieNeighbours)
            {
                Gizmos.color = (closest == null) ? Color.white : Color.black;

                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }


    public List<Grid.Tile> GetNeighbours(Grid.Tile tile)
    {
        List<Grid.Tile> neighbours = new List<Grid.Tile>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = tile.x + x;
                int checkY = tile.y + y;

                if (checkX >= 0 && checkX < 40 && checkY >= 0 && checkY < 40)
                {
                    var tileToAdd = new Grid.Tile();
                    tileToAdd.x = checkX;
                    tileToAdd.y = checkY;
                    neighbours.Add(tileToAdd);
                }
            }
        }
        return neighbours;
    }




    void FindShortestPathToTarget(Vector3 startPos, Vector3 targetPos)
    {
        //List<Grid.Tile> alltiles = Grid.Instance.GetTiles();

        //Grid.Tile startNode = Grid.Instance.GetClosest(transform.position);
        //Grid.Tile targetNode = Grid.Instance.GetFinishTile();

        //List<Grid.Tile> openSet = new List<Grid.Tile>();
        //HashSet<Grid.Tile> closedSet = new HashSet<Grid.Tile>();


        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);


        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                Debug.Log("path found");
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }

            }
        }
    }

    void FindPathWithoutZombies(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                Debug.Log("path found");
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }

            }
        }
    }


    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        tilesPath = grid.ConvertNodePathToTilePath(path);
        path.Reverse();

        grid.path = path;
    }


    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        else
        {
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }



    Vector3 GetEndPoint()
    {
        return Grid.Instance.WorldPos(Grid.Instance.GetFinishTile());
    }

    GameObject[] GetContextByTag(string aTag)
    {
        Collider[] context = Physics.OverlapSphere(transform.position, ContextRadius);
        List<GameObject> returnContext = new List<GameObject>();
        foreach (Collider c in context)
        {
            if (c.transform.CompareTag(aTag))
            {
                returnContext.Add(c.gameObject);
            }
        }
        return returnContext.ToArray();
    }

    GameObject GetClosest(GameObject[] aContext)
    {
        float dist = float.MaxValue;
        GameObject Closest = null;
        foreach (GameObject z in aContext)
        {
            float curDist = Vector3.Distance(transform.position, z.transform.position);
            if (curDist < dist)
            {
                dist = curDist;
                Closest = z;
            }
        }
        return Closest;
    }
}
