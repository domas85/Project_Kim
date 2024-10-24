using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    public BehaviourNode myRootNode;
    public BlackBoard myBlackBoard;

    public void UpdateTree()
    {
        myRootNode.Evaluate();
    }
}

public class BlackBoard
{
    public Dictionary<string, object> data = new Dictionary<string, object>();
}