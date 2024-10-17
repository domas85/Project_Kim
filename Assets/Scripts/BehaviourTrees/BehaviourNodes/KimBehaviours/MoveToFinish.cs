using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToFinish : BehaviourNode
{
    public MoveToFinish(List<BehaviourNode> someChildren) : base(someChildren)
    {
    }

    public override ReturnState Evaluate()
    {
        Kim kim = myBlackBoard.data["Kim"] as Kim;
        List<Grid.Tile> tilePath = new List<Grid.Tile>();
        
        kim.FindShortestPathToTarget(kim.transform.position, kim.GetEndPoint());


        if (NodeGrid.instance.path != null)
        {
            tilePath = NodeGrid.instance.ConvertNodePathToTilePath(NodeGrid.instance.path);

        }
        if (tilePath != null)
        {
            kim.SetWalkBuffer(tilePath);
            return ReturnState.Success;
        }
        return ReturnState.Failure;
    }
}
