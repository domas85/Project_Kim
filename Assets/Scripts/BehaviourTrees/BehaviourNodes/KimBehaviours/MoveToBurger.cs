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

    List<Grid.Tile> returnPath = new List<Grid.Tile>();

    public override ReturnState Evaluate()
    {



        Kim kim = myBlackBoard.data["Kim"] as Kim;
        List<Vector3> safePoints = myBlackBoard.data["Safe"] as List<Vector3>;
        List<Grid.Tile> TilePath = new List<Grid.Tile>();
        List<Grid.Tile> reversedPath = new List<Grid.Tile>();
        var path = NodeGrid.instance.path;
        if (NodeGrid.instance.path != null)
        {
            TilePath = NodeGrid.instance.ConvertNodePathToTilePath(NodeGrid.instance.path);
            reversedPath = NodeGrid.instance.ConvertNodePathToTilePath(NodeGrid.instance.path);

        }
        reversedPath.Reverse();




        //for (int i = 0; i < TilePath.Count / 2; i++)
        //{
        //    returnPath.Add(TilePath[i]);
        //}

        //if (myBlackBoard.data.ContainsKey("Return") == false)
        //{
        //    myBlackBoard.data.Add("Return", returnPath);
        //}
        //var returnPathData = myBlackBoard.data["Return"] as List<Grid.Tile>;
        //if(returnPath != null && returnPath != returnPathData)
        //{
        //    myBlackBoard.data["Return"] = returnPath;
        //}

        foreach (Node n in NodeGrid.instance.path)
        {
            if (kim.zombieThreatLevel.Evaluate(n.zThreatLevel) / 1000f < 0.6f && myBlackBoard.data.TryGetValue("Safe", out var value))
            {
                if (Vector3.Distance(((List<Vector3>)value)[((List<Vector3>)value).Count - 1], kim.transform.position) >= 5)
                {
                    ((List<Vector3>)value).Add(kim.transform.position);
                }
            }
            //Debug.Log(n.fCost);
            if (kim.zombieThreatLevel.Evaluate(n.zThreatLevel) / 1000f >= 0.3f)
            {

                return ReturnState.Failure;
            }
        }
        if (TilePath != null)
        {
            kim.SetWalkBuffer(TilePath);
            return ReturnState.Success;
        }

        return ReturnState.Failure;
    }

}
