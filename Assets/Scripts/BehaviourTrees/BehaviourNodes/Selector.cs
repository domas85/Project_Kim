using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : BehaviourNode
{
    public Selector(List<BehaviourNode> someChildren) : base(someChildren)
    {
    }

    public override ReturnState Evaluate()
    {
        ReturnState aState = ReturnState.Failure;


        foreach (BehaviourNode child in myChildren)
        {
            switch (child.Evaluate())
            {
                case ReturnState.Success:
                    aState = ReturnState.Success;
                    return aState;
                case ReturnState.Failure:
                    aState = ReturnState.Failure;
                    continue;
                case ReturnState.Running:
                    aState = ReturnState.Running;
                    return aState;
            }
        }

        return aState;
    }
}
