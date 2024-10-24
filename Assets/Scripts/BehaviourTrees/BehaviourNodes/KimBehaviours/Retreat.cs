using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Retreat : BehaviourNode
{
    public Retreat(List<BehaviourNode> someChildren) : base(someChildren)
    {

    }

    public override ReturnState Evaluate()
    {
        List<Vector3> returnPoints = myBlackBoard.data["Safe"] as List<Vector3>;
        Kim kim = myBlackBoard.data["Kim"] as Kim;
        List<Grid.Tile> pathToRetreat = new List<Grid.Tile>();

        if (NodeGrid.instance.path != null)
        {
            kim.FindShortestPathToTarget(kim.transform.position, returnPoints[returnPoints.Count - 1]);
            pathToRetreat = NodeGrid.instance.ConvertNodePathToTilePath(NodeGrid.instance.path);
        }

        foreach (Node n in NodeGrid.instance.path)
        {
            if (kim.zombieThreatLevel.Evaluate(n.zThreatLevel) / 1000f > 0.001f)
            {
                kim.SetWalkBuffer(pathToRetreat);
                return ReturnState.Running;
            }
        }

        if (pathToRetreat.Count == 0)
        {
            return ReturnState.Failure;
        }

        if (pathToRetreat.Count != 0)
        {
            kim.SetWalkBuffer(pathToRetreat);
            return ReturnState.Success;
        }
        return ReturnState.Running;
    }
}
