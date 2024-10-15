using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : BehaviourNode
{
    public Sequence(List<BehaviourNode> someChildren) : base(someChildren)
    {

    }

    public override ReturnState Evaluate()
    {
        ReturnState aState = ReturnState.Failure;

        bool isRunning = false;

        foreach(BehaviourNode child in myChildren)
        {
            switch (child.Evaluate())
            {
                case ReturnState.Success:
                    aState = ReturnState.Success;
                    continue;
                case ReturnState.Failure:
                    return aState;
                case ReturnState.Running:
                    isRunning = true;
                    aState = ReturnState.Running;
                    continue;
            }
        }


        if (isRunning) aState = ReturnState.Running; 
        return aState;
    }
}
