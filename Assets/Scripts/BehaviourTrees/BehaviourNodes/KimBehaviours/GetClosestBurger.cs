using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GetClosestBurger : BehaviourNode
{
    public GetClosestBurger(List<BehaviourNode> someChildren) : base(someChildren)
    {

    }

    List<Burger> allburgers = new();

    public override ReturnState Evaluate()
    {
        Kim kim = myBlackBoard.data["Kim"] as Kim;
        allburgers = myBlackBoard.data["Burgers"] as List<Burger>;
        myBlackBoard.data.Remove("BurgersCollected");

        List <List<Node>> burgerPath = new List<List<Node>>();

        List<List<Node>> SortedBurgerList = new List<List<Node>>();
        if (allburgers != null && allburgers.Count >= 2)
        {
            for (int i = 0; i < allburgers.Count; i++)
            {
                if (allburgers[i].isActiveAndEnabled)
                {
                    kim.FindShortestPathToTarget(kim.transform.position, allburgers[i].transform.position);
                    burgerPath.Add(NodeGrid.instance.path);
                }
            }
        }
        SortedBurgerList = burgerPath.OrderBy(go => go.Count).ToList();
        if(myBlackBoard.data.ContainsKey("BurgersCollected") == false && SortedBurgerList.Count == 0)
        {
            myBlackBoard.data.Add("BurgersCollected", SortedBurgerList);
            return ReturnState.Success;
        }
        if(myBlackBoard.data.ContainsKey("BurgersCollected") == true)
        {
            return ReturnState.Success;
        }
        if (allburgers.Count >= 1)
        {
            NodeGrid.instance.path = SortedBurgerList[0];
            return ReturnState.Success;
        }


        return ReturnState.Failure;
    }

}
