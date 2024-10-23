using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class Kim : CharacterController
{
    [SerializeField] float ContextRadius;
    KimBehaviourTree kimBehaviourTree;

    public List<Burger> allBurgers = new();

    List<Grid.Tile> tilesPath;
    List<Node> zombieNeighbours = new List<Node>();
    Zombie closest;
    List<Zombie> zombies;


    public Node lastZombieNode;
    public Node Currentzombie;
    public AnimationCurve zombieThreatLevel;
    List<Node> previousNeightbours = new List<Node>();
    float timer = 3;

    public override void StartCharacter()
    {
        base.StartCharacter();
        FindAllBurgers();
        zombies = FindObjectsOfType<Zombie>().ToList();
        kimBehaviourTree = GetComponent<KimBehaviourTree>();
        kimBehaviourTree.StartKimBehabiour();
    }

    public override void UpdateCharacter()
    {
        base.UpdateCharacter();
        FindAllBurgers();

        //FindShortestPathToTarget(transform.position, burger.position);
        foreach (var zombie in zombies)
        {
            closest = GetClosest(GetContextByTag("Zombie"))?.GetComponent<Zombie>();
        }

        if (closest != null)
        {
            GetZombieDeathZone();
        }

        timer -= Time.deltaTime;

        if(timer <= Time.deltaTime)
        {
            ClearMapOfThreatLevel();
            timer = 3;
        }


    }




    public void FindAllBurgers()
    {
        allBurgers = FindObjectsOfType<Burger>(false).ToList();
    }





    void GetZombieDeathZone()
    {
        if (NodeGrid.instance != null)
        {
            Currentzombie = NodeGrid.instance.NodeFromWorldPoint(closest.transform.position);
            if (lastZombieNode == null)
            {
                lastZombieNode = Currentzombie;
            }

            List<Node> neighbourPoints = new List<Node>();

            zombieNeighbours = NodeGrid.instance.GetZombieNeighbours(Currentzombie);


            if (lastZombieNode == Currentzombie)
            {
                previousNeightbours = zombieNeighbours;
                foreach (Node zombieNode in zombieNeighbours)
                {
                    zombieNode.zCost = (int)zombieThreatLevel.Evaluate(zombieNode.zThreatLevel);
                    //zombieNode.walkable = false;
                }
            }
            if (lastZombieNode != Currentzombie)
            {
                lastZombieNode = Currentzombie;
                foreach (Node zombieNode in previousNeightbours)
                {
                    //zombieNode.walkable = true;
                    //zombieNode.zThreatLevel = 0;
                    zombieNode.zCost = 0;
                }
            }
        }
    }

    void ClearMapOfThreatLevel()
    {
        foreach(Node n in NodeGrid.instance.grid)
        {
            n.zThreatLevel = 0;
        }
    }

    //float nodeDiameter = 0.4f;
    //private void OnDrawGizmos()
    //{
    //    if (NodeGrid.instance != null && zombieNeighbours != null)
    //    {
    //        foreach (Node node in zombieNeighbours)
    //        {
    //            Gizmos.color = (closest == null) ? Color.white : new Color(1, 1 - zombieThreatLevel.Evaluate(node.zThreatLevel) / 1000, 1 - zombieThreatLevel.Evaluate(node.zThreatLevel) / 1000);

    //            Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - .1f));
    //        }
    //    }
    //}


    public void FindShortestPathToTarget(Vector3 startPos, Vector3 targetPos)
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();

        Node startNode = NodeGrid.instance.NodeFromWorldPoint(startPos);
        Node targetNode = NodeGrid.instance.NodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(NodeGrid.instance.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);


        while (openSet.Count > 0)
        {
            //Node currentNode = openSet[0];
            //for (int i = 1; i < openSet.Count; i++)
            //{
            //    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
            //    {
            //        currentNode = openSet[i];
            //    }
            //}
            //openSet.Remove(currentNode);

            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                //sw.Stop();
                //print("Path found in: " + sw.Elapsed);
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in NodeGrid.instance.GetNeighbours(currentNode))
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

        path.Reverse();

        NodeGrid.instance.returnPath = path;
        NodeGrid.instance.path = path;
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



    public Vector3 GetEndPoint()
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
