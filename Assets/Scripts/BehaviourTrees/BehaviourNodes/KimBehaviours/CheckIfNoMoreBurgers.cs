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
        if (myBlackBoard.data.ContainsKey("BurgersLeft") != false)
        {
            allBurgersCollected = myBlackBoard.data["BurgersLeft"] as List<List<Node>>;
        }

        if (allBurgersCollected != null && allBurgersCollected.Count == 0)
        {
            //Debug.Log("yay burger");
            return ReturnState.Success;
        }
        else
        {
            return ReturnState.Failure;
        }

    }
}
