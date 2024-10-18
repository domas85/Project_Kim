using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForPath : BehaviourNode
{
    public WaitForPath(List<BehaviourNode> someChildren) : base(someChildren)
    {
    }
    public override ReturnState Evaluate()
    {
        var returnPath = myBlackBoard.data["Return"] as List<Grid.Tile>;


        Kim kim = myBlackBoard.data["Kim"] as Kim;
        List<Grid.Tile> path = new List<Grid.Tile>();
        if (NodeGrid.instance.path != null)
        {
            path = NodeGrid.instance.ConvertNodePathToTilePath(NodeGrid.instance.path);

        }


        foreach (Node n in NodeGrid.instance.path) //it kinda works ?
        {
            Debug.Log(n.fCost);
            if (n.zThreatLevel !<= 0.2f)
            {
                kim.SetWalkBuffer(returnPath);
                return ReturnState.Success;
            }
        }
        if (path != null)
        {
            kim.SetWalkBuffer(path);
            return ReturnState.Failure;
        }

        return ReturnState.Success;
    }
}
