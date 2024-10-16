using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourNode
{
    public enum ReturnState
    {
        Success,
        Running,
        Failure
    }

    public BehaviourNode(List<BehaviourNode> someChildren)
    {
        myChildren = someChildren;

        foreach (BehaviourNode c in myChildren)
        {
            if (c != null)
            {
                c.Parent = this;
            }
        }
    }

    protected List<BehaviourNode> myChildren = new List<BehaviourNode>();
    protected BlackBoard myBlackBoard = null;
    protected BehaviourNode Parent;

    public void PopulateBlackBoard(BlackBoard aBlackBoard)
    {
        myBlackBoard = aBlackBoard;
        foreach (BehaviourNode child in myChildren)
        {
            child.PopulateBlackBoard(myBlackBoard);
        }
    }


    public virtual ReturnState Evaluate() => ReturnState.Failure;

}
