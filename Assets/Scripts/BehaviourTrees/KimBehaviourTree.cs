using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KimBehaviourTree : BehaviourTree
{

    private void Start()
    {
        myBlackBoard = new BlackBoard();

        myRootNode = 
            new Sequence(new List<BehaviourNode> 
            { 
                
            });
        myBlackBoard.data.Add("KimTransform", transform);
        myRootNode.PopulateBlackBoard(myBlackBoard);

    }

    void Update()
    {
        UpdateTree();    
    }

}
