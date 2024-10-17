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
        if (NodeGrid.instance.path != null)
        {
            path = NodeGrid.instance.ConvertNodePathToTilePath(NodeGrid.instance.path);
          
        }
        foreach(Node n in NodeGrid.instance.path)
        {
            Debug.Log(n.fCost);
            if(n.fCost > 5000000)
            {

                return ReturnState.Failure;
            }
            else if(path != null)
            {
                kim.SetWalkBuffer(path);
                return ReturnState.Success;
            }
        }
 
        return ReturnState.Failure;
    }

}
