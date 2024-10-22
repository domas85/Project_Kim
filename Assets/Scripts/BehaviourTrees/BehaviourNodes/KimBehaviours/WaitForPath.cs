using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaitForPath : BehaviourNode
{
    public WaitForPath(List<BehaviourNode> someChildren) : base(someChildren)
    {
    }
    public override ReturnState Evaluate()
    {
        var returnPoints = myBlackBoard.data["Safe"] as List<Vector3>;


        Kim kim = myBlackBoard.data["Kim"] as Kim;
        List<Grid.Tile> path = new List<Grid.Tile>();
        List<Grid.Tile> pathToRetreat = new List<Grid.Tile>();
        if (NodeGrid.instance.path != null)
        {
            path = NodeGrid.instance.ConvertNodePathToTilePath(NodeGrid.instance.path);
            kim.FindShortestPathToTarget(kim.transform.position, returnPoints[returnPoints.Count - 1]);
            pathToRetreat = NodeGrid.instance.ConvertNodePathToTilePath(NodeGrid.instance.returnPath);
        }


        foreach (Node n in NodeGrid.instance.path) //it kinda works ?
        {
            //Debug.Log(n.fCost);
            if (kim.zombieThreatLevel.Evaluate(n.zThreatLevel) / 1000f >= 0.1f)
            {
                kim.SetWalkBuffer(pathToRetreat);
                return ReturnState.Success;
            }
        }

        if (path != null && pathToRetreat.Count < pathToRetreat.Count / 2)
        {

            //kim.SetWalkBuffer(path);
            return ReturnState.Failure;
        }

        return ReturnState.Success;
    }
}
