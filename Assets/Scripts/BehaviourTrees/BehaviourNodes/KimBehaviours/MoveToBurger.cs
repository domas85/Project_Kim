using System.Collections;
using System.Collections.Generic;
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
        List<Grid.Tile> path = new List<Grid.Tile>();
        if (kim.grid.path != null)
        {
            path = kim.grid.ConvertNodePathToTilePath(kim.grid.path);
        }
        if (path != null)
        {
            kim.SetWalkBuffer(path);
            return ReturnState.Success;
        }

        return ReturnState.Failure;
    }

}
