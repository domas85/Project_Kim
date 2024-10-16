using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KimBehaviourTree : BehaviourTree
{
    Kim kim;

    private void Start()
    {
        myBlackBoard = new BlackBoard();
        kim = GetComponent<Kim>();


        myRootNode =
            new Sequence(new List<BehaviourNode>
            {
                new GetClosestBurger(new List<BehaviourNode>()),
                new Sequence(new List<BehaviourNode>
                {
                    new MoveToBurger(new List<BehaviourNode>()),

                })
            });
        myBlackBoard.data.Add("KimTransform", transform);
        myBlackBoard.data.Add("Kim", kim);
        myBlackBoard.data.Add("Burgers", kim.allBurgers);
        myRootNode.PopulateBlackBoard(myBlackBoard);

    }

    void Update()
    {
        UpdateTree();
    }

}
