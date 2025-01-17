using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GetClosestBurger : BehaviourNode
{
    public GetClosestBurger(List<BehaviourNode> someChildren) : base(someChildren)
    {

    }

    List<Burger> allBurgers = new();

    public override ReturnState Evaluate()
    {
        Kim kim = myBlackBoard.data["Kim"] as Kim;
        allBurgers = myBlackBoard.data["Burgers"] as List<Burger>;
        myBlackBoard.data.Remove("BurgersLeft");

        List <List<Node>> burgerPaths = new List<List<Node>>();
        List<List<Node>> sortedBurgerPaths = new List<List<Node>>();

        if (allBurgers != null && allBurgers.Count >= 2)
        {
            for (int i = 0; i < allBurgers.Count; i++)
            {
                if (allBurgers[i].isActiveAndEnabled)
                {
                    kim.FindShortestPathToTarget(kim.transform.position, allBurgers[i].transform.position);
                    burgerPaths.Add(NodeGrid.instance.path);
                }
            }
        }

        sortedBurgerPaths = burgerPaths.OrderBy(go => go.Count).ToList();

        if(myBlackBoard.data.ContainsKey("BurgersLeft") == false && sortedBurgerPaths.Count == 0)
        {
            myBlackBoard.data.Add("BurgersLeft", sortedBurgerPaths);
            return ReturnState.Success;
        }

        if(myBlackBoard.data.ContainsKey("BurgersLeft") == true)
        {
            return ReturnState.Success;
        }

        if (allBurgers.Count >= 1)
        {
            NodeGrid.instance.path = sortedBurgerPaths[0];
            return ReturnState.Success;
        }
        return ReturnState.Failure;
    }
}
