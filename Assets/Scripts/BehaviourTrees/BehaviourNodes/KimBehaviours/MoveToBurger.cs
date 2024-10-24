using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class MoveToBurger : BehaviourNode
{
    public MoveToBurger(List<BehaviourNode> someChildren) : base(someChildren)
    {

    }

    public override ReturnState Evaluate()
    {
        Kim kim = myBlackBoard.data["Kim"] as Kim;
        List<Grid.Tile> tilePath = new List<Grid.Tile>();
        List<Node> path = NodeGrid.instance.path;
        List<Node> firstFiveNodes;

        if (NodeGrid.instance.path != null)
        {
            tilePath = NodeGrid.instance.ConvertNodePathToTilePath(path);
        }

        if (path.Count >= 5)
        {
            firstFiveNodes = NodeGrid.instance.path.GetRange(0, 5);
        }
        else
        {
            firstFiveNodes = NodeGrid.instance.path;
        }

        foreach (Node n in firstFiveNodes)
        {
            if (kim.zombieThreatLevel.Evaluate(n.zThreatLevel) / 1000f < 0.6f && myBlackBoard.data.TryGetValue("Safe", out var value))
            {
                var safePointData = (List<Vector3>)value;

                if (Vector3.Distance(safePointData[safePointData.Count - 1], kim.transform.position) >= 5)
                {
                    safePointData.Add(kim.transform.position);
                }
            }

            if (kim.zombieThreatLevel.Evaluate(n.zThreatLevel) / 1000f >= 0.1f)
            {
                return ReturnState.Failure;
            }
        }

        if (tilePath != null)
        {
            kim.SetWalkBuffer(tilePath);
            return ReturnState.Success;
        }
        return ReturnState.Failure;
    }
}
