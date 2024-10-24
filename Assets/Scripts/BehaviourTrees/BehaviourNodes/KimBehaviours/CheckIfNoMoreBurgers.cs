using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfNoMoreBurgers : BehaviourNode
{
    public CheckIfNoMoreBurgers(List<BehaviourNode> someChildren) : base(someChildren)
    {

    }

    List<List<Node>> allBurgersCollected;

    public override ReturnState Evaluate()
    {
        Kim kim = myBlackBoard.data["Kim"] as Kim;

        if (myBlackBoard.data.ContainsKey("BurgersLeft") != false)
        {
            allBurgersCollected = myBlackBoard.data["BurgersLeft"] as List<List<Node>>;
        }

        if (allBurgersCollected != null && allBurgersCollected.Count == 1 && myBlackBoard.data.TryGetValue("Safe", out var value))
        {
            ((List<Vector3>)value).Clear();
            ((List<Vector3>)value).Add(kim.transform.position);
            return ReturnState.Failure;
        }

        if (allBurgersCollected != null && allBurgersCollected.Count == 0)
        {
            return ReturnState.Success;
        }
        else
        {
            return ReturnState.Failure;
        }
    }
}
