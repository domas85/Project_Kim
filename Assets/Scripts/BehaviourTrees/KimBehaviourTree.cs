using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KimBehaviourTree : BehaviourTree
{
    Kim kim;
    public void StartKimBehabiour()
    {
        myBlackBoard = new BlackBoard();
        kim = GetComponent<Kim>();


        myRootNode =
            new Sequence(new List<BehaviourNode>
            {
                new GetClosestBurger(new List<BehaviourNode>()),
                new Sequence(new List<BehaviourNode>
                {
                    new Selector(new List<BehaviourNode>
                    {
                        new MoveToBurger(new List<BehaviourNode>()),
                        new Retreat(new List<BehaviourNode>())
                    })
                }),
                new CheckIfNoMoreBurgers(new List<BehaviourNode>()),
                new Selector(new List<BehaviourNode>
                {
                    new MoveToFinish(new List<BehaviourNode>()),
                    new Retreat(new List<BehaviourNode>())
                }),
            });
        myBlackBoard.data.Add("KimTransform", transform);
        myBlackBoard.data.Add("Kim", kim);
        myBlackBoard.data.Add("Safe", new List<Vector3>() { kim.transform.position });
        myBlackBoard.data.Add("Burgers", kim.allBurgers);
        /*((List<Vector3>)myBlackBoard.data["Safe"]).Add(kim.transform.position);
        if (myBlackBoard.data.TryGetValue("Safe",out var value))
        {
            ((List<Vector3>)value).Add(kim.transform.position);
        }*/
        myRootNode.PopulateBlackBoard(myBlackBoard);

    }

    void Update()
    {
        UpdateTree();
    }

}
